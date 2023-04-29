using App.Generation.Grid;
using UnityEngine;

namespace App.Levels
{
    public class LevelGrid : Grid<LevelGridTile>
    {
        public int Scale = 8;

        private void OnDrawGizmos()
        {
            if (Tiles == null)
            {
                return;
            }

            Gizmos.matrix = transform.localToWorldMatrix * Matrix4x4.Scale(Vector3.one * Scale);
            var flat = new Vector3(1.0f, 1.0f, 0);
            var half = new Vector3(0.5f, 0.5f, 0);

            var width = Bounds.width;
            var height = Bounds.height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var t = Get(x, y);
                    var color = Color.black;
                    if (t == null)
                    {
                        continue;
                    }

                    if (t.Type == LevelGridTileType.Pit)
                    {
                        color = Color.magenta;
                    }

                    if (t.Type == LevelGridTileType.Ground)
                    {
                        color = Color.white;
                    }

                    if (t.Type == LevelGridTileType.Dirt)
                    {
                        color = Color.cyan;
                    }

                    if (t.Type == LevelGridTileType.Grass)
                    {
                        color = Color.green;
                    }
                    
                    Gizmos.color = color;
                    Gizmos.DrawCube(new Vector3(x, y, 0) + half, flat);
                    Gizmos.DrawWireCube(new Vector3(x, y, 0) + half, flat);
                }
            }
        }
    }
    
}