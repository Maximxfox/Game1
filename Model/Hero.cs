using KnightOfLight.model.AuxiliaryClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KnightOfLight.Contoller;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using SharpDX.MediaFoundation;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel;

namespace KnightOfLight.model
{
    public class Hero : Sprite
    {
        private int AnimationAttack;
        private float JumpSpeed = -4f;

        public Hero(List<Texture2D> animations, List<SoundEffect> sound)
        {
            Position = new Vector2(100, 410);
            Speed = 1.5f;
            Animations = new Dictionary<string, Animation>()
            {
                {"stay", new Animation(animations[0],4, null)},
                {"hit", new Animation(animations[1],2, sound[0]) },
                {"attack", new Animation(animations[2],5, sound[1]) },
                {"attack2", new Animation(animations[3],4, sound[1]) },
                {"attack3", new Animation(animations[4],4, sound[1]) },
                {"jump", new Animation(animations[5],6,sound[2]) },
                {"death", new Animation(animations[6],6,sound[3]) },
                {"walk", new Animation(animations[7],8,sound[4]) },
                {"run", new Animation(animations[8],7,sound[4]) }
            };
            Health = CurrentHealth = 100;
            TextureHeight = 64;
            TextureWidth = 52;
            AnimationManager = new AnimationManager(Animations.First().Value);
        }

        private void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X += Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X -= Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Run) && !IsAnimation)            
                Velocity.X *= Speed;
        }

        private void SetAnimations()
        {
            if (Health == 0)
            {
                IsDying = true;
                AnimationManager.Start(Animations["death"]);
                if (AnimationManager.Animation.FrameCount - 1 == AnimationManager.Animation.CurrentFrame)
                {
                    IsDead = true;
                    IsDying = false;
                }
                Velocity.X = 0;
            }  
            else if (IsHit)
            {
                CurrentHealth = Health;
                AnimationManager.Start(Animations["hit"]);
            }
            else if (IsAnimation)
            {
                if (AnimationAttack == 1)
                    AnimationManager.Start(Animations["attack"]);
                else if (AnimationAttack == 2)
                    AnimationManager.Start(Animations["attack2"]);
                else
                    AnimationManager.Start(Animations["attack3"]);
            }
            else if (Math.Abs(Velocity.Y) > Gravity * offset)
                AnimationManager.Start(Animations["jump"]);
            else if (Velocity.X > 0)
            {
                if (Velocity.X != Speed)
                    AnimationManager.Start(Animations["run"]);
                else 
                    AnimationManager.Start(Animations["walk"]);
                if (CurrentSide == Side.Left)
                    IsChangeSide = false;
                CurrentSide = Side.Right;
            }
            else if (Velocity.X < 0)
            {
                if (Velocity.X != -Speed)
                    AnimationManager.Start(Animations["run"]);
                else
                    AnimationManager.Start(Animations["walk"]);
                if (CurrentSide == Side.Right)
                    IsChangeSide = true;
                CurrentSide = Side.Left;
            }
            else
                AnimationManager.Start(Animations["stay"]);
        }

        private void Jump()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Jump) && OnGround)
            {
                Velocity.Y = JumpSpeed * Speed;
                Pos.Y = Position.Y - offset;
                OnGround = false;
            }
            if (!OnGround)
                Velocity.Y += Gravity;
        }


        private void Attack()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Attack) && !IsAnimation)
            {
                AnimationAttack = 1;
                IsAnimation = true;
            }
            else if (AnimationManager.Animation.FrameCount - 1 == AnimationManager.Animation.CurrentFrame && AnimationAttack != 4 && IsAnimation)
            {
                var enemy = Map.Sprites[1];
                MakeDamage(enemy);
                AnimationAttack++;
            }
            if (AnimationAttack == 4 || !Keyboard.GetState().IsKeyDown(Input.Attack))
                IsAnimation = false;

        }


        private void MakeDamage(Sprite enemy)
        {
            if (Math.Abs(enemy.Position.Y - Position.Y) <= 20 && ((Position.X > enemy.Position.X && Position.X - enemy.Position.X <= TextureWidth && CurrentSide == Side.Left) || (enemy.Position.X > Position.X && enemy.Position.X - Position.X - TextureWidth <= TextureWidth && CurrentSide == Side.Right)))
            {
                enemy.Health -= Damage;
                if (CurrentSide == Side.Left)
                    enemy.Pos.X -= offset;
                else
                    enemy.Pos.X += offset;
            }
        }
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            if (IsDying)
            {
                UpdatePosition();
                SetAnimations();
                AnimationManager.Update(gameTime);
            }
            else if (!IsDead)
            {
                Hit();
                Attack();
                Move();
                Jump();
                UpdatePosition();
                SetAnimations();
                AnimationManager.Update(gameTime);
                Velocity.X = Vector2.Zero.X;
            }
        }
    }
}
