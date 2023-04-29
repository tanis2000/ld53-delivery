using App.Generation;
using UnityEngine;

namespace App.Levels.Steps
{
    public class CreateGround : GenerationStep
    {
        public int GroundHeight = 5;
        
        private void Set(LevelGrid grid, int x, int y, LevelGridTile t)
        {
            grid.Set(new Vector2Int(x, y), t.Clone());
        }
        
        public override void Generate(int seed)
        {
            var grid = GetComponent<LevelGrid>();

            for (var y = 0; y < GroundHeight; y++)
            {
                for (var x = 0; x < grid.Bounds.width; x++)
                {
                    if (y == GroundHeight - 1)
                    {
                        Set(grid, x, y, new LevelGridTile() { Type = LevelGridTileType.Ground});
                    }
                    else
                    {
                        Set(grid, x, y, new LevelGridTile() { Type = LevelGridTileType.Dirt});
                    }
                }
            }
        }

    }
}