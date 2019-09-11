using BenchmarkDotNet.Running;

namespace Uuid.Benchmarks
{
    public static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<UuidBenchmarks>();
        }
    }
}