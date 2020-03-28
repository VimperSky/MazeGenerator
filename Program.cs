using System;
using System.IO;
using Core.GameLogic.MazeGen.Primitives;

namespace Core.GameLogic.MazeGen
{
    public static class Program
    {
        private static void Main()
        {
            var mazeGenParams = new MazeGenParams(20, 20, GeneratorType.Random, 30);
            var mazeGen = new MazeGenerator(mazeGenParams);
            var maze = mazeGen.GenerateMaze();
        }
    }
}