using System;
using System.Collections;
using System.Collections.Generic;

namespace Eve.Collections.Graph
{
    public abstract class GraphBase<TNode, TEdge> : IEnumerable<Node<TNode>>
    {
        protected const int Growth = 1024;
        protected readonly DynamicArray<Node<TNode>> _Nodes;
        protected int _AverageEdges = Growth;
        public bool Directed { get; }

        public int Count { get; protected set; }

        #region Init

        public GraphBase(bool directed)
        {
            _Nodes = new DynamicArray<Node<TNode>>(_AverageEdges);
            Directed = directed;
        }

        public GraphBase(bool directed, int count)
        {
            _Nodes = new DynamicArray<Node<TNode>>(_AverageEdges = (int)Math.Ceiling(Math.Sqrt(count + Growth)));
            Directed = directed;
        }

        #endregion

        public abstract Node<TNode> this[int id]
        {
            get;
            set;
        }

        public abstract Node<TNode> Set(int id, TNode value);

        public abstract void AddEdge(int source, int destination);

        public abstract void RemoveNode(int id);

        public abstract void RemoveEdge(int source, int destination);

        public abstract IEnumerable<Node<TNode>> GetNeigbors(int nodeId);

        public abstract bool AreNeigbor(int node1, int node2);

        public abstract TEdge[,] Adjacency();

        public abstract T Clone<T>() where T : GraphBase<TNode, TEdge>;

        public abstract T SubGraph<T>(IEnumerable<int> nodes) where T : GraphBase<TNode, TEdge>;

        public abstract void Clear();

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
