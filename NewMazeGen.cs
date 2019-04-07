using System;
using System.Collections.Generic;

namespace MazeGenerator
{
    public class MazeGen
    {
        private readonly int width;
        private readonly int height;

        private MazeWorker[] workers = new MazeWorker[4];

        private readonly Random random = new Random();

        private byte finishedCount = 0;

        public MazeGen(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public byte[,] GenerateMaze()
        {

            workers[0] = new MazeWorker { CurrentCell = new Cell(0, 0) };
            workers[1] = new MazeWorker { CurrentCell = new Cell(width - 1, 0) };

            workers[2] = new MazeWorker { CurrentCell = new Cell(width - 1, height - 1) };
            workers[3] = new MazeWorker { CurrentCell = new Cell(0, height - 1) };



            byte[,] maze = new byte[width, height];

            //Initialization
            for (int i = 0; i < workers.Length; i++)
            {
                maze[workers[i].CurrentCell.X, workers[i].CurrentCell.Y] = (byte)(i + 1);
                workers[i].ExpandableCells.Add(workers[i].CurrentCell);
            }

            while (finishedCount < 4)
            {
                for (byte i = 0; i < workers.Length; i++)
                {
                    var worker = workers[i];
                    if (worker.ExpandableCells.Count == 0)
                    {
                        worker.IsFinished = true;
                        ++finishedCount;
                    }

                    if (worker.IsFinished)
                        continue;

                    worker.CurrentCell = worker.ExpandableCells[worker.ExpandableCells.Count - 1];

                    var unvisitedNeighbours = new List<Cell>();

                    //Right = 1. Bottom = 2. Left = 3. Top = 4.
                    var checkCell = new Cell(worker.CurrentCell.X + 1, worker.CurrentCell.Y);
                    if (checkCell.X < width)
                    {
                        if (maze[checkCell.X, checkCell.Y] == 0)
                        {
                            unvisitedNeighbours.Add(checkCell);
                        }
                        else if ((maze[checkCell.X, checkCell.Y] != (byte)(i + 1)))
                        {
                            if (!worker.WorkersFound.Contains((byte)(maze[checkCell.X, checkCell.Y] - 1)))
                                worker.WorkersFound.Add((byte)(maze[checkCell.X, checkCell.Y] - 1));
                            if (!workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Contains(i))
                                workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Add(i);
                        }
                    }

                    checkCell = new Cell(worker.CurrentCell.X, worker.CurrentCell.Y + 1);
                    if (checkCell.Y < height)
                    {
                        if (maze[checkCell.X, checkCell.Y] == 0)
                        {
                            unvisitedNeighbours.Add(checkCell);
                        }
                        else if ((maze[checkCell.X, checkCell.Y] != (byte)(i + 1)))
                        {
                            if (!worker.WorkersFound.Contains((byte)(maze[checkCell.X, checkCell.Y] - 1)))
                                worker.WorkersFound.Add((byte)(maze[checkCell.X, checkCell.Y] - 1));
                            if (!workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Contains(i))
                                workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Add(i);
                        }
                    }

                    checkCell = new Cell(worker.CurrentCell.X - 1, worker.CurrentCell.Y);
                    if (checkCell.X >= 0)
                    {
                        if (maze[checkCell.X, checkCell.Y] == 0)
                        {
                            unvisitedNeighbours.Add(checkCell);
                        }
                        else if ((maze[checkCell.X, checkCell.Y] != (byte)(i + 1)))
                        {
                            if (!worker.WorkersFound.Contains((byte)(maze[checkCell.X, checkCell.Y] - 1)))
                                worker.WorkersFound.Add((byte)(maze[checkCell.X, checkCell.Y] - 1));
                            if (!workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Contains(i))
                                workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Add(i);
                        }
                    }

                    checkCell = new Cell(worker.CurrentCell.X, worker.CurrentCell.Y - 1);
                    if (checkCell.Y >= 0)
                    {
                        if (maze[checkCell.X, checkCell.Y] == 0)
                        {
                            unvisitedNeighbours.Add(checkCell);
                        }
                        else if ((maze[checkCell.X, checkCell.Y] != (byte)(i + 1)))
                        {
                            if (!worker.WorkersFound.Contains((byte)(maze[checkCell.X, checkCell.Y] - 1)))
                                worker.WorkersFound.Add((byte)(maze[checkCell.X, checkCell.Y] - 1));
                            if (!workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Contains(i))
                                workers[maze[checkCell.X, checkCell.Y] - 1].WorkersFound.Add(i);
                        }
                    }

                    if (worker.WorkersFound.Count >= 2)
                    {
                        worker.IsFinished = true;
                        ++finishedCount;
                        continue;
                    }

                    if (unvisitedNeighbours.Count == 0)
                    {
                        worker.ExpandableCells.Remove(worker.CurrentCell);
                        continue;
                    }

                    var randDirection = random.Next(unvisitedNeighbours.Count);

                    var cell = new Cell(unvisitedNeighbours[randDirection].X, unvisitedNeighbours[randDirection].Y);
                    worker.ExpandableCells.Add(cell);

                    maze[cell.X, cell.Y] = (byte)(i + 1);
                }
            }
            return maze;
        }
    }
}
