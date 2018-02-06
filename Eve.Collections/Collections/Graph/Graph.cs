using Eve.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Collections.Graph
{
    public class Graph<T> : IEnumerable<Node<T>>
    {
        private DynamicArray<Node<T>> _Nodes;

        public int Count { get; private set; }

        #region Init

        public Graph()
        {
            _Nodes = new DynamicArray<Node<T>>();
        }
        public Graph(int count)
        {
            _Nodes = new DynamicArray<Node<T>>((int)Math.Sqrt(count) + 1);
            Count = count;
        }

        #endregion

        public Node<T> this[int id]
        {
            get
            {
                return _Nodes[id];
            }
        }

        public Node<T> Add(T value)
        {
            lock (_Nodes)
            {
                var n = new Node<T>(value, Count.ToString());
                _Nodes.Add(n);
                return n;
            }
        }

        #region Intefaces

        public IEnumerator<Node<T>> GetEnumerator()
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
