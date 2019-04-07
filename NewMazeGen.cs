using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
    public class MazeGen
    {
        private readonly int width;
        private readonly int height;

        private const int workerCount = 4;

        private MazeWorker[] workers = new MazeWorker[workerCount];

        private readonly Random random = new Random();

        private byte finishedCount = 0;

        public MazeGen(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public byte[,] GenerateMaze()
        {
            var startCells = new Cell[workerCount]
            {
                new Cell(random.Next(0, width / 4), random.Next(0, height / 4)),
                new Cell(random.Next(3 * width / 4 + 1, width), random.Next(0, height / 4)),
                new Cell(random.Next(3 * width / 4 + 1, width), random.Next(3 * height / 4 + 1, height)),
                new Cell(random.Next(0, width / 4), random.Next(3 * height / 4 + 1, height))
            };

            byte[,] maze = new byte[width, height];

            //Initialization
            for (byte i = 0; i < workers.Length; i++)
            {
                workers[i] = new MazeWorker();
                workers[i].ExpandableCells.Add(startCells[i]);
            }
                
            while (finishedCount < workerCount)
            {
                for (byte i = 0; i < workers.Length; i++)
                {
                    var worker = workers[i];

                    if (worker.IsFinished)
                        continue;

                    worker.CurrentCell = worker.ExpandableCells[worker.ExpandableCells.Count - 1];

                    if (worker.WorkersFound.Count >= 2)
                    {
                        worker.IsFinished = true;
                        ++finishedCount;
                        continue;
                    }

                    var unvisitedNeighbours = GetNeighbours(worker.CurrentCell, maze, worker, i);

                    if (unvisitedNeighbours.Count == 0)
                    {
                        worker.ExpandableCells.Remove(worker.CurrentCell);
                        if (worker.ExpandableCells.Count == 0)
                        {
                            worker.IsFinished = true;
                            ++finishedCount;
                        }
                        continue;
                    }

                    var choosenNeighbour = 0;

                    var commonWeight = 0;
                    foreach (var neighbour in unvisitedNeighbours)
                    {
                        var neighboursCount = GetNeighbours(neighbour.Cell, maze).Count;
                        neighbour.Rank = neighboursCount;
                        commonWeight += neighbour.Rank;
                    }

                    var targetRandom = random.Next(commonWeight);
                    var weight = 0;
                    for (int r = 0; r < unvisitedNeighbours.Count; r++)
                    {
                        weight += unvisitedNeighbours[r].Rank;
                        if (targetRandom < weight)
                        {
                            choosenNeighbour = r;
                            break;
                        }
                    }

                    var cell = new Cell(unvisitedNeighbours[choosenNeighbour].Cell.X, unvisitedNeighbours[choosenNeighbour].Cell.Y);
                    worker.ExpandableCells.Add(cell);

                    maze[cell.X, cell.Y] = (byte)(i + 1);
                }
            }
            return maze;
        }

        private List<Neighbour> GetNeighbours(Cell currentcell, byte[,] maze, MazeWorker? worker =  null, byte workerNumber = 0)
        {
            var foundNeighbours = new List<Neighbour>();

            //Right = 1. Bottom = 2. Left = 3. Top = 4.
            var checkCell = new Cell(currentcell.X + 1, currentcell.Y);
            if (checkCell.X < width)
            {
                if (maze[checkCell.X, checkCell.Y] == 0)
                {
                    foundNeighbours.Add(new Neighbour(checkCell));
                }
                else if ((worker != null) && (maze[checkCell.X, checkCell.Y] != (byte)(workerNumber + 1)))
                {
                    if (!worker.WorkersFound.Contains((byte)(maze[checkCell.X, checkCell.Y] - 1)))
                        worker.WorkersFound.Add((byte)(maze[checkCell.X, checkCell.Y] - 1));
                    if (!workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Contains(workerNumber))
                        workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Add(workerNumber);
                }
            }

            checkCell = new Cell(currentcell.X, currentcell.Y + 1);
            if (checkCell.Y < height)
            {
                if (maze[checkCell.X, checkCell.Y] == 0)
                {
                    foundNeighbours.Add(new Neighbour(checkCell));

                }
                else if ((worker != null) && (maze[checkCell.X, checkCell.Y] != (byte)(workerNumber + 1)))
                {
                    if (!worker.WorkersFound.Contains((byte)(maze[checkCell.X, checkCell.Y] - 1)))
                        worker.WorkersFound.Add((byte)(maze[checkCell.X, checkCell.Y] - 1));
                    if (!workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Contains(workerNumber))
                        workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Add(workerNumber);
                }
            }

            checkCell = new Cell(currentcell.X - 1, currentcell.Y);
            if (checkCell.X >= 0)
            {
                if (maze[checkCell.X, checkCell.Y] == 0)
                {
                    foundNeighbours.Add(new Neighbour(checkCell));
                }
                else if ((worker != null ) && (maze[checkCell.X, checkCell.Y] != (byte)(workerNumber + 1)))
                {
                    if (!worker.WorkersFound.Contains((byte)(maze[checkCell.X, checkCell.Y] - 1)))
                        worker.WorkersFound.Add((byte)(maze[checkCell.X, checkCell.Y] - 1));
                    if (!workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Contains(workerNumber))
                        workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Add(workerNumber);
                }
            }

            checkCell = new Cell(currentcell.X, currentcell.Y - 1);
            if (checkCell.Y >= 0)
            {
                if (maze[checkCell.X, checkCell.Y] == 0)
                {
                    foundNeighbours.Add(new Neighbour(checkCell));
                }
                else if ((worker != null) && (maze[checkCell.X, checkCell.Y] != (byte)(workerNumber + 1)))
                {
                    if (!worker.WorkersFound.Contains((byte)(maze[checkCell.X, checkCell.Y] - 1)))
                        worker.WorkersFound.Add((byte)(maze[checkCell.X, checkCell.Y] - 1));
                    if (!workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Contains(workerNumber))
                        workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Add(workerNumber);
                }
            }


            // If worker didn't pass - then we check diagonal neighbours to count rank(weight)
            if (worker == null)
            {
                checkCell = new Cell(currentcell.X + 1, currentcell.Y + 1);
                if (checkCell.X < width && checkCell.Y < height)
                {
                    if (maze[checkCell.X, checkCell.Y] == 0)
                    {
                        foundNeighbours.Add(new Neighbour(checkCell));
                    }
                }

                checkCell = new Cell(currentcell.X + 1, currentcell.Y - 1);
                if (checkCell.X < width && checkCell.Y >= 0)
                {
                    if (maze[checkCell.X, checkCell.Y] == 0)
                    {
                        foundNeighbours.Add(new Neighbour(checkCell));
                    }
                }


                checkCell = new Cell(currentcell.X - 1, currentcell.Y - 1);
                if (checkCell.X >= 0 && checkCell.Y >= 0)
                {
                    if (maze[checkCell.X, checkCell.Y] == 0)
                    {
                        foundNeighbours.Add(new Neighbour(checkCell));
                    }
                }

                checkCell = new Cell(currentcell.X - 1, currentcell.Y + 1);
                if (checkCell.X >= 0 && checkCell.Y < height)
                {
                    if (maze[checkCell.X, checkCell.Y] == 0)
                    {
                        foundNeighbours.Add(new Neighbour(checkCell));
                    }
                }
            }

            return foundNeighbours;
        }

    }
}