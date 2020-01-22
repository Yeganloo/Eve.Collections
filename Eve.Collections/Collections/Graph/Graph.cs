using System;
using System.Collections.Generic;

namespace Eve.Collections.Graph
{
    public class Graph<TNode> : GraphBase<TNode, bool>
    {
        protected object GLock = new object();
        protected DynamicArray<DynamicArray<int>> _Neigbors;

        #region Init

        public Graph(bool directed) : base(directed)
        {
            _Neigbors = new DynamicArray<DynamicArray<int>>(_AverageEdges);
        }
        public Graph(bool directed, int count) : base(directed, count)
        {
            _Neigbors = new DynamicArray<DynamicArray<int>>(_AverageEdges);
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
                    _Neigbors[id] = new DynamicArray<int>(_AverageEdges);
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
                var src = _Neigbors[source];
                lock (src)
                {
                    src.Add(destination);
                }
            }
            else
            {
                var src = _Neigbors[source];
                var dst = _Neigbors[destination];
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
                _Neigbors.Clear();
                Count = 0;
            }
        }

        public override IEnumerable<Node<TNode>> GetNeigbors(int nodeId)
        {
            foreach (var n in _Neigbors[nodeId])
                yield return _Nodes[n];
        }

        public override bool AreNeigbor(int node1, int node2)
        {
            return _Neigbors[node1].Contains(node2);
        }

        public override void RemoveNode(int id)
        {
            lock (GLock)
            {
                _Nodes.RemoveAt(id);
                if (Directed)
                {
                    foreach (var i in _Neigbors)
                    {
                        while (i.Remove(id)) ;
                    }
                }
                else
                    foreach (var i in _Neigbors[id])
                    {
                        _Neigbors[i].Remove(id);
                    }
                _Neigbors.RemoveAt(id);
            }
        }

        public override void RemoveEdge(int src, int dst)
        {
            lock (GLock)
            {
                _Neigbors[src].Remove(dst);
                if (!Directed)
                    _Neigbors[dst].Remove(src);
            }
        }

        public override bool[,] Adjacency()
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            var res = new Graph<TNode>(Directed, 1);
            res._Nodes = _Nodes.Clone();
            res._Neigbors = new DynamicArray<DynamicArray<int>>(Count);
            for (int i = 0; i < Count; i++)
                res._Neigbors[i] = _Neigbors[i]?.Clone();
            return res;
        }

        public override T SubGraph<T>(IEnumerable<int> nodes)
        {
            throw new NotImplementedException();
        }
    }
}
