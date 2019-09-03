using BenchmarkDotNet.Running;

namespace Uuid.Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<UuidBenchmarks>();
        }
    }
}