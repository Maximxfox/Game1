using KnightOfLight.Contoller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KnightOfLight.View
{
    public class Game1 : Game
    {
        protected GraphicsDeviceManager Graphics;
        protected State CurrentState;
        protected State NextState;
        protected Input Input = new Input();

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public void ChangeState(State state)
        {
            NextState = state;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            CurrentState = new GameMenu(this, GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Input.LeftGame))
                Exit();
            if (NextState != null)
            {
                CurrentState = NextState;
                NextState = null;
            }
            CurrentState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            CurrentState.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
