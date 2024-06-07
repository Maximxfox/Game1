using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightOfLight.View
{
    public class State
    {
        protected ContentManager Content;
        protected GraphicsDevice GraphicsDevice;
        protected Game1 Game;

        public virtual void Draw(GameTime gameTime)
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            Game = game;
            GraphicsDevice = graphicsDevice;
            Content = content;
        }
    }
}
