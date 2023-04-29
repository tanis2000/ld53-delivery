using App.Generation;
using UnityEngine;

namespace App.Levels.Steps
{
    public class CreateFoliage : GenerationStep
    {
        public int FoliageHeight = 5;

        private void Set(LevelGrid grid, int x, int y, LevelGridTile t)
        {
            grid.Set(new Vector2Int(x, y), t.Clone());
        }

        public override void Generate(int seed)
        {
            var grid = GetComponent<LevelGrid>();
            var y = FoliageHeight;

            for (var x = 0; x < grid.Bounds.width; x++)
            {
                Set(grid, x, y, new LevelGridTile() { Type = LevelGridTileType.Grass });
            }
        }
    }
}