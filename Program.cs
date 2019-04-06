using System;

namespace MazeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BufferHeight = 9999;

            var mazeGenerator = new MazeGen(10, 10);

            //Should get two-dimensional array or matrix with 1(not wall) and 0(wall) values
            //Spazzled maze generation algorithm
            var maze = mazeGenerator.GenerateMaze();

            for (byte i = 0; i < maze.GetLength(0); i++)
            {
                for (byte j = 0; j < maze.GetLength(1); j++)
                {
                    Console.Write(maze[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
