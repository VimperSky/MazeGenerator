using System;
using System.Collections.Generic;
using System.Linq;
using Core.GameLogic.MazeGen.Primitives;

namespace Core.GameLogic.MazeGen
{
    // Growing Tree Alghorithm
    public class MazeGenerator
    {
        private const byte Wall = 0;
        private const byte Passable = 1;
        private const byte UnbreakableWall = 2;
        
        private readonly Random _random = new Random();
        private readonly MazeGenParams _mazeGenParams;
        private readonly List<IntVector> _openedCells;
        private readonly byte[,] _maze;

        private int MazeWidth => _maze.GetLength(0);
        private int MazeHeight => _maze.GetLength(1);

        private readonly int _rootedMazeSize;

        private bool _isUsed;
        
        private bool IsEven(IntVector intVector)
        {
            return intVector.X % 2 == 0 && intVector.Y % 2 == 0;
        }
        
        public MazeGenerator(MazeGenParams mazeGenParams)
        {
            _mazeGenParams = mazeGenParams;
            _openedCells = new List<IntVector>();
            _maze = new byte[mazeGenParams.XSize * 2 + 1, mazeGenParams.YSize * 2 + 1];
            
            // Make unbreakable walls
            for (var i = 0; i < MazeHeight; i++)
            {
                for (var j = 0; j < MazeWidth; j++)
                {
                    if (j == 0 || i == 0 || j == MazeWidth - 1 || i == MazeHeight - 1)
                        _maze[j, i] = UnbreakableWall;
                }
            }

            _rootedMazeSize = (int)Math.Sqrt(mazeGenParams.XSize * mazeGenParams.YSize);
        }
        
                
        public byte[,] GenerateMaze()
        {
            if (_isUsed)
                throw new Exception("You cannot use this class again!");

            _isUsed = true;
            
            // Generate initial cell
            var randX = _mazeGenParams.StartCellX ?? _random.Next(_mazeGenParams.XSize);
            var randY = _mazeGenParams.StartCellX ?? _random.Next(_mazeGenParams.YSize);
            _openedCells.Add(new IntVector(randX * 2 + 1, randY * 2 + 1));
            _maze[_openedCells[0].X, _openedCells[0].Y] = 1;


            while (_openedCells.Count > 0)
            {
                var cell = GetNextCell();
                var neighbours = GetNeighbours(cell);
                if (neighbours.Count == 0)
                {
                    _openedCells.Remove(cell);
                    continue;
                }
                BreakNeighbour(neighbours, _random.Next(neighbours.Count));
            } 
            
            var noWallsCount = 0;
            for (var i = 0; i < _maze.GetLength(1); i++)
            {
                for (var j = 0; j < _maze.GetLength(0); j++)
                {
                    if (_maze[j, i] == 1)
                        noWallsCount++;
                }
            }
            
            Console.WriteLine("Count is: " + noWallsCount * _mazeGenParams.CavernRate / MazeConfig.MazeMaxCavernRate);
            Console.WriteLine("Max level is: " + (_rootedMazeSize / 5 * 2 + 1));
            for (var c = 0; c < 1f * noWallsCount * _mazeGenParams.CavernRate / MazeConfig.MazeMaxCavernRate; c++)
            {
                var initialCell = new IntVector(_random.Next(_mazeGenParams.XSize) * 2 + 1,  _random.Next(_mazeGenParams.YSize) * 2 + 1);

                var levels = _random.Next(1 + _rootedMazeSize / 5) * 2 + 1;
                var breakCells = new List<IntVector> {initialCell};
                for (var l = 0; l < levels; l++)
                {
                    var nextBreakCells = new List<IntVector>();
                    foreach (var breakCell in breakCells)
                    {
                        foreach (var shift in IntVector.OrtNormals)
                        {
                            var nei = breakCell + shift;
                            if (GetCell(nei) == 0)
                            {
                                _maze[nei.X, nei.Y] = Passable;
                                nextBreakCells.Add(nei);
                            }
                        }
                    }

                    breakCells = nextBreakCells;
                }
            }
            
            return _maze;
        }
        
        private void BreakNeighbour(List<Neighbour> neighbours, int direction)
        {
            _maze[neighbours[direction].TargetCell.X, neighbours[direction].TargetCell.Y] = 1;
            _maze[neighbours[direction].WallCell.X, neighbours[direction].WallCell.Y] = 1;
            _openedCells.Add(neighbours[direction].TargetCell);
            neighbours.RemoveAt(direction);
        }
        
        private byte GetCell(IntVector cell)
        {
            if (cell.X < MazeWidth && cell.Y < MazeHeight && cell.X >= 0 && cell.Y >= 0)
            {
                return _maze[cell.X, cell.Y];
            }

            return 10;
        }
        
        private List<Neighbour> GetNeighbours(IntVector currentCell)
        {
            var neighbours = new List<Neighbour>();
            
            var cell = new IntVector(currentCell.X + 2, currentCell.Y);
            if (GetCell(cell) == Wall) neighbours.Add(new Neighbour(cell, (cell + currentCell) / 2));
            
            cell = new IntVector(currentCell.X, currentCell.Y + 2);
            if (GetCell(cell) == Wall) neighbours.Add(new Neighbour(cell, (cell + currentCell) / 2));

            cell = new IntVector(currentCell.X - 2, currentCell.Y);
            if (GetCell(cell) == Wall) neighbours.Add(new Neighbour(cell, (cell + currentCell) / 2));

            cell = new IntVector(currentCell.X, currentCell.Y - 2);
            if (GetCell(cell) == Wall) neighbours.Add(new Neighbour(cell, (cell + currentCell) / 2));

            return neighbours;
        }
        
        private IntVector GetNextCell()
        {
            IntVector currentCell;
            switch (_mazeGenParams.GeneratorType)
            {
                case GeneratorType.Latest:
                    currentCell = _openedCells[_openedCells.Count - 1];
                    break;
                case GeneratorType.Random:
                    currentCell = _openedCells[_random.Next(_openedCells.Count)];
                    break;
                case GeneratorType.LatestAndRandom:
                    currentCell = (_random.Next(2) == 0 ? _openedCells[_random.Next(_openedCells.Count)] : _openedCells[_openedCells.Count - 1]);
                    break;
                case GeneratorType.LatestOverRandom:
                    currentCell = (_random.Next(4) == 0 ? _openedCells[_random.Next(_openedCells.Count)] : _openedCells[_openedCells.Count - 1]);
                    break;
                case GeneratorType.RandomOverLatest:
                    currentCell = (_random.Next(4) != 0 ? _openedCells[_random.Next(_openedCells.Count)] : _openedCells[_openedCells.Count - 1]);
                    break;
                case GeneratorType.Oldest:
                    currentCell = _openedCells[0];
                    break;
                case GeneratorType.OldestAndLatest:
                    currentCell = (_random.Next(2) == 0 ? _openedCells[0] : _openedCells[_openedCells.Count - 1]);
                    break;
                case GeneratorType.OldestAndRandom:
                    currentCell = (_random.Next(2) == 0 ? _openedCells[0] : _openedCells[_random.Next(_openedCells.Count)]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return currentCell;
        }
        
    }
}
