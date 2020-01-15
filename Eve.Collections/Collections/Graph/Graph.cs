using System;
using System.Collections.Generic;

namespace Eve.Collections.Graph
{
    public class Graph<TNode> : GraphBase<TNode, int>
    {
        protected object GLock = new object();
        protected readonly DynamicArray<DynamicArray<int>> Neigbors;

        #region Init

        public Graph(bool directed) : base(directed)
        {
            Neigbors = new DynamicArray<DynamicArray<int>>(_AverageEdges);
        }
        public Graph(bool directed, int count) : base(directed, count)
        {
            Neigbors = new DynamicArray<DynamicArray<int>>(_AverageEdges);
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
                    Neigbors[id] = new DynamicArray<int>(_AverageEdges);
                    _Nodes[id] = value;
                    Count++;
                }
                _AverageEdges = (int)Math.Sqrt(Count + Growth);
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
                var src = Neigbors[source];
                lock (src)
                {
                    src.Add(destination);
                }
            }
            else
            {
                var src = Neigbors[source];
                var dst = Neigbors[destination];
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
                Neigbors.Clear();
                Count = 0;
            }
        }

        public override IEnumerable<Node<TNode>> GetNeigbors(int nodeId)
        {
            foreach (var n in Neigbors[nodeId])
                yield return _Nodes[n];
        }

        public override bool AreNeigbor(int node1, int node2)
        {
            return Neigbors[node1].Contains(node2);
        }
    }
}
