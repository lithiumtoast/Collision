// using System;
// using System.Collections.Generic;
// using Microsoft.Xna.Framework;
// using MonoGame.Extended;
//
// namespace MonoCollision
// {
//     internal readonly struct QuadTreeData
//     {
//         public QuadTreeData(Collider collider, ICollisionActor collisionActor)
//         {
//             Collider = collider;
//             CollisionActor = collisionActor;
//         }
//
//         public bool Equals(QuadTreeData quadTree)
//         {
//             return CollisionActor == quadTree.CollisionActor;
//         }
//
//         public readonly Collider Collider;
//         public readonly ICollisionActor CollisionActor;
//     }
//
//     internal class CollisionResolver // Same as MGE CollisionComponent
//     {
//         private readonly List<QuadTree> _activeQuadTrees = new List<QuadTree>();
//         //Hashset to speedup removing and it avoids double insertion
//         private readonly HashSet<ICollisionActor> _collisionActors = new HashSet<ICollisionActor>();
//         private readonly Stack<WeakReference<QuadTree>> _inactiveQuadTrees = new Stack<WeakReference<QuadTree>>();
//         private readonly List<QuadTreeData> _quadTreeDataCollection = new List<QuadTreeData>();
//         public RectangleF Bounds { get; }
//         protected int MaxCollidersPerNode { get; set; } = 25;
//
//         public CollisionResolver(RectangleF bounds)
//         {
//             Bounds = bounds;
//         }
//
//         private QuadTree CreateQuadTree(RectangleF bounds)
//         {
//             while (_inactiveQuadTrees.TryPop(out WeakReference<QuadTree> weakReference))
//             {
//                 if (weakReference.TryGetTarget(out QuadTree target))
//                 {
//                     _activeQuadTrees.Add(target);
//                     target.Reset(bounds);
//                     return target;
//                 }
//             }
//
//             var quadTree = new QuadTree(this);
//             quadTree.Reset(bounds);
//             _activeQuadTrees.Add(quadTree);
//             return quadTree;
//         }
//
//         public bool Insert(ICollisionActor collisionActor)
//         {
//             return _collisionActors.Add(collisionActor);
//         }
//
//         public bool Remove(ICollisionActor collisionActor)
//         {
//             return _collisionActors.Remove(collisionActor);
//         }
//
//         public void Update()
//         {
//             QuadTree quadTree = CreateQuadTree(Bounds);
//
//             _quadTreeDataCollection.Clear();
//             foreach (ICollisionActor collisionActor in _collisionActors)
//             {
//                 var quadTreeData = new QuadTreeData(collisionActor.GetCollider(), collisionActor);
//                 _quadTreeDataCollection.Add(quadTreeData);
//                 quadTree.Insert(quadTreeData);
//             }
//
//             var queryResult = new List<QuadTreeData>();
//             for (var i = 0; i < _quadTreeDataCollection.Count; i++)
//             {
//                 QuadTreeData quadTreeData = _quadTreeDataCollection[i];
//                 quadTree.Query(quadTreeData.Collider, queryResult);
//                 for (var index = 0; index < queryResult.Count; index++)
//                 {
//                     QuadTreeData other = queryResult[index];
//                     if (other.Equals(quadTreeData))
//                     {
//                         continue;
//                     }
//
//                     Vector2 penetrationVector = quadTreeData.Collider.CalculatePenetrationVector(other.Collider);
//                     quadTreeData.CollisionActor.HandleCollision(new Collision
//                         {Penetration = penetrationVector, Other = other.CollisionActor});
//                 }
//
//                 queryResult.Clear();
//             }
//
//             for (var index = 0; index < _activeQuadTrees.Count; index++)
//             {
//                 QuadTree activeQuadTree = _activeQuadTrees[index];
//                 _inactiveQuadTrees.Push(new WeakReference<QuadTree>(activeQuadTree));
//             }
//
//             _activeQuadTrees.Clear();
//         }
//     }
//     }
// }