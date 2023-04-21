using System.Collections.Generic;
using App.Utils;
using UnityEngine;

namespace App.Generation.Grid
{
    public class Grid<T> : MonoBehaviour where T : class, ICloneable<T>
    {
        public RectInt Bounds = new RectInt(0, 0, 3, 3);
        public T[] Tiles;

        public bool IsInitialized => Tiles != null && Tiles.Length == Bounds.width * Bounds.height;

        public bool InBounds(int x, int y)
        {
            return x >= Bounds.xMin && x < Bounds.xMax && y >= Bounds.yMin && y < Bounds.yMax;
        }

        public bool InBounds(Vector2Int p)
        {
            return InBounds(p.x, p.y);
        }

        public T Get(int x, int y)
        {
            if (!InBounds(x, y))
            {
                Debug.LogWarning($"Wrong position {x}, {y}");
                return null;
            }

            return Tiles[GetIndex(x, y)];
        }

        public T Get(Vector2Int p)
        {
            return Get(p.x, p.y);
        }

        public void Set(int x, int y, T t)
        {
            if (!InBounds(x, y))
            {
                Debug.LogError($"Wrong poisition {x}, {y}");
                return;
            }

            Tiles[GetIndex(x, y)] = t;
        }

        public void Set(Vector2Int p, T t)
        {
            Set(p.x, p.y, t);
        }

        public int GetIndex(int x, int y)
        {
            var xo = x - Bounds.xMin;
            var yo = y - Bounds.yMin;
            return xo + yo * Bounds.width;
        }

        public int GetIndex(Vector2Int p)
        {
            return GetIndex(p.x, p.y);
        }

        public void Clear()
        {
            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i] = null;
            }
        }

        public void Initialize(RectInt bounds)
        {
            Bounds = bounds;
            Tiles = new T[bounds.width * bounds.height];
        }

        public void Scale(int s)
        {
            var bw = Bounds.width;
            var bh = Bounds.height;
            var scaled = new T[bw * s * bh * s];
            for (int y = 0; y < bh * s; y++)
            {
                for (int x = 0; x < bw * s; x++)
                {
                    var bx = x / s;
                    var by = y / s;
                    var t = Tiles[bx + by * bw];
                    if (t != null)
                    {
                        scaled[x + y * bw * s] = t.Clone();
                    }
                }
            }

            Bounds = new RectInt(
                Bounds.x * s,
                Bounds.y * s,
                bw * s,
                bh * s);

            Tiles = scaled;
        }

        public void Mirror()
        {
            var bw = Bounds.width;
            var bh = Bounds.height;
            var mirrored = new T[bw * 2 * bh];

            for (int y = 0; y < bh; y++)
            {
                for (int x = 0; x < bw; x++)
                {
                    var t = Tiles[x + y * bw];
                    if (t != null)
                    {
                        var xMirrored = bw - x - 1;
                        var xNormal = x + bw;
                        mirrored[xMirrored + y * bw * 2] = t.Clone();
                        mirrored[xNormal + y * bw * 2] = t.Clone();
                    }
                }
            }

            Bounds = new RectInt(
                Bounds.x - bw,
                Bounds.y,
                Bounds.width * 2,
                Bounds.height);

            Tiles = mirrored;
        }

        public void Resize(RectInt newBounds, Vector2Int offset)
        {
            var resized = new T[newBounds.width * newBounds.height];

            for (int y = 0; y < newBounds.height; y++)
            {
                for (int x = 0; x < newBounds.width; x++)
                {
                    var p = new Vector2Int(x, y) + Bounds.min + offset;
                    var t = Get(p);
                    resized[x + y * newBounds.width] = t;
                }
            }

            Bounds = newBounds;
            Tiles = resized;
        }

        public List<Vector2Int> Flood(Vector2Int start, System.Func<Vector2Int, Vector2Int, Vector2Int, bool> canExpand)
        {
            var connected = new List<Vector2Int>();
            var open = new List<Vector2Int>() { start };

            while (open.Count > 0)
            {
                var current = open[0];
                open.RemoveAt(0);
                connected.Add(current);

                foreach (var direction in Vector2IntUtils.Directions4())
                {
                    var target = current + direction;
                    if (
                        !connected.Contains(target) &&
                        !open.Contains(target) &&
                        InBounds(target) &&
                        canExpand(current, target, direction))
                    {
                        open.Add(target);
                    }
                }
            }

            return connected;
        }
    }
}