using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MonoCollision.Memory
{
    public unsafe struct UnsafeArray<T> : IDisposable where T : unmanaged
    {
        public readonly uint Capacity;

        private readonly void* _pointer;
        private readonly uint _typeSize;

        public UnsafeArray(uint capacity)
        {
            Capacity = capacity;
            _typeSize = (uint)Marshal.SizeOf<T>();
            var sizeInBytes = _typeSize * capacity;
            _pointer = (void*)Marshal.AllocHGlobal((IntPtr)sizeInBytes);
        }
        
        public ref T this[uint index]
        {
            get
            {
                var pointer = (T*)((UIntPtr)_pointer + (int)(index * _typeSize));
                return ref *pointer;
            }
        }
        
        public readonly Span<T> GetSpan()
        {
            var pointer = (void*)((UIntPtr)_pointer);
            return new Span<T>(pointer, (int)Capacity);
        }

        public readonly Span<T> GetSpan(uint offset, uint length)
        {
            var pointer = (void*)((UIntPtr)_pointer + (int)(offset * _typeSize));
            return new Span<T>(pointer, (int)length);
        }

        public readonly void Clear()
        {
            Unsafe.InitBlockUnaligned(_pointer, 0, Capacity);
        }

        public readonly void Dispose()
        {
            Marshal.FreeHGlobal((IntPtr)_pointer);
        }
    }
}
