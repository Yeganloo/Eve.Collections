
namespace Eve.Collections.Graph
{
  public class DirectedGraph<TNode> : Graph<TNode>
  {
    #region Init

    public DirectedGraph() : base(true)
    {
    }
    public DirectedGraph(int count) : base(true, count)
    {
    }

    #endregion

    public override void AddEdge(int source, int destination)
    {
      var src = _Neighbors[source];
      lock (src)
      {
        src.Add(destination);
      }
    }

    // BUG This is wrong!
    public override void RemoveNode(int id)
    {
      lock (GLock)
      {
        _Nodes.RemoveAt(id);
        foreach (var i in _Neighbors)
        {
          while (i.Remove(id)) ;
        }
        _Neighbors.RemoveAt(id);
      }
    }
    
    public override void RemoveEdge(int src, int dst)
    {
      lock (GLock)
      {
        _Neighbors[src].Remove(dst);
        _Neighbors[dst].Remove(src);
      }
    }

  }
}
