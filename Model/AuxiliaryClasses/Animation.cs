using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightOfLight.model.AuxiliaryClasses
{
    public class Animation
    {
        public int CurrentFrame;
        public int FrameCount;
        public int FrameHeight { get { return Texture.Height; } }
        public int FrameWidth { get { return Texture.Width / FrameCount; } }
        public float FrameSpeed;
        public Texture2D Texture;
        public SoundEffect SoundEffect;
        public Animation(Texture2D texture, int frameCount, SoundEffect soundEffect)
        {
            Texture = texture;
            FrameCount = frameCount;
            FrameSpeed = 0.2f;
            SoundEffect = soundEffect;
        }
    }
}
