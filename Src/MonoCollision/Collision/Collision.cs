using Microsoft.Xna.Framework;

namespace MonoCollision
{
    public struct Collision
    {
        public Vector2 Penetration;
        public ICollisionActor Other;
    }
}