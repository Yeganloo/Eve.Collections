namespace CommonLib.Collections.Graph
{
    public struct Node<T>
    {
        public T Value { get; }

        public string Title;

        public Node(T value, string title)
        {
            Title = title;
            Value = value;
        }
        
    }
}
