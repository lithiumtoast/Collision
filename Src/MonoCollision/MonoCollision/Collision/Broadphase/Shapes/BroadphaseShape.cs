using System.Runtime.InteropServices;

namespace MonoCollision
{
    [StructLayout(LayoutKind.Explicit)]
    public struct BroadphaseShape
    {
        [FieldOffset(0)]
        internal BroadphaseShapeType Type;
        [FieldOffset(4)]
        public BroadphaseRectangle Rectangle;
        [FieldOffset(4)]
        public BroadphaseCircle Circle;
    }
}