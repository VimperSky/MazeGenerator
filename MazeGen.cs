using System;
using System.Collections.Generic;

namespace MazeGenerator
{
    public class MazeGen
    {
        private readonly int width;
        private readonly int height;

        private List<Cell> cells = new List<Cell>();

        private Cell currentCell;

        private readonly Random random = new Random();


        public MazeGen(int width, int height)
        {
            this.height = height;
            this.width = width;
        }

        public byte[,] GenerateMaze()
        {
            var maze = new byte[width, height];

            //Generate initial cell
            var randWidth = random.Next(maze.GetLength(0));
            var randHeight = random.Next(maze.GetLength(1));
            var cell = new Cell(randWidth, randHeight);
            cells.Add(cell);

            maze[cell.X, cell.Y] = 1;

            while (cells.Count > 0)
            {
                currentCell = cells[random.Next(cells.Count)];

                var unvisitedNeighbours = new List<Cell>();

                //Right = 1. Bottom = 2. Left = 3. Top = 4.
                if ((currentCell.X + 1 < width) && (maze[currentCell.X + 1, currentCell.Y] == 0))
                    unvisitedNeighbours.Add(new Cell(currentCell.X + 1, currentCell.Y));

                if ((currentCell.Y + 1 < height) && maze[currentCell.X, currentCell.Y + 1] == 0)
                    unvisitedNeighbours.Add(new Cell(currentCell.X, currentCell.Y + 1));

                if ((currentCell.X - 1 >= 0 ) && maze[currentCell.X - 1, currentCell.Y] == 0)
                    unvisitedNeighbours.Add(new Cell(currentCell.X - 1, currentCell.Y));

                if ((currentCell.Y - 1 >= 0) && maze[currentCell.X, currentCell.Y - 1] == 0)
                    unvisitedNeighbours.Add(new Cell(currentCell.X, currentCell.Y - 1));

                if (unvisitedNeighbours.Count == 0)
                {
                    cells.Remove(currentCell);
                    continue;
                }

                var randDirection = random.Next(unvisitedNeighbours.Count);

                maze[unvisitedNeighbours[randDirection].X, unvisitedNeighbours[randDirection].Y] = 1;

                cells.Add(unvisitedNeighbours[randDirection]);
            }

            return maze;
        }
    }
}
