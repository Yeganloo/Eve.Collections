using System;
using System.Collections.Generic;
using System.Text;

namespace Eve.Collections
{
    public class NamedList<T>
    {
        public NamedList()
        {
            Data = new DynamicArray<KeyValuePair<string, T>>();
        }

        DynamicArray<KeyValuePair<string, T>> Data;

        private int BinarySearch(string name)
        {
            int min = 0, max = Data.Length - 1;
            while (min < max)
            {
                int mid = (min + max) / 2;
                var cp = name.CompareTo(Data[mid].Key);
                // Check if x is present at mid 
                if (cp == 0)
                    return mid;

                // If x greater, ignore left half 
                if (cp < 0)
                {
                    if (min == max)
                        return min;
                    min = mid + 1;
                }

                // If x is smaller, ignore right half 
                else
                    max = mid - 1;
            }

            // if we reach here, then element was 
            // not present 
            return min;
        }

        public bool ContainsKey(string name)
        {
            //TODO
            return false;
        }

        public T this[string name]
        {
            get
            {
                if (Data.Count == 0)
                    throw new KeyNotFoundException();
                var index = BinarySearch(name);
                if (index > Data.Count - 1)
                    throw new KeyNotFoundException();
                var tmp = Data[index];
                if (tmp.Key != name)
                    throw new KeyNotFoundException();
                return tmp.Value;
            }
        }

        public void Add(string name, T value)
        {
            if (Data.Count == 0)
                Data.Add(new KeyValuePair<string, T>(name, value));
            Data.Insert(BinarySearch(name), new KeyValuePair<string, T>(name, value));
        }

    }
}
