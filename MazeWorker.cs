using System;
using System.Collections.Generic;
using System.Text;

namespace MazeGenerator
{
    class MazeWorker
    {
        public List<byte> WorkersFound = new List<byte>();
        public Cell CurrentCell { get; set; } = new Cell();

        public List<Cell> ExpandableCells { get; set; } = new List<Cell>();
        public bool IsFinished { get; set; } = false;
    }
}
