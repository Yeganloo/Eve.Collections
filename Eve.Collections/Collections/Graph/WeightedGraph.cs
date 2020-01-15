using System;
using System.Collections.Generic;

namespace Eve.Collections.Graph
{
    public class WeightedGraph<TNode, TEdge> : GraphBase<TNode, TEdge>
    {
        private object GLock = new object();
        protected readonly DynamicArray<DynamicArray<KeyValuePair<int, TEdge>>> Neigbors;

        #region Init

        public WeightedGraph(bool directed) : base(directed)
        {
            Neigbors = new DynamicArray<DynamicArray<KeyValuePair<int, TEdge>>>(_AverageEdges);
        }
        public WeightedGraph(bool directed, int count) : base(directed, count)
        {
            Neigbors = new DynamicArray<DynamicArray<KeyValuePair<int, TEdge>>>(_AverageEdges);
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
                    Neigbors[id] = new DynamicArray<KeyValuePair<int, TEdge>>(_AverageEdges);
                    _Nodes[id] = value;
                    Count++;
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
            AddEdge(source, destination, default);
        }

        public void AddEdge(int source, int destination, TEdge value)
        {
            if (Directed)
            {
                var src = Neigbors[source];
                lock (src)
                {
                    src.Add(new KeyValuePair<int, TEdge>(destination, value));
                }
            }
            else
            {
                var src = Neigbors[source];
                var dst = Neigbors[destination];
                lock (GLock)
                {
                    src.Add(new KeyValuePair<int, TEdge>(destination, value));
                    dst.Add(new KeyValuePair<int, TEdge>(source, value));
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
                yield return _Nodes[n.Key];
        }

        public override bool AreNeigbor(int node1, int node2)
        {
            foreach (var n in Neigbors[node1])
                if (n.Key == node2)
                    return true;
            return false;
        }
    }
}
