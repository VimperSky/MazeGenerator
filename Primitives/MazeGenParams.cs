using System;
using System.Text;
using Newtonsoft.Json;

namespace Core.GameLogic.MazeGen.Primitives
{

    public class MazeGenParams
    {
        public ushort XSize { get; set; }
        public ushort YSize { get; set; }
        public GeneratorType GeneratorType { get; set; }
        public ushort CavernRate { get; set; }
        public ushort? StartCellX { get; set; }
        public ushort? StartCellY { get; set; }
        
        [JsonIgnore]
        public byte MaxPlayers { get; } 

        /// <summary>
        /// Generates a maze and returns it as two-dimensional array
        /// </summary>
        /// <param name="xSize">Width = X-size * 2 + 1</param>
        /// <param name="ySize">Height = Y-size * 2 + 1</param>
        /// <param name="generatorType">Type of generator. Possible variants: Latest, Random, LatestAndRandom,
        /// LatestOverRandom, RandomOverLatest, Oldest, OldestAndNewest, OldestAndRandom</param>
        /// <param name="cavernRate">Cavern frequency. How much caverns will be on map. It's making maze non-perfect. </param>
        /// <param name="startCellX">CreateWorld cell for mazegen algorithm X. Null for Random.</param>
        /// <param name="startCellY">CreateWorld cell for mazegen algorithm Y. Null for Random.</param>
        /// <returns></returns>
        public MazeGenParams(ushort xSize = 5, ushort ySize = 5, GeneratorType generatorType = GeneratorType.LatestOverRandom, 
            ushort cavernRate = 30, ushort? startCellX = null, ushort? startCellY = null)
        {
            if (xSize > MazeConfig.MazeMaxSize)
                xSize = MazeConfig.MazeMaxSize;
            if (ySize > MazeConfig.MazeMaxSize)
                ySize = MazeConfig.MazeMaxSize;

            if (xSize < MazeConfig.MazeMinSize)
                xSize = MazeConfig.MazeMinSize;
            if (ySize < MazeConfig.MazeMinSize)
                ySize = MazeConfig.MazeMinSize;

            XSize = xSize;
            YSize = ySize;

            GeneratorType = generatorType;

            if (cavernRate > MazeConfig.MazeMaxCavernRate)
                cavernRate = MazeConfig.MazeMaxCavernRate;

            if (cavernRate < MazeConfig.MazeMinCavernRate)
                cavernRate = MazeConfig.MazeMinCavernRate;

            CavernRate = cavernRate;

            StartCellX = startCellX;
            StartCellY = startCellY;
            
            // formula: (x * y - 2) ^ 0.67
            var maxP = Convert.ToUInt16(Math.Pow(XSize * YSize - 1, 2 / 3f));
            if (maxP > byte.MaxValue)
                maxP = byte.MaxValue;
            MaxPlayers = (byte)maxP;
        }

        public static MazeGenParams UnpackFromJson(string json)
        {
            return JsonConvert.DeserializeObject<MazeGenParams>(json);
        }
        
        public string PackToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public override string ToString()
        {
            return $"XSize: {XSize}, YSize: {YSize}, MaxPlayers: {MaxPlayers}";
        }
    }
}
