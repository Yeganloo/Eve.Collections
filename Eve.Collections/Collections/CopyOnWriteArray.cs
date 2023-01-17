namespace Eve.Collections
{
    public class CopyOnWriteArray<T>
    {
        public static CopyOnWriteArray<T> CopyFrom(DynamicArray<T> array)
        {
            return new CopyOnWriteArray<T>(new DynamicArray<T>(array));
        }
        public static CopyOnWriteArray<T> ConvertFrom(DynamicArray<T> array)
        {
            return new CopyOnWriteArray<T>(array);
        }
        public CopyOnWriteArray(int capacity = 1024)
        {
            _array = new DynamicArray<T>(capacity);
        }
        private CopyOnWriteArray(DynamicArray<T> from)
        {
            this._version = 0;
            this._array = from;
            this._versions = new DynamicArray<T>[10];
        }
        private int _max_copy;
        private uint _version;
        private DynamicArray<T> _array;
        private DynamicArray<T>[] _versions;
    }
}