namespace MazeGenerator
{
    /// <summary>
    /// Represents a cell with x, y coords.
    /// </summary>
    public struct Cell
    {
        public int X { get; }
        public int Y { get; }

        /// <summary>
        /// Create a cell with x and y coords.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Neighbour
    {
        public int Rank { get; set; } = 0; 
        public Cell Cell { get; }
        public Neighbour(Cell cell)
        {
            Cell = cell;
        }
    }
}
