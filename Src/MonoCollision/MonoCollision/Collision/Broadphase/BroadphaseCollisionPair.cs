namespace MonoCollision
{
    public readonly struct BroadphaseCollisionPair
    {
        public readonly uint FirstIndex;
        public readonly uint SecondIndex;

        public BroadphaseCollisionPair(uint firstIndex, uint secondIndex)
        {
            FirstIndex = firstIndex;
            SecondIndex = secondIndex;
        }
    }
}
