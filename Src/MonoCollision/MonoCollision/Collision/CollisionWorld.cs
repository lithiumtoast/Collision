using System;
using Microsoft.Xna.Framework;
using MonoCollision.Memory;

namespace MonoCollision
{
    public class CollisionWorld
    {
        private readonly Broadphase _broadphase;
        private readonly CollisionNarrowphase _narrowphase;
        
        private uint _collidersCount;

        private readonly UnsafeArray<BroadphaseShape> _broadphaseShapes;
        private readonly UnsafeArray<BroadphaseCollisionPair> _broadphaseCollisionPairs;

        public CollisionWorld(CollisionWorldDescriptor? descriptor = null)
        {
            var desc = descriptor ?? new CollisionWorldDescriptor
            {
                MaximumBroadphaseCollisionPairs = 1000,
                MaximumBroadphaseShapes = 1000
            };
            
            _broadphase = new BroadphaseBruteForce();
            _narrowphase = new CollisionNarrowphasePassThrough();
            _broadphaseShapes = new UnsafeArray<BroadphaseShape>(desc.MaximumBroadphaseShapes);
            _broadphaseCollisionPairs = new UnsafeArray<BroadphaseCollisionPair>(desc.MaximumBroadphaseCollisionPairs);
        }

        public Collider CreateCollider<TBroadphaseShape>(object target, TBroadphaseShape shape) where TBroadphaseShape : struct, BroadphaseShapeMarker
        {
            var broadphaseCollider = default(BroadphaseCollider);
            var index = _collidersCount++;

            switch (shape)
            {
                case BroadphaseRectangle rectangle:
                    broadphaseCollider.Shape.Type = BroadphaseShapeType.Rectangle;
                    broadphaseCollider.Shape.Rectangle = rectangle;
                    break;
                case BroadphaseCircle circle:
                    broadphaseCollider.Shape.Type = BroadphaseShapeType.Circle;
                    broadphaseCollider.Shape.Circle = circle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape));
            }
            
            _broadphaseShapes[index] = broadphaseCollider.Shape;

            var collider = new Collider(this, index);
            return collider;
        }

        public void MoveCollider(Collider collider, BroadphaseRectangle rectangle)
        {
        }

        public void DestroyCollider(Collider collider)
        {
        }

        private bool _once;
        
        public void Update(GameTime gameTime)
        {
            if (!_once)
            {
                _once = true;
            }
            else
            {
                return;
            }
            
            var shapes = _broadphaseShapes.GetSpan(0, _collidersCount);
            var pairs = _broadphaseCollisionPairs.GetSpan();
            
            var pairCount = _broadphase.FindCollidingShapes(shapes, pairs);
            var collidingPairs = _broadphaseCollisionPairs.GetSpan(0, pairCount);

            foreach (var pair in collidingPairs)
            {
                Console.Write(@$"pair: ({pair.FirstIndex}, {pair.SecondIndex})");
            }
        }
    }
}
