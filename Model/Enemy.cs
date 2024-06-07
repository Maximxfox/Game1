using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using KnightOfLight.model.AuxiliaryClasses;
using KnightOfLight.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;


namespace KnightOfLight.model
{
    public class Enemy: Sprite
    {
        private Random Random = new Random();
        private List<Vector2> Path;
        private BFS BFS = new BFS();
        private int CurrentPath;
        private bool IsJump;
        private float Timer;
        public Enemy(List<Texture2D> animations, List<SoundEffect> sound)
        {
            Position = new Vector2(400, 410);
            Speed = 0.5f;
            Animations = new Dictionary<string, Animation>()
            {
                {"stay", new Animation(animations[0], 7, null)},
                {"hit", new Animation(animations[1],2, sound[0]) },
                {"attack", new Animation(animations[2],4, sound[1]) },
                {"death", new Animation(animations[3],4,sound[3]) },
                {"walk", new Animation(animations[4],7,sound[4]) },
            };
            Health = CurrentHealth = 40;
            TextureHeight = 64;
            TextureWidth = 64;
            AnimationManager = new AnimationManager(Animations.First().Value);
        }

        private void Move(Sprite hero, GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!OnGround)
                Velocity.Y += Gravity;
            if (!IsAnimation)
            {
                if (Path == null || CurrentPath == Path.Count || Timer >= 1f)
                {
                    Timer = 0;
                    CurrentPath = 0;
                    Path = BFS.FindPath(Map.Tiles, PositionInTiles, hero.PositionInTiles);
                }
                else
                {
                    if (Path[CurrentPath].X == PositionInTiles.X && Path[CurrentPath].Y <= PositionInTiles.Y)
                        CurrentPath++;
                    else if (Path[CurrentPath].X < PositionInTiles.X)
                    {
                        Gravity = 0.15f;
                        Velocity.X -= Speed;
                    }
                    else if (Path[CurrentPath].X > PositionInTiles.X)
                    {
                        Velocity.X += Speed;
                        Gravity = 0.15f;
                    }
                    if (Path[CurrentPath].Y > PositionInTiles.Y)
                    {
                        if (OnGround)
                            IsJump = false;
                        Jump(hero);
                    }
                }
            }
            else
            {
                Path = null;
            }
        }

        private void Jump(Sprite hero)
        {
            if (!IsJump)
            {
                Velocity.Y = -5f;
                OnGround = false;
                if (Map.Tiles[(int)PositionInTiles.X - 1, (int)PositionInTiles.Y + 1] != 0 && Position.X < hero.Position.X)
                    Velocity.X -= Speed;
                else
                    Velocity.X += Speed;
                IsJump = true;
            }
            if (!OnGround)
                Velocity.Y += Gravity;
        }

        private void SetAnimations()
        {
            if (Health <= 0)
            {
                AnimationManager.Start(Animations["death"]);
                if (AnimationManager.Animation.FrameCount - 1 == AnimationManager.Animation.CurrentFrame)
                    IsDead = true;
            }
            else if (IsHit)
            {
                CurrentHealth = Health;
                AnimationManager.Start(Animations["hit"]);
            }
            else if (IsAnimation)
                AnimationManager.Start(Animations["attack"]);
            else if (Velocity.X > 0)
            {
                AnimationManager.Start(Animations["walk"]);
                if (CurrentSide == Side.Left)
                    IsChangeSide = false;
                CurrentSide = Side.Right;
            }
            else if (Velocity.X < 0)
            {
                AnimationManager.Start(Animations["walk"]);
                if (CurrentSide == Side.Right)
                    IsChangeSide = true;
                CurrentSide = Side.Left;
            }
            else
            {
                AnimationManager.Start(Animations["stay"]);
                IsAnimation = false;
            }
        }

        private void Attack(Sprite enemy)
        {
            if (!enemy.IsDying)
            {
                var checkHitBox = Math.Abs(enemy.Position.Y - Position.Y) <= 20 && ((Position.X > enemy.Position.X && Position.X - enemy.Position.X  <=  2 * enemy.TextureWidth && CurrentSide == Side.Left) || (enemy.Position.X > Position.X && enemy.Position.X - Position.X <= 2 * enemy.TextureWidth && CurrentSide == Side.Right));
                if (checkHitBox && !enemy.IsDead && !IsAnimation)
                    IsAnimation = true;
                if (AnimationManager.Animation.FrameCount - 1 == AnimationManager.Animation.CurrentFrame && IsAnimation && !IsHit && !IsDying && checkHitBox)
                    MakeDamage(enemy, checkHitBox);
                if (AnimationManager.Animation.FrameCount - 1 == AnimationManager.Animation.CurrentFrame)
                    IsAnimation = false;
            }
        }

        private void MakeDamage(Sprite enemy, bool checkHitBox)
        {
            if (checkHitBox)
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
            if (!IsDead)
            {
                var hero = Map.Sprites[0];
                Attack(hero);
                Hit();
                if (!IsHit)
                {
                    Move(hero, gameTime);
                }
                SetAnimations();
                UpdatePosition();
                AnimationManager.Update(gameTime);
                Position += Velocity;
                Velocity = Vector2.Zero;
            }
        }
    }
}
