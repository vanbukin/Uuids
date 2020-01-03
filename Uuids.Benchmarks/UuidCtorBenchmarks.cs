using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Uuids.Benchmarks
{
    [MemoryDiagnoser]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    public unsafe class UuidCtorBenchmarks
    {
        private byte* _guidBytesPtr = GetGuidBytesPtr();
#pragma warning disable 649
        private byte[] _guidBytesArray = new byte[sizeof(Guid)];
        private byte[] _uuidBytesArray = new byte[sizeof(Uuid)];
#pragma warning restore 649
        private string _guidStringP = GetGuidString("P");
        private string _guidStringB = GetGuidString("B");
        private string _guidStringX = GetGuidString("X");
        private string _guidStringD = GetGuidString("D");
        private string _guidStringN = GetGuidString("N");

        [GlobalSetup]
        public void Setup()
        {
            var guid = Guid.NewGuid();
            _guidBytesArray = guid.ToByteArray();
            _uuidBytesArray = new byte[sizeof(Uuid)];
            fixed (byte* pinnedGuidArray = _uuidBytesArray)
            {
                *(Guid*) pinnedGuidArray = guid;
            }
        }

        private static byte* GetGuidBytesPtr()
        {
            var resultPtr = (byte*) Marshal.AllocHGlobal(sizeof(Guid));
            var guidResultPtr = (Guid*) resultPtr;
            // ReSharper disable once PossibleNullReferenceException
            guidResultPtr[0] = Guid.NewGuid();
            return resultPtr;
        }

        private static string GetGuidString(string format)
        {
            return Guid.NewGuid().ToString(format);
        }

        // byte[]
        [Benchmark]
        public void uuid_CtorByteArray()
        {
            var _ = new Uuid(_guidBytesArray);
        }

        [Benchmark]
        public void guid_CtorByteArray()
        {
            var _ = new Guid(_guidBytesArray);
        }

        // ReadOnlySpan<byte>
        [Benchmark]
        public void uuid_CtorSpan()
        {
            var readOnlySpan = new ReadOnlySpan<byte>(_guidBytesPtr, sizeof(Uuid));
            var _ = new Uuid(readOnlySpan);
        }

        [Benchmark]
        public void guid_CtorSpan()
        {
            var readOnlySpan = new ReadOnlySpan<byte>(_guidBytesPtr, sizeof(Guid));
            var _ = new Guid(readOnlySpan);
        }

        // string - P
        [Benchmark]
        public void uuid_CtorStringP()
        {
            var _ = new Uuid(_guidStringP);
        }

        [Benchmark]
        public void guid_CtorStringP()
        {
            var _ = new Guid(_guidStringP);
        }

        // string - B
        [Benchmark]
        public void uuid_CtorStringB()
        {
            var _ = new Uuid(_guidStringB);
        }

        [Benchmark]
        public void guid_CtorStringB()
        {
            var _ = new Guid(_guidStringB);
        }

        // string - X
        [Benchmark]
        public void uuid_CtorStringX()
        {
            var _ = new Uuid(_guidStringX);
        }

        [Benchmark]
        public void guid_CtorStringX()
        {
            var _ = new Guid(_guidStringX);
        }

        // string - D

        [Benchmark]
        public void uuid_CtorStringD()
        {
            var _ = new Uuid(_guidStringD);
        }

        [Benchmark]
        public void guid_CtorStringD()
        {
            var _ = new Guid(_guidStringD);
        }

        // string - N
        [Benchmark]
        public void uuid_CtorStringN()
        {
            var _ = new Uuid(_guidStringN);
        }

        [Benchmark]
        public void guid_CtorStringN()
        {
            var _ = new Guid(_guidStringN);
        }
    }
}