namespace Core.GameLogic.MazeGen
{
    internal static class MazeExtensions
    {
        public static byte[] ToByteArray(this byte[,] maze)
        {
            var byteArr = new byte[maze.GetLength(0) * maze.GetLength(1)];
            int arrIter = 0;
            for (byte i = 0; i < maze.GetLength(1); i++)
            {
                for (byte j = 0; j < maze.GetLength(0); j++)
                {
                    byteArr[arrIter++] = maze[j, i];
                }
            }
            return byteArr;
        }

        public static byte[,] ToMaze(this byte[] byteArr, int width, int height)
        {
            byte[,] maze = new byte[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    maze[j, i] = byteArr[i * width + j];
                }
            }
            return maze;
        }
    }
}
