using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightOfLight.Contoller;
using KnightOfLight.model;
using SharpDX.Direct3D9;

namespace KnightOfLight.model.AuxiliaryClasses
{
    public class Sprite
    {
        public enum Side
        {
            Right,
            Left
        };

        public bool IsDead;
        public bool IsDying;
        public bool OnGround;
        public bool IsChangeSide;
        public bool IsAnimation;
        public bool IsHit;
        public int CurrentHealth;
        public int Health;
        public int TextureHeight;
        public int TextureWidth;
        public int offset = 10;
        public int Damage = 20;
        public float Speed;
        public float Gravity = 0.15f;
        public Texture2D Texture;
        public Vector2 Pos;
        public Vector2 Velocity;
        public AnimationManager AnimationManager;
        public Dictionary<string, Animation> Animations;
        public Input Input = new Input();
        public Side CurrentSide = Side.Right;

        public Vector2 PositionInTiles
        {
            get { return Pos / 32; }
        }
        public Vector2 Position
        {
            get { return Pos; }
            set
            {
                Pos = value;
                if (AnimationManager != null)
                    AnimationManager.Position = Pos;
            }
        }

        public Rectangle HitBox(Vector2 pos) =>  new Rectangle((int) pos.X, (int) pos.Y, TextureWidth, TextureHeight);

        public void UpdatePosition()
        {
            var newPos = Position + Velocity;
            var newRect = HitBox(newPos);
            if (newPos != Position)
                OnGround = false;
            foreach (var collider in Map.GetNearestColliders(newRect))
            {
                if (newPos != Position)
                {
                    newRect = HitBox(new Vector2(newPos.X, Position.Y));
                    if (newRect.Intersects(collider))
                    {
                        if (newPos.X > Position.X)
                            newPos.X = collider.Left - TextureWidth;
                        else
                            newPos.X = collider.Right;
                        Velocity.X = 0;
                    }
                }
                newRect = HitBox(new Vector2(Position.X, newPos.Y));
                if (newRect.Intersects(collider))
                {
                    if (Velocity.Y > 0)
                    {
                        newPos.Y = collider.Top - TextureHeight;
                        OnGround = true;
                    }
                    else
                    {
                        if (Position.Y != collider.Bottom)
                            newPos.Y = collider.Bottom;
                    }
                    Velocity.Y = 0;
                }
            }
            Position = newPos;
        }

        public void Hit()
        {
            if (IsHit)
                Health = CurrentHealth;
            if (CurrentHealth != Health)
            {
                CurrentHealth = Health;
                IsHit = true;
            }
            else if (AnimationManager.Animation.FrameCount - 1 == AnimationManager.Animation.CurrentFrame)
                IsHit = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(Texture, Position, Color.White);
            else if (AnimationManager != null)
                AnimationManager.Draw(spriteBatch, IsChangeSide);
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {

        }
    }
}
