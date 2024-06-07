using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using KnightOfLight.model.AuxiliaryClasses;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;

namespace KnightOfLight.model
{
    public class Map
    {
        private static RenderTarget2D Target;
        private static readonly int TileSize = 32;
        public static readonly int[,] Tiles = new int[,]{
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,74,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 74,0,0,0,0,0,0,0,0,74,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 74,74,74,74,74,74,74,74,74,74,74,74,74,74,74,74,74,0,0,0,74,74,74,74,74},
        { 74,74,74,74,74,74,74,74,0,0,74,74,74,74,74,74,74,0,0,0,74,74,74,74,74 }
        };
        private static int Width = Tiles.GetLength(1);
        private static int Height = Tiles.GetLength(0);
        private static GraphicsDevice GraphicsDevice;
        private static Texture2D Texture;
        public static List<Sprite> Sprites;
        private static Rectangle[,] Colliders { get; } = new Rectangle[Height, Width];

        public Map( GraphicsDevice graphicsDevice, Texture2D texture)
        {
            Texture = texture;
            GraphicsDevice = graphicsDevice;
        }

        public static List<Rectangle> GetNearestColliders(Rectangle bounds)
        {
            var leftTile = (int)Math.Floor((float)bounds.Left / TileSize);
            var rightTile = (int)Math.Ceiling((float)bounds.Right / TileSize) - 1;
            var topTile = (int)Math.Floor((float)bounds.Top / TileSize);
            var bottomTile = (int)Math.Ceiling((float)bounds.Bottom / TileSize) - 1;
            leftTile = MathHelper.Clamp(leftTile, 0, Width);
            rightTile = MathHelper.Clamp(rightTile, 0, Width);
            topTile = MathHelper.Clamp(topTile, 0, Height);
            bottomTile = MathHelper.Clamp(bottomTile, 0, Height);
            var result = new List<Rectangle>();
            for (var x = topTile; x <= bottomTile; x++)
            {
                for (var y = leftTile; y <= rightTile; y++)
                {
                    if (x < Height)
                    {
                        if (Tiles[x, y] != 0)
                            result.Add(Colliders[x, y]);
                    }
                }
            }
            return result;
        }

        public void AddEntity(List<Sprite> sprites)
        {
            Sprites = sprites;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Target = new RenderTarget2D(GraphicsDevice, Width * TileSize, Height * TileSize);
            GraphicsDevice.SetRenderTarget(Target);
            GraphicsDevice.Clear(Color.Transparent);
            for (var x = 0; x < Height; x++)
            {
                for (var y = 0; y < Width; y++)
                {

                    if (Tiles[x, y] != 0)
                    {
                        var posX = y * TileSize;
                        var posY = x * TileSize;
                        var texture = Texture;
                        Colliders[x, y] = new Rectangle(posX, posY, TileSize, TileSize);
                        spriteBatch.Draw(texture, new Vector2(posX, posY), Color.White);
                    }
                }
            }
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Draw(Target, Vector2.Zero, Color.White);
        }
    }
}
