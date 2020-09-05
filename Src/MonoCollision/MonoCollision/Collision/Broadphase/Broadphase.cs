using System;
using Microsoft.Xna.Framework;

namespace MonoCollision
{
    public abstract class Broadphase
    {
        protected static bool ShapesIntersect(ref BroadphaseShape first, ref BroadphaseShape second)
        {
            var typeA = first.Type;
            var typeB = second.Type;

            var intersects = typeA switch
            {
                BroadphaseShapeType.Rectangle when typeB == BroadphaseShapeType.Rectangle => 
                    RectangleRectangleIntersects(ref first.Rectangle, ref second.Rectangle),
                BroadphaseShapeType.Rectangle when typeB == BroadphaseShapeType.Circle => 
                    CircleRectangleIntersects(ref second.Circle, ref first.Rectangle),
                BroadphaseShapeType.Circle when typeB == BroadphaseShapeType.Rectangle => 
                    CircleRectangleIntersects(ref first.Circle, ref second.Rectangle),
                BroadphaseShapeType.Circle when typeB == BroadphaseShapeType.Circle => 
                    CircleCircleIntersects(ref first.Circle, ref second.Circle),
                _ => throw new ArgumentOutOfRangeException()
            };

            return intersects;
        }
        
        private static bool RectangleRectangleIntersects(ref BroadphaseRectangle a, ref BroadphaseRectangle b)
        {
            if (a.Maximum.X < b.Minimum.X || a.Minimum.X > b.Maximum.X)
            {
                return false;
            }
            
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (a.Maximum.Y < b.Minimum.Y || a.Minimum.Y > b.Maximum.Y)
            {
                return false;
            }

            return true;
        }

        private static bool CircleRectangleIntersects(ref BroadphaseCircle a, ref BroadphaseRectangle b)
        {
            var squaredDistance = SquaredDistance(a.Center, ref b);
            return squaredDistance <= a.Radius * a.Radius;
        }

        private static bool CircleCircleIntersects(ref BroadphaseCircle a, ref BroadphaseCircle b)
        {
            var distanceVector = a.Center - b.Center;
            var distanceSquared = Vector2.Dot(distanceVector, distanceVector);
            var radiusSum = a.Radius + b.Radius;
            var radiusSquared = radiusSum * radiusSum;
            return distanceSquared <= radiusSquared;
        }

        public abstract uint FindCollidingShapes(Span<BroadphaseShape> shapes, Span<BroadphaseCollisionPair> pairs);


        private static float SquaredDistance(Vector2 point, ref BroadphaseRectangle rectangle)
        {
            var squaredDistance = 0.0f;
            var (x, y) = point;

            if (x < rectangle.Minimum.X)
            {
                squaredDistance += (rectangle.Minimum.X - x) * (rectangle.Minimum.X - x);
            }

            if (x > rectangle.Maximum.X)
            {
                squaredDistance += (x - rectangle.Maximum.X) * ((x - rectangle.Maximum.X));
            }
            
            if (y < rectangle.Minimum.Y)
            {
                squaredDistance += (rectangle.Minimum.Y - y) * (rectangle.Minimum.Y - y);
            }

            if (y > rectangle.Maximum.Y)
            {
                squaredDistance += (y - rectangle.Maximum.Y) * ((y - rectangle.Maximum.Y));
            }

            return squaredDistance;
        }
    }
}
