namespace MazeGenerator
{
    /// <summary>
    /// Represents a cell with x, y coords.
    /// </summary>
    public struct Cell
    {
        public int X { get; }
        public int Y { get; }
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// Represents a Neighbour with TargetCell and WallCell
    /// </summary>
    public struct Neighbour
    {
        public Cell TargetCell { get; set; }

        public Cell WallCell { get; set; }

        public Neighbour(Cell targetCell, Cell wallCell)
        {
            TargetCell = targetCell;
            WallCell = wallCell;
        }
    }
}
