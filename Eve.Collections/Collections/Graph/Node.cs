using Eve.Collections;
using System.Collections.Generic;

namespace CommonLib.Collections.Graph
{
    public struct Node<T>
    {
        public T Value { get; }

        //public IList<Node<T>> Neighbor;

        public string Title;

        public Node(T value, string title)
        {
            //Neighbor = new DynamicArray<Node<T>>(10);
            Title = title;
            Value = value;
        }

        //public Node(T value, string title, IList<Node<T>> neighbor)
        //{
        //    Neighbor = neighbor;
        //    Title = title;
        //    Value = value;
        //}
    }
}
