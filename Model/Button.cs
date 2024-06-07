using KnightOfLight.model.AuxiliaryClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightOfLight.model
{
    public class Button
    {
        public event EventHandler Click;
        private MouseState MousePosition;
        private SpriteFont Font;
        private bool IsHovering;
        private Texture2D Texture;
        private Color PenColor;
        private Vector2 Position;
        private string Text;
        private Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }

        public Button(Texture2D texture, SpriteFont font, string text, Vector2 position)
        {
            Texture = texture;
            Font = font;
            Text = text;
            Position = position;
            PenColor = Color.DarkBlue;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if (IsHovering)
                color = Color.Gray;
            spriteBatch.Draw(Texture, HitBox, color);
            if (!string.IsNullOrEmpty(Text))
            {
                var x = HitBox.X + HitBox.Width / 2 - Font.MeasureString(Text).X / 2;
                var y = HitBox.Y + HitBox.Height / 2 - Font.MeasureString(Text).Y / 2;
                spriteBatch.DrawString(Font, Text, new Vector2(x, y), PenColor);
            }
        }

        public void Update(GameTime gameTime)
        {
            MousePosition = Mouse.GetState();
            var mouseRectangle = new Rectangle(MousePosition.X, MousePosition.Y, 1, 1);
            IsHovering = false;
            if (mouseRectangle.Intersects(HitBox))
            {
                IsHovering = true;
                if (MousePosition.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
