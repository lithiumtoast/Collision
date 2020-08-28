using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace MonoCollision
{
    public class CollisionWorld
    {
        private readonly HashSet<IEntity> _entities = new HashSet<IEntity>();
        private CollisionResolver _resolver;

        public CollisionWorld(RectangleF bounds)
        {
            _resolver = new CollisionResolver(bounds);
        }

        public IReadOnlyCollection<IEntity> GetEntities()
        {
            return _entities;
        }
        
        public void Add(IEntity entity)
        {
            _entities.Add(entity);
            _resolver.Insert(entity);
        }

        public void Update(GameTime gameTime)
        {
            foreach (IEntity entity in _entities)
            {
                entity.Update(gameTime);
            }

            _resolver.Update();
        }
    }
}
