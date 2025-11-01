using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Configuration;
using System.IO;
using System.Text.Json.Serialization;

namespace Game4
{
    public class Game4 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Tilemap _redTilemap;

        private Tilemap[] _tileMaps = new Tilemap[3];

        private GameColor GameColor;

        private Texture2D[] backgrounds = new Texture2D[3];

        private KeyboardState _pastKeyboardState;

        private Player _player;

        private Vector2 _spawnPoint = new Vector2(0, 300);

        private string _saveFileName = "Game4Save.json";

        private GameState _gameState;

        private SpriteFont _font;


        public Game4()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _gameState = GameState.Playing;
            _tileMaps[(int)GameColor.Red] = new Tilemap("redMap.txt");
            _tileMaps[(int)GameColor.Green] = new Tilemap("greenMap.txt");
            _tileMaps[(int)GameColor.Blue] = new Tilemap("blueMap.txt");
            if (File.Exists(_saveFileName))
            {
                SaveState save = System.Text.Json.JsonSerializer.Deserialize<SaveState>(File.ReadAllText(_saveFileName));
                _player = new Player(new Vector2(save.PlayerPositionX, save.PlayerPositionY), GraphicsDevice.Viewport, _tileMaps, xVelocity: save.PersistentVelocity.X, yVelocity: save.PersistentVelocity.Y, left: save.LeftFacing);
                this.GameColor = save.CurrentColor;
            }
            else 
            {
                _player = new Player(_spawnPoint, GraphicsDevice.Viewport, _tileMaps);
                this.GameColor = GameColor.Red;
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach(Tilemap t in _tileMaps) t.LoadContent(this.Content);
            backgrounds[(int)GameColor.Red] = Content.Load<Texture2D>("redBackground");
            backgrounds[(int)GameColor.Blue] = Content.Load<Texture2D>("blueBackground");
            backgrounds[(int)GameColor.Green] = Content.Load<Texture2D>("greenBackground");
            _font = Content.Load<SpriteFont>("Saira");
            _player.LoadContent(this.Content);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState current = Keyboard.GetState();
            if (current.IsKeyDown(Keys.Escape)) 
            {
                SaveGame();
                Exit();
            }

            if (_gameState == GameState.Playing) 
            {
                if (current.IsKeyDown(Keys.R) && !CollisionHelper.Collides(_tileMaps[(int)GameColor.Red].Bounds, _player.Bounds))
                    GameColor = GameColor.Red;
                else if (current.IsKeyDown(Keys.G) && !CollisionHelper.Collides(_tileMaps[(int)GameColor.Green].Bounds, _player.Bounds))
                    GameColor = GameColor.Green;
                else if (current.IsKeyDown(Keys.B) && !CollisionHelper.Collides(_tileMaps[(int)GameColor.Blue].Bounds, _player.Bounds))
                    GameColor = GameColor.Blue;

                if(current.IsKeyDown(Keys.Space) && _pastKeyboardState.IsKeyUp(Keys.Space))
                {
                    _gameState = GameState.Paused;
                }

                _player.Update(gameTime, this.GameColor, _pastKeyboardState, current);

                if (_player.Position.Y > GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height)
                {
                    ResetGame();
                }

            }
            else if (_gameState == GameState.Paused) 
            {
                if (current.IsKeyDown(Keys.Space) && _pastKeyboardState.IsKeyUp(Keys.Space))
                {
                    _gameState = GameState.Playing;
                }
            }
            _pastKeyboardState = current;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);


            _spriteBatch.Begin();
            _spriteBatch.Draw(backgrounds[(int)GameColor], new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), null, Color.White);
            if (_gameState == GameState.Playing)
            {
                for (int i = 0; i < _tileMaps.Length; i++)
                {
                    _tileMaps[i].Draw(gameTime, _spriteBatch, i == (int)this.GameColor);
                }
                _spriteBatch.DrawString(_font, "PRESS SPACE FOR INSTRUCTIONS", new Vector2(GraphicsDevice.Viewport.Width / 2 - 140, GraphicsDevice.Viewport.Height / 2 - 20), Color.Black);
                _player.Draw(this.GameColor, _spriteBatch);
            }
            else if (_gameState == GameState.Paused) 
            {
                _spriteBatch.DrawString(_font,
                        "Follow the following instructions to play\n\n" +
                        "    1. Use arrow keys to move and jump\n" +
                        "    2. Press 'R', 'G', or 'B' to change color and platforms available\n" +
                        "    3. You cannot change color if you would collide with a platform\n" +
                        "    4. Exitting the game with 'esc' will save the game automatically\n" +
                        "    5. Falling off the map will restart the game\n" +
                        "    6. There is no win condition yet\n\n" +
                        "PRESS SPACE TO RESUME",
                        new Vector2(GraphicsDevice.Viewport.Width / 2 - 220, GraphicsDevice.Viewport.Height / 2 - 100), Color.Black
                        );
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame() 
        {
            _player.Reset(_spawnPoint);
            this.GameColor = GameColor.Red;
        }

        private void SaveGame() 
        {
            SaveState save = new SaveState(_player.Position.X, _player.Position.Y, this.GameColor, _player.FacingLeft, _player.Velocity);
            string json = save.Serialize();
            File.WriteAllText(_saveFileName, json);
        }
    }
}
