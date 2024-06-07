using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using KnightOfLight.model;
using KnightOfLight.model.AuxiliaryClasses;

namespace KnightOfLight.View
{
    public class GameMenu: State
    {
        protected List<Button> Components;
        protected SpriteBatch SpriteBatch;
        protected Texture2D Background;
        protected Song Song;
        protected Song Song2;
        protected Texture2D ButtonTexture;
        protected SpriteFont ButtonFont;

        public GameMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            LoadContent();
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            MediaPlayer.Play(Song);
            MediaPlayer.Volume = 0.1f;
            var newGame = new Button(ButtonTexture, ButtonFont, "Начать игру", new Vector2(300, 100));
            var settings = new Button(ButtonTexture, ButtonFont, "Настройки", new Vector2(300, 200));
            var quitGame = new Button(ButtonTexture, ButtonFont, "Выйти", new Vector2(300, 300));
            newGame.Click += NewGameButton;
            quitGame.Click += QuitGameButton;
            Components = new List<Button>()
            {
                newGame,
                settings,
                quitGame,
            };
        }

        private void NewGameButton(object sender, EventArgs e)
        {
            Game.ChangeState(new MainState(Game, GraphicsDevice, Content));
            MediaPlayer.Play(Song2);
            MediaPlayer.Volume = 0.1f;
        }

        private void QuitGameButton(object sender, EventArgs e)
        {
            Game.Exit();
        }

        public void LoadContent()
        {
            Background = Content.Load<Texture2D>("Objects/castle");
            Song = Content.Load<Song>("Music/1");
            Song2 = Content.Load<Song>("Music/2");
            ButtonTexture = Content.Load<Texture2D>("Objects/button");
            ButtonFont = Content.Load<SpriteFont>("Fonts/font");
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            foreach (var component in Components)
                component.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in Components)
                component.Update(gameTime);
        }
    }
}
