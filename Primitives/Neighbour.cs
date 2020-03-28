namespace Core.GameLogic.MazeGen.Primitives
{
    internal struct Neighbour
    {
        public IntVector TargetCell { get; }
        public IntVector WallCell { get; }
        public Neighbour(IntVector targetCell, IntVector wallCell)
        {
            TargetCell = targetCell;
            WallCell = wallCell;
        }
    }
}
