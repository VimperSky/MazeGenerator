using System;

namespace MazeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BufferHeight = 9999;

            var mazeGenerator = new MazeGen(25, 25);

            var maze = mazeGenerator.GenerateMaze();

            for (byte i = 0; i < maze.GetLength(1); i++)
            {
                for (byte j = 0; j < maze.GetLength(0); j++)
                {
                    Console.Write(maze[j, i] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
