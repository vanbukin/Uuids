using BenchmarkDotNet.Running;

namespace Uuid.Benchmarks
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<DumpCode>();
        }
    }
}