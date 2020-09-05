using System;

namespace MonoCollision
{
    public class BroadphaseBruteForce : Broadphase
    {
        public override uint FindCollidingShapes(Span<BroadphaseShape> shapes, Span<BroadphaseCollisionPair> pairs)
        {
            var pairCount = 0;
            var shapeCount = shapes.Length;
            for (var i = 0; i < shapeCount; i++)
            {
                for (var j = 0; j < shapeCount; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    
                    ref var first = ref shapes[i];
                    ref var second = ref shapes[j];

                    if (ShapesIntersect(ref first, ref second))
                    {
                        if (pairCount < pairs.Length)
                        {
                            var pair = new BroadphaseCollisionPair((uint)i, (uint)j);
                            pairs[pairCount++] = pair;   
                        }
                    }
                }
            }

            return (uint)pairCount;
        }
    }
}
