using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightOfLight.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;

namespace KnightOfLight.Model
{
    public class BFS
    {
        private int[,] Tiles;
        private Vector2 Start;
        private Vector2 End;
        private int Width;
        private int Height;
        private Vector2[] Directions = new Vector2[] { new Vector2(1, 0),new Vector2(-1, 0), new Vector2(0, 1), -new Vector2(0, 1) };

        public List<Vector2> FindPath(int[,] tiles, Vector2 start, Vector2 end)
        {
            Tiles =  tiles;
            Start = new Vector2((int)start.X, (int)start.Y + 1);
            End = new Vector2((int)end.X, (int)end.Y + 1);
            Width = Tiles.GetLength(1);
            Height = Tiles.GetLength(0);
            var queue = new Queue<Vector2>();
            var cameFrom = new Dictionary<Vector2, Vector2>();
            var visited = new HashSet<Vector2>();
            queue.Enqueue(Start);
            visited.Add(Start);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var neighbor in GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                        cameFrom[neighbor] = current;
                    }
                }
            }
            var path = new List<Vector2>();
            var currentPos = End;
            while (currentPos != Start)
            {
                path.Add(currentPos);
                if (!cameFrom.ContainsKey(currentPos))
                    return null;
                currentPos = cameFrom[currentPos];
            }
            path.Reverse();
            return path;
        }

        private List<Vector2> GetNeighbors(Vector2 position)
        {
            var neighbors = new List<Vector2>();
            foreach (var direction in Directions)
            {
                var neighbor = position + direction;
                if (IsValidGridPosition(neighbor, direction))
                    neighbors.Add(neighbor);
            }

            return neighbors;
        }

        private bool IsValidGridPosition(Vector2 neighbor, Vector2 direction)
        {
            if (neighbor.X >= 0 && neighbor.X < Width && neighbor.Y >= 0 && neighbor.Y < Height)
            {
                if (Tiles[(int)neighbor.X / 32, (int)neighbor.Y / 32] == 0)
                {
                    if (direction.Y == 0)
                        return Tiles[(int)neighbor.X, (int)neighbor.Y - 1 ] == 0;
                    if (direction.X == 0 && neighbor.Y >= 3)
                        return Tiles[(int)neighbor.X, (int)neighbor.Y - 2] == 0 && Tiles[(int)neighbor.X, (int)neighbor.Y - 3] == 0 && Tiles[(int)neighbor.X, (int)neighbor.Y - 1] != 0 ;
                }
            }
            return false;
        }
    }
}
