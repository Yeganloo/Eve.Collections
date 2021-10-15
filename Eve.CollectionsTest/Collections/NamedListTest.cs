using Xunit;
using Eve.Collections;
using System;


namespace Eve.CollectionsTest
{
  [Collection("Non-Parallel")]
  public class NamedListTest
  {
    private const int Round = 90000;

    [Fact]
    public void _Sequence_ReadWrite()
    {
      var list = new NamedList<object>();
      for (int i = 0; i < Round; i++)
      {
        list.Add(i.ToString(), i);
      }
      for (int i = 0; i < Round; i++)
      {
        Assert.True(i == (int)list[i.ToString()]);
      }
    }
  }
}
