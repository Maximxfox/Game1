using KnightOfLight.model.AuxiliaryClasses;
using KnightOfLight.Contoller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightOfLight.model;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace KnightOfLight.View
{
    public class MainState : State
    {
        protected SpriteBatch SpriteBatch;
        protected List<Sprite> Sprites;
        protected Texture2D Background;
        protected Texture2D Buildings;
        protected Map Map;
        protected List<Texture2D> HeroAnimations;
        protected List<SoundEffect> Sound;
        protected List<Texture2D> EnemyAnimations;
        protected Texture2D Tileset;

        public MainState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
         : base(game, graphicsDevice, content)
        {
            LoadContent();
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Map = new Map( graphicsDevice, Tileset);
            Sprites = new List<Sprite>()
            {
                new Hero(HeroAnimations, Sound),
                new Enemy(EnemyAnimations, Sound)
            };
            Map.AddEntity(Sprites);
        }

        public void LoadContent()
        {
            Tileset = Content.Load<Texture2D>("Objects/block");
            Background = Content.Load<Texture2D>("Objects/0");
            Buildings = Content.Load<Texture2D>("Objects/1");
            HeroAnimations = new List<Texture2D>
            {
                Content.Load<Texture2D>("HeroAnimation/Idle"),
                Content.Load<Texture2D>("HeroAnimation/Hurt"),
                Content.Load<Texture2D>("HeroAnimation/Attack"),
                Content.Load<Texture2D>("HeroAnimation/Attack2"),
                Content.Load<Texture2D>("HeroAnimation/Attack3"),
                Content.Load<Texture2D>("HeroAnimation/Jump"),
                Content.Load<Texture2D>("HeroAnimation/Death"),
                Content.Load<Texture2D>("HeroAnimation/Walk"),
                Content.Load<Texture2D>("HeroAnimation/Run")
            };
            Sound = new List<SoundEffect>
            {
                Content.Load<SoundEffect>("Effects/HeroHit"),
                Content.Load<SoundEffect>("Effects/AttackHero"),
                Content.Load<SoundEffect>("Effects/HeroJump"),
                Content.Load<SoundEffect>("Effects/DeathHero"),
                Content.Load<SoundEffect>("Effects/HeroWalk"),
            };
            EnemyAnimations = new List<Texture2D>
            {
                Content.Load<Texture2D>("Enemy/Idle"),
                Content.Load<Texture2D>("Enemy/Hurt"),
                Content.Load<Texture2D>("Enemy/Attack"),
                Content.Load<Texture2D>("Enemy/Dead"),
                Content.Load<Texture2D>("Enemy/Walk"),
            };
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var sprite in Sprites)
                if (sprite != null)
                    sprite.Update(gameTime, Sprites);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin();
            SpriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            SpriteBatch.Draw(Buildings, new Vector2(0, 0), Color.White);
            Map.Draw(SpriteBatch);
            foreach (var sprite in Sprites)
                sprite.Draw(SpriteBatch);
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}