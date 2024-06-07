using KnightOfLight.model.AuxiliaryClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightOfLight.model
{
    public class AnimationManager
    {
        public Vector2 Position;
        public Animation Animation;
        private float Timer;
        private int TextureWidth = 64;

        public AnimationManager(Animation animation)
        {
            Animation = animation;
        }

        public void Start(Animation animation)
        {
            if (Animation == animation)
                return;
            if (Animation.SoundEffect != null)
            {
                var soundEffectInstance = Animation.SoundEffect.CreateInstance();
                soundEffectInstance.Play();
            }
            Animation = animation;
            Animation.CurrentFrame = 0;
            Timer = 0;
        }


        public virtual void Draw(SpriteBatch spriteBatch, bool IsChangeSide) 
        {
            if (IsChangeSide)
                spriteBatch.Draw(Animation.Texture, new Vector2(Position.X - TextureWidth, Position.Y), new Rectangle(Animation.CurrentFrame * Animation.FrameWidth,0,Animation.FrameWidth,Animation.FrameHeight), Color.White, 0f, new Vector2(1, 1), new Vector2(1,1),  SpriteEffects.FlipHorizontally, 0f);
            else
                spriteBatch.Draw(Animation.Texture, Position, new Rectangle(Animation.CurrentFrame * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight), Color.White, 0f, new Vector2(1, 1), new Vector2(1, 1), SpriteEffects.None, 0f);
        }

        public virtual void Update(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer > Animation.FrameSpeed)
            {
                Timer = 0;
                Animation.CurrentFrame++;
                if (Animation.CurrentFrame == Animation.FrameCount)
                    Animation.CurrentFrame = 0;
            }
        }
    }
}
