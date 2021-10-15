using System;

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
        
    }
}
