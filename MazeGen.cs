using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
    public class MazeGen
    {
        private readonly List<Cell> cells = new List<Cell>();

        private Cell currentCell;

        private readonly Random random = new Random();

        public byte[,] GenerateMaze(int mazeWidth, int mazeHeight, int cavernRate = 3)
        {
            var width = mazeWidth * 2 + 1;
            var height = mazeHeight * 2 + 1;

            // 1 means 100%, 10 means 1%
            if (cavernRate > 10)
                cavernRate = 10;
            else if (cavernRate < 1)
                cavernRate = 1;

            var maze = new byte[width, height];

            //Generate initial cell
            var randX = random.Next(mazeWidth);
            var randY = random.Next(mazeHeight);
            var cell = new Cell(randX * 2 + 1, randY * 2 + 1);
            cells.Add(cell);

            maze[cell.X, cell.Y] = 1;
             
            while (cells.Count > 0)
            {
                if (random.Next(2) == 0)
                    currentCell = cells[random.Next(cells.Count)];
                else
                    currentCell = cells[cells.Count - 1];

                var unvisitedNeighbours = new List<Neighbour>();

                //Right = 1. Bottom = 2. Left = 3. Top = 4.
                if ((currentCell.X + 2 < width) && (maze[currentCell.X + 2, currentCell.Y] == 0))
                {
                    unvisitedNeighbours.Add(new Neighbour(new Cell(currentCell.X + 2, currentCell.Y), new Cell(currentCell.X + 1, currentCell.Y)));
                }

                if ((currentCell.Y + 2 < height) && maze[currentCell.X, currentCell.Y + 2] == 0)
                {
                    unvisitedNeighbours.Add(new Neighbour(new Cell(currentCell.X, currentCell.Y + 2), new Cell(currentCell.X, currentCell.Y + 1)));
                }

                if ((currentCell.X - 2 >= 0 ) && maze[currentCell.X - 2, currentCell.Y] == 0)
                {
                    unvisitedNeighbours.Add(new Neighbour(new Cell(currentCell.X - 2, currentCell.Y), new Cell(currentCell.X - 1, currentCell.Y)));
                }

                if ((currentCell.Y - 2 >= 0) && maze[currentCell.X, currentCell.Y - 2] == 0)
                {
                    unvisitedNeighbours.Add(new Neighbour(new Cell(currentCell.X, currentCell.Y - 2), new Cell(currentCell.X, currentCell.Y - 1)));
                }

                if (unvisitedNeighbours.Count == 0)
                {
                    cells.Remove(currentCell);
                    continue;
                }

                var randDirection = random.Next(unvisitedNeighbours.Count);

                maze[unvisitedNeighbours[randDirection].TargetCell.X, unvisitedNeighbours[randDirection].TargetCell.Y] = 1;
                maze[unvisitedNeighbours[randDirection].WallCell.X, unvisitedNeighbours[randDirection].WallCell.Y] = 1;

                cells.Add(unvisitedNeighbours[randDirection].TargetCell);

                if (random.Next(cavernRate) == 0)
                {
                    unvisitedNeighbours.RemoveAt(randDirection);

                    var diagonalNeighbours = new List<Neighbour>();

                    if (currentCell.X + 2 < width && currentCell.Y + 2 < height && (maze[currentCell.X + 2, currentCell.Y + 2] == 0))
                    {
                        diagonalNeighbours.Add(new Neighbour(new Cell(currentCell.X + 2, currentCell.Y + 2), new Cell(currentCell.X + 1, currentCell.Y + 1)));
                    }

                    if (currentCell.X + 2 < width && currentCell.Y - 2 >= 0 && (maze[currentCell.X + 2, currentCell.Y - 2] == 0))
                    {
                        diagonalNeighbours.Add(new Neighbour(new Cell(currentCell.X + 2, currentCell.Y - 2), new Cell(currentCell.X + 1, currentCell.Y - 1)));
                    }

                    if (currentCell.X - 2 >= 0 && currentCell.Y - 2 >= 0 && (maze[currentCell.X - 2, currentCell.Y - 2] == 0))
                    {
                        diagonalNeighbours.Add(new Neighbour(new Cell(currentCell.X - 2, currentCell.Y - 2), new Cell(currentCell.X - 1, currentCell.Y - 1)));
                    }

                    if (currentCell.X - 2 >= 0 && currentCell.Y + 2 < height && (maze[currentCell.X - 2, currentCell.Y + 2] == 0))
                    {
                        diagonalNeighbours.Add(new Neighbour(new Cell(currentCell.X - 2, currentCell.Y + 2), new Cell(currentCell.X - 1, currentCell.Y + 1)));
                    }

                    foreach(var neighbour in unvisitedNeighbours)
                    {
                        maze[neighbour.WallCell.X, neighbour.WallCell.Y] = 1;
                    }

                    foreach (var diaNeighbour in diagonalNeighbours)
                    {
                        maze[diaNeighbour.WallCell.X, diaNeighbour.WallCell.Y] = 1;
                    }
                }
            }

            return maze;
        }
    }
}