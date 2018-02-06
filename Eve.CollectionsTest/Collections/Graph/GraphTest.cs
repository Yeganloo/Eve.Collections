using NUnit.Framework;
using Eve.Collections;
using CommonLib.Collections.Graph;

namespace Eve.CollectionsTest.Graph
{
    [TestFixture()]
    public class GraphTest
    {
        private const int Round = 3000000;

        [Test()]
        public void _()
        {

        }

        [Test()]
        public void _Sequenc_ReadWrite()
        {
            var graph = new Graph<object>();
            for (int i = 0; i < Round; i++)
            {
                graph.Add(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.IsTrue(i == (int)graph[i].Value);
            }
        }

    }
}
