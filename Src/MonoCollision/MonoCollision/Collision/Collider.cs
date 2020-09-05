namespace MonoCollision
{
    public struct Collider
    {
        private CollisionWorld _world;
        public readonly uint Handle;

        internal Collider(CollisionWorld world, uint handle)
        {
            _world = world;
            Handle = handle;
        }
    }
}
