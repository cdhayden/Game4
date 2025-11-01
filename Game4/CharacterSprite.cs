using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    public class CharacterSprite
    {
        private int _idleFrameCount;
        private int _walkFrameCount;

        private string _filePath;

        private Texture2D _texture;

        private int _frameWidth;
        private int _frameHeight;

        private const float ANIMATION_SPEED = 0.15f;
        private float _animationTimer = 0;
        private int _animationFrame = 0;

        private CharacterState _state;

        public CharacterSprite(string filepath, int idleFrameCount, int walkFrameCount, int frameWidth, int frameHeight)
        {
            _idleFrameCount = idleFrameCount;
            _walkFrameCount = walkFrameCount;
            _filePath = filepath;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _state = CharacterState.Idle;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(_filePath);
        }

        public void Update(GameTime gameTime, Vector2 velocity)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _animationTimer += dt;
            if (velocity.X == 0)
            {
                if (_state != CharacterState.Idle)
                    _animationFrame = 0;
                _state = CharacterState.Idle;
            }
            else
            {
                if (_state != CharacterState.Walking)
                    _animationFrame = 0;
                _state = CharacterState.Walking;
            }

            if (_animationTimer >= ANIMATION_SPEED)
            {
                _animationTimer -= ANIMATION_SPEED;
                switch (_state)
                {
                    case CharacterState.Idle:
                        _animationFrame = (_animationFrame + 1) % _idleFrameCount;
                        break;
                    case CharacterState.Walking:
                        _animationFrame = (_animationFrame + 1) % _walkFrameCount;
                        break;

                    default:
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float stretch, bool left)
        {
            Rectangle sourceRect = new Rectangle(_animationFrame * _frameWidth, (int)_state * _frameHeight, _frameWidth, _frameHeight);
            SpriteEffects effects = left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(_texture, position, sourceRect, Color.White, 0f, Vector2.Zero, stretch, effects, 0f);
        }
    }
}
