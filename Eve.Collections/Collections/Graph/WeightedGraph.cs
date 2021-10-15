using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eve.Collections.Graph
{
  public class WeightedGraph<TNode, TEdge> : GraphBase<TNode, TEdge>
  {
    protected readonly object GLock = new object();
    protected readonly DynamicArray<DynamicArray<KeyValuePair<int, int>>> _Neighbors;
    protected readonly DynamicArray<TEdge> _Edges;

    #region Init

    public WeightedGraph(bool directed) : base(directed)
    {
      _Neighbors = new DynamicArray<DynamicArray<KeyValuePair<int, int>>>(_AverageEdges);
      _Edges = new DynamicArray<TEdge>(Growth);
    }
    public WeightedGraph(bool directed, int count) : base(directed, count)
    {
      _Neighbors = new DynamicArray<DynamicArray<KeyValuePair<int, int>>>(_AverageEdges);
      _Edges = new DynamicArray<TEdge>(count);
    }

    #endregion

    public override Node<TNode> this[int id]
    {
      get
      {
        return _Nodes[id];
      }
      set
      {
        lock (GLock)
        {
          _Nodes[id] = value;
          _Neighbors[id] = new DynamicArray<KeyValuePair<int, int>>(_AverageEdges);
          Count = id > Count ? id : Count;
        }
        _AverageEdges = (int)Math.Max(Math.Sqrt(Count + Growth), _AverageEdges);
      }
    }

    public override Node<TNode> Set(int id, TNode value)
    {
      var n = new Node<TNode>(value, id.ToString());
      this[id] = n;
      return n;
    }

    public override void AddEdge(int source, int destination)
    {
      AddEdge(source, destination, default(TEdge));
    }

    public void AddEdge(int source, int destination, TEdge value)
    {
      var src = _Neighbors[source];
      var i = _Edges.Count;
      lock (GLock)
      {
        _Edges.Add(value);
        src.Add(new KeyValuePair<int, int>(destination, i));
        if (!Directed)
          _Neighbors[destination].Add(new KeyValuePair<int, int>(source, i));
      }

    }

    public override void Clear()
    {
      lock (GLock)
      {
        _Nodes.Clear();
        _Neighbors.Clear();
        _Edges.Clear();
        Count = 0;
      }
    }

    public override IEnumerable<Node<TNode>> GetNeighbors(int nodeId)
    {
      foreach (var n in _Neighbors[nodeId])
        yield return _Nodes[n.Key];
    }

    public override bool AreNeighbor(int node1, int node2)
    {
      return _Neighbors[node1].Any(q => q.Key == node2);
    }

    // TODO
    public override void RemoveNode(int id)
    {
      lock (GLock)
      {
        _Nodes.RemoveAt(id);
        // BUG This is wrong!
        if (Directed)
        {
          foreach (var i in _Neighbors)
          {
            while (i.Remove(id)) ;
          }
        }
        else
          foreach (var i in _Neighbors[id])
          {
            _Neighbors[i].Remove(id);
          }
        _Neighbors.RemoveAt(id);
      }
    }

    public override void RemoveEdge(int src, int dst)
    {
      lock (GLock)
      {
        _Neighbors[src].Remove(dst);
        if (!Directed)
          _Neighbors[dst].Remove(src);
      }
    }

    public override bool[,] Adjacency()
    {
      var ad = new bool[Count, Count];
      lock (GLock)
      {
        Parallel.For(0, Count, (i) =>
        {
          foreach (var id in _Neighbors[i])
            ad[i, id] = true;
        });
      }
      return ad;
    }

    public override object Clone()
    {
      var res = new Graph<TNode>(Directed, 1);
      res._Nodes = _Nodes.Clone();
      res._Neighbors = new DynamicArray<DynamicArray<int>>(Count);
      for (int i = 0; i < Count; i++)
        res._Neighbors[i] = _Neighbors[i]?.Clone();
      res.Count = Count;
      return res;
    }

    //TODO Implement
    public override T SubGraph<T>(IEnumerable<int> nodes)
    {
      throw new NotImplementedException();
    }
  }
}
