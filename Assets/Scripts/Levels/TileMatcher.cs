using System.Linq;
using App.Utils;
using UnityEngine;

namespace App.Levels.Steps
{
    public class TileMatcher : MonoBehaviour
    {
        public bool Enabled = true;
        public LevelGridTileType Type = LevelGridTileType.Ground;
        public bool RequireSurround = false;
        public LevelGridTileType[] SurroundTypes;

        [Range(0f, 1f)] public float Chance = 1.0f;

        public bool Match(LevelGrid grid, Vector2Int position, LevelGridTile tile)
        {
            if (!Enabled || tile.Type != Type)
            {
                return false;
            }

            if (!RequireSurround)
            {
                return true;
            }

            var canBeOutside = SurroundTypes.Contains(LevelGridTileType.Outside);

            foreach (var direction in Vector2IntUtils.Directions4())
            {
                var p = position + direction;
                var t = grid.Get(p);

                if (t == null && !canBeOutside)
                {
                    return false;
                }

                if (t == null && canBeOutside)
                {
                    continue;
                }

                if (!SurroundTypes.Contains(t.Type))
                {
                    return false;
                }
            }

            return true;
        }
    }
}