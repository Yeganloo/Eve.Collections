using System.Collections;
using System.Collections.Generic;
using System;


namespace Eve.Collections
{
  public class NamedList<T> : IEnumerable<KeyValuePair<string, T>>
  {
    public NamedList()
    {
      Data = new DynamicArray<KeyValuePair<string, T>>();
    }

    DynamicArray<KeyValuePair<string, T>> Data;

    private int BinarySearch(string name)
    {
      int min = 0, max = Data.Length - 1, mid = 0;
      while (min <= max)
      {
        mid = (min + max) / 2;
        var cp = name.CompareTo(Data[mid].Key);
        // Check if x is present at mid 
        if (cp == 0)
          return mid;

        // If x greater, ignore left half 
        if (cp > 0)
        {
          min = mid + 1;
        }
        // If x is smaller, ignore right half 
        else
        {
          if (min == max)
            return min;
          max = mid - 1;
        }
      }

      // if we reach here, then element was 
      // not present 
      return min;
    }

    public bool ContainsKey(string name)
    {
      var index = BinarySearch(name);
      return index < Data.Length && Data[index].Key == name;
    }

    public T this[string name]
    {
      get
      {
        if (Data.Count == 0)
          throw new KeyNotFoundException();
        var index = BinarySearch(name);
        if (index >= Data.Count)
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
      else
      {
        Data.Insert(BinarySearch(name), new KeyValuePair<string, T>(name, value));
      }
    }

    public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
    {
      return this.Data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.Data.GetEnumerator();
    }
  }
}
