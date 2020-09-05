using Microsoft.Xna.Framework;

namespace MonoCollision
{
    public struct BroadphaseRectangle : BroadphaseShapeMarker
    {
        public Vector2 Minimum;
        public Vector2 Maximum;
    }
}
