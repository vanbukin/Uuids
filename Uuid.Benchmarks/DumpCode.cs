using BenchmarkDotNet.Attributes;

namespace Uuid.Benchmarks
{
    [DryCoreJob]
    [DisassemblyDiagnoser]
    public class DumpCode
    {
        [Benchmark]
        public double Sum()
        {
            double res = 0;
            for (int i = 0; i < 64; i++)
                res += i;
            return res;
        }
    }
}