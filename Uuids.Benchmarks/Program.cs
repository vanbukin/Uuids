using BenchmarkDotNet.Running;

namespace Uuids.Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<UuidGeneratorBenchmarks>();
            BenchmarkRunner.Run<UuidCtorBenchmarks>();
            BenchmarkRunner.Run<UuidToStringBenchmarks>();
            BenchmarkRunner.Run<UuidCommonBenchmarks>();
            BenchmarkRunner.Run<UuidTryParseBenchmarks>();
        }
    }
}