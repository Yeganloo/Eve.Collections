using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eve.Collections.Graph
{
    public class WeightedGraph<TNode, TEdge> : GraphBase<TNode, TEdge>
    {
        private object GLock = new object();
        protected DynamicArray<DynamicArray<KeyValuePair<int, TEdge>>> _Neighbors;

        #region Init

        public WeightedGraph(bool directed) : base(directed)
        {
            h = new DynamicArray<DynamicArray<KeyValuePair<int, TEdge>>>(_AverageEdges);
        }
        public WeightedGraph(bool directed, int count) : base(directed, count)
        {
            h = new DynamicArray<DynamicArray<KeyValuePair<int, TEdge>>>(_AverageEdges);
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
                    h[id] = new DynamicArray<KeyValuePair<int, TEdge>>(_AverageEdges);
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
                var src = h[source];
                lock (src)
                {
                    src.Add(new KeyValuePair<int, TEdge>(destination, value));
                }
            }
            else
            {
                var src = h[source];
                var dst = h[destination];
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
                h.Clear();
                Count = 0;
            }
        }

        public override IEnumerable<Node<TNode>> GetNeighbors(int nodeId)
        {
            foreach (var n in h[nodeId])
                yield return _Nodes[n.Key];
        }

        public override bool AreNeighbor(int node1, int node2)
        {
            foreach (var n in h[node1])
                if (n.Key == node2)
                    return true;
            return false;
        }

        public override void RemoveNode(int id)
        {
            lock (GLock)
            {
                _Nodes.RemoveAt(id);
                if (Directed)
                {
                    foreach (var i in h)
                    {
                        while (i.Remove(id)) ;
                    }
                }
                else
                    foreach (var i in h[id])
                    {
                        h[i].Remove(id);
                    }
                h.RemoveAt(id);
            }
        }

        public override void RemoveEdge(int src, int dst)
        {
            lock (GLock)
            {
                h[src].Remove(dst);
                if (!Directed)
                    h[dst].Remove(src);
            }
        }

        public override TEdge[,] Adjacency()
        {
            var ad = new TEdge[Count, Count];
            lock (GLock)
            {
                Parallel.For(0, Count, (i) =>
                {
                    foreach (var item in h[i])
                        ad[i, item.Key] = item.Value;
                });
            }
            return ad;
        }

        //TODO Write Test
        public override object Clone()
        {
            var res = new WeightedGraph<TNode, TEdge>(Directed, 1);
            res._Nodes = _Nodes.Clone();
            res.h = new DynamicArray<DynamicArray<KeyValuePair<int, TEdge>>>(Count);
            Parallel.For(0, Count, (i) =>
            {
                res.h[i] = h[i]?.Clone();
            });
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
