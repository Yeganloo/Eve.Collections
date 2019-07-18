using Eve.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Eve.Collections.Graph
{
    public class Graph<TNode, TEdge> : IEnumerable<Node<TNode>>
    {
        #region Lockers

        private readonly object Locker_Global = new object();

        #endregion

        private readonly DynamicArray<Node<TNode>> _Nodes;
        private readonly DynamicArray<DynamicArray<KeyValuePair<int, TEdge>>> Neigbors;
        private int _AverageEdges = 4;
        public bool Directed { get; }


        private int _Count;
        public int Count
        {
            get
            {
                return _Count;
            }
        }

        #region Init

        public Graph(bool directed)
        {
            _Nodes = new DynamicArray<Node<TNode>>();
            Neigbors = new DynamicArray<DynamicArray<KeyValuePair<int, TEdge>>>();
            Directed = directed;
        }
        public Graph(bool directed, int count)
        {
            int bufs = (int)Math.Sqrt(count) + 4;
            _Nodes = new DynamicArray<Node<TNode>>(bufs);
            Neigbors = new DynamicArray<DynamicArray<KeyValuePair<int, TEdge>>>(bufs);
            Directed = directed;
        }

        #endregion

        public Node<TNode> this[int id]
        {
            get
            {
                return _Nodes[id];
            }
            set
            {
                lock (Neigbors)
                {
                    Neigbors[id] = new DynamicArray<KeyValuePair<int, TEdge>>(_AverageEdges);
                    _Nodes[id] = value;
                    _Count++;
                    _AverageEdges = (int)Math.Sqrt(Count + 16);
                }
            }
        }

        public Node<TNode> Set(int id, TNode value)
        {
            var n = new Node<TNode>(value, id.ToString());
            this[id] = n;
            return n;
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
                lock (Neigbors)
                {
                    src.Add(new KeyValuePair<int, TEdge>(destination, value));
                    dst.Add(new KeyValuePair<int, TEdge>(source, value));
                }
            }
        }

        #region Intefaces

        public IEnumerator<Node<TNode>> GetEnumerator()
        {
            return _Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Nodes.GetEnumerator();
        }

        #endregion
    }
}
