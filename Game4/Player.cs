using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Game4
{
    public class Player
    {
        private CharacterSprite[] _sprites = new CharacterSprite[3];

        private float _stretch;

        private Viewport _viewport;

        private Tilemap[] _tilemaps;

        private float _gravity = 500;

        private float _speed = 100;

        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }

        public bool FacingLeft { get; private set; }

        public CollisionRectangle Bounds => new CollisionRectangle((int)(Position.X + 43 * _stretch), (int)(Position.Y + 42 * _stretch), (int)(13 * _stretch), (int)(15 * _stretch));

        public Vector2 Center => new Vector2(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2);

        public Player(Vector2 pos, Viewport viewport, Tilemap[] tilemaps, float xVelocity = 0, float yVelocity = 0, bool left = false)
        {
            Position = pos;
            _sprites[(int)GameColor.Red] = new CharacterSprite("Knight/Knight/Knight", 6, 8, 100, 100);
            _sprites[(int)GameColor.Green] = new CharacterSprite("Archer/Archer/Archer", 6, 8, 100, 100);
            _sprites[(int)GameColor.Blue] = new CharacterSprite("Wizard/Wizard/Wizard", 6, 8, 100, 100);
            _stretch = 2;
            _viewport = viewport;
            _tilemaps = tilemaps;
            FacingLeft = left;
            Velocity = new Vector2(xVelocity, yVelocity);
        }

        public void LoadContent(ContentManager content) 
        {
            foreach(var sprite in _sprites)
            {
                sprite.LoadContent(content);
            }
        }   

        public void Update(GameTime gameTime, GameColor color, KeyboardState past, KeyboardState current) 
        {
            bool downBlocked = false;
            bool upBlocked = false;
            bool rightBlocked = false;
            bool leftBlocked = false;

            var collisionTiles = _tilemaps[(int)color].Bounds;
            
            foreach (CollisionRectangle cr in collisionTiles) 
            {
                if (CollisionHelper.Collides(cr, new Vector2(Bounds.Center.X, Bounds.Bottom + 1))) 
                {
                    downBlocked = true;
                }
                if (CollisionHelper.Collides(cr, new Vector2(Bounds.Center.X, Bounds.Top - 1)))
                {
                    upBlocked = true;
                }
                if (CollisionHelper.Collides(cr, new Vector2(Bounds.Left - 1, Bounds.Center.Y)))
                {
                    leftBlocked = true;
                }
                if (CollisionHelper.Collides(cr, new Vector2(Bounds.Right + 1, Bounds.Center.Y)))
                {
                    rightBlocked = true;
                }
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(current.IsKeyDown(Keys.Right) && !rightBlocked && Bounds.Right < _viewport.X + _viewport.Width)
            {
                Velocity = new Vector2(_speed, Velocity.Y);
            }
            else if(current.IsKeyDown(Keys.Left) && !leftBlocked && Bounds.Left > _viewport.X)
            {
                Velocity = new Vector2(-_speed, Velocity.Y);
            }
            else 
            {
                Velocity = new Vector2(0, Velocity.Y);
            }

            if (current.IsKeyDown(Keys.Up) && past.IsKeyUp(Keys.Up) && !upBlocked && downBlocked)
            {
                Velocity = new Vector2(Velocity.X, -300);
            }
            else if (downBlocked || upBlocked)
            {
                Velocity = new Vector2(Velocity.X, 0);
            }

            Vector2 acceleration = Vector2.Zero;
            if (!downBlocked)
            {
                acceleration += new Vector2(0, _gravity);
            }

            Velocity += acceleration * dt;
            Position += dt * Velocity;

            if(Velocity.X < 0)
                FacingLeft = true;
            else if (Velocity.X > 0)
                FacingLeft = false;

            foreach (var sprite in _sprites)
                sprite.Update(gameTime, Velocity);
        }

        public void Draw(GameColor color, SpriteBatch spriteBatch) 
        {
            _sprites[(int)color].Draw(spriteBatch, Position, _stretch, FacingLeft);
        }

        public void Reset(Vector2 spawn) 
        {
            Position = spawn;
            Velocity = Vector2.Zero;
        }
    }
}
