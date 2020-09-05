// using System;
// using System.Collections.Generic;
// using MonoGame.Extended;
//
// namespace MonoCollision.Collections
// {
//     private class QuadTree
//         {
//             private readonly QuadTree[] _branches = new QuadTree[4];
//             private readonly List<QuadTreeData> _colliders = new List<QuadTreeData>();
//             private readonly CollisionResolver _collisionResolver;
//             public RectangleF Bounds { get; set; }
//             public bool IsLeaf { get; set; }
//
//             public QuadTree(CollisionResolver collisionResolver)
//             {
//                 _collisionResolver = collisionResolver;
//                 IsLeaf = true;
//             }
//
//             public void Insert(in QuadTreeData quadTreeData)
//             {
//                 if (quadTreeData.Collider.Intersects(Bounds) == false)
//                 {
//                     return;
//                 }
//
//                 if (IsLeaf && _colliders.Count >= _collisionResolver.MaxCollidersPerNode)
//                 {
//                     Split();
//                 }
//
//                 if (IsLeaf)
//                 {
//                     _colliders.Add(quadTreeData);
//                 }
//                 else
//                 {
//                     for (var index = 0; index < _branches.Length; index++)
//                     {
//                         QuadTree branch = _branches[index];
//                         branch.Insert(in quadTreeData);
//                     }
//                 }
//             }
//
//             public void Query(in Collider collider, List<QuadTreeData> collisions)
//             {
//                 // Collider is not in quad tree
//                 if (!collider.Intersects(Bounds))
//                 {
//                     return;
//                 }
//
//                 if (IsLeaf)
//                 {
//                     for (var index = 0; index < _colliders.Count; index++)
//                     {
//                         QuadTreeData childCollider = _colliders[index];
//                         if (collider.Intersects(childCollider.Collider))
//                         {
//                             collisions.Add(childCollider);
//                         }
//                     }
//                 }
//                 else
//                 {
//                     for (var i = 0; i < _branches.Length; i++)
//                     {
//                         _branches[i].Query(collider, collisions);
//                     }
//                 }
//             }
//
//             private void Split()
//             {
//                 Point2 min = Bounds.TopLeft;
//                 Point2 max = Bounds.BottomRight;
//                 Point2 center = Bounds.Center;
//
//                 Span<RectangleF> childAreas = stackalloc RectangleF[]
//                 {
//                     RectangleF.CreateFrom(min, center),
//                     RectangleF.CreateFrom(new Point2(center.X, min.Y), new Point2(max.X, center.Y)),
//                     RectangleF.CreateFrom(center, max),
//                     RectangleF.CreateFrom(new Point2(min.X, center.Y), new Point2(center.X, max.Y))
//                 };
//
//                 for (var i = 0; i < childAreas.Length; ++i)
//                 {
//                     QuadTree node = _collisionResolver.CreateQuadTree(childAreas[i]);
//                     _branches[i] = node;
//                 }
//
//                 for (var index = 0; index < _colliders.Count; index++)
//                 {
//                     QuadTreeData collider = _colliders[index];
//                     for (var i = 0; i < _branches.Length; i++)
//                     {
//                         QuadTree childQuadTree = _branches[i];
//                         childQuadTree.Insert(collider);
//                     }
//                 }
//
//                 IsLeaf = false;
//             }
//
//             public void Reset(RectangleF bounds)
//             {
//                 _colliders.Clear();
//                 Bounds = bounds;
//                 IsLeaf = true;
//             }
// }
