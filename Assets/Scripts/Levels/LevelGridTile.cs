using App.Utils;

namespace App.Levels
{
    [System.Serializable]
    public class LevelGridTile : ICloneable<LevelGridTile>
    {
        public LevelGridTileType Type = LevelGridTileType.Pit;

        public LevelGridTile Clone()
        {
            return new LevelGridTile()
            {
                Type = this.Type,
            };
        }
    }
}