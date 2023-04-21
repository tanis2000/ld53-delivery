using App.Generation;
using UnityEngine;

namespace App.Levels.Steps
{
    public class InitializeLevelGrid : GenerationStep
    {
        public Vector2Int Size = new Vector2Int(10, 10);
        public override void Clear()
        {
            var grid = GetComponent<LevelGrid>();
            grid.Clear();
        }

        public override void Generate(int seed)
        {
            var grid = GetComponent<LevelGrid>();
            grid.Initialize(new RectInt(0, 0, Size.x, Size.y));
            foreach (var p in grid.Bounds.allPositionsWithin)
            {
                grid.Set(p, new LevelGridTile() { Type = LevelGridTileType.Pit});
            }
        }

    }
}