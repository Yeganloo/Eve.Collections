using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eve.Collections.Graph
{
    public class Graph<TNode> : GraphBase<TNode, bool>
    {
        protected readonly object GLock = new object();
        protected readonly IDynamicArray<IDynamicArray<int>> _Neighbors;

        #region Init

        public Graph(bool directed) : base(directed)
        {
            _Neighbors = new DynamicArray<IDynamicArray<int>>(_AverageEdges);
        }
        public Graph(bool directed, int count) : base(directed, count)
        {
            _Neighbors = new DynamicArray<IDynamicArray<int>>(count);
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
                    _Neighbors[id] = new DynamicArray<int>(_AverageEdges);
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
            if (Directed)
            {
                var src = _Neighbors[source];
                lock (src)
                {
                    src.Add(destination);
                }
            }
            else
            {
                var src = _Neighbors[source];
                var dst = _Neighbors[destination];
                lock (GLock)
                {
                    src.Add(destination);
                    dst.Add(source);
                }
            }
        }

        public override void Clear()
        {
            lock (GLock)
            {
                _Nodes.Clear();
                _Neighbors.Clear();
                Count = 0;
            }
        }

        public override IEnumerable<Node<TNode>> GetNeighbors(int nodeId)
        {
            foreach (var n in _Neighbors[nodeId])
                yield return _Nodes[n];
        }

        public override bool AreNeighbor(int node1, int node2)
        {
            return _Neighbors[node1].Contains(node2);
        }

        public override void RemoveNode(int id)
        {
            lock (GLock)
            {
                _Nodes.RemoveAt(id);
                if (Directed)
                {
                    _Neighbors.RemoveAt(id);
                    foreach (var i in _Neighbors)
                    {
                        while (i.Remove(id)) ;
                    }
                }
                else
                {
                    foreach (var i in _Neighbors[id])
                    {
                        _Neighbors[i].Remove(id);
                    }
                    _Neighbors.RemoveAt(id);
                }
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
            var res = new Graph<TNode>(Directed, Count);
            res._Nodes = _Nodes.Clone();
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
