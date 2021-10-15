﻿using Eve.Collections.Graph;
using Xunit;
using System;

namespace Eve.CollectionsTest.Graph
{
    [Collection("Non-Parallel")]
    public class GraphTest
    {
        private const int Round = 500000;

        [Fact]
        public void _Sequence_ReadWrite()
        {
            var graph = new Graph<object>(false, Round);
            var random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < Round; i++)
            {
                graph.Set(i, i);
            }
            for (int i = 0; i < Round * 4.5; i++)
            {
                graph.AddEdge(random.Next(0, Round - 1), random.Next(0, Round - 1));
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(i == (int)graph[i].Value);
            }
        }

    }
}
