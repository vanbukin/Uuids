using BenchmarkDotNet.Running;

namespace Uuids.Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<CtorBenchmarks>();
            BenchmarkRunner.Run<GeneratorBenchmarks>();
            BenchmarkRunner.Run<ImplementedInterfacesBenchmarks>();
            BenchmarkRunner.Run<InstanceMethodsBenchmarks>();
            BenchmarkRunner.Run<OverridesBenchmarks>();
            BenchmarkRunner.Run<ToStringBenchmarks>();
            BenchmarkRunner.Run<TryParseBenchmarks>();
        }
    }
}