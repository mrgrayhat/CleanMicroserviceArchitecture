using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace SaeedRezayi.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Benchmarks...");

            var summary = BenchmarkRunner.Run<ListBenchmarks>();

        }

        [MemoryDiagnoser]
        public class ListBenchmarks
        {
            List<string> FixedList = new List<string>(10);
            List<string> DynamicList = new List<string>();

            [Benchmark(Baseline = true)]
            public void TestFixedList()
            {
                for (int i = 0; i < FixedList.Capacity; i++)
                {
                    FixedList.Add("dynamic item");
                }

            }

            [Benchmark]
            public void TestDynamicList()
            {
                for (int i = 0; i < 10; i++)
                {
                    DynamicList.Add("dynamic item");

                }
            }

        }
    }
}
