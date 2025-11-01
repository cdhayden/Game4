using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    public class Tilemap
    {
        /// <summary>
        /// The dimensions of the tiles and map
        /// </summary>
        public int TileWidth, TileHeight, MapWidth, MapHeight;

        /// <summary>
        /// array that provides collision rectangles for the tiles
        /// </summary>
        public CollisionRectangle[] Bounds;

        /// <summary>
        /// tileset texture
        /// </summary>
        public Texture2D TilesetTexture;

        /// <summary>
        /// The tile info in the tileset
        /// </summary>
        public Rectangle[] Tiles;

        /// <summary>
        /// the tile map data
        /// </summary>
        public int[] Map;

        //FIXME: delete after getting pipeline to work
        private string _filename;

        public Tilemap(string filename)
        {
            //FIXME: delete after getting pipeline to work
            _filename = filename;
        }

        //FIXME: delete after getting pipeline to work
        public void LoadContent(ContentManager content)
        {
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _filename));
            var lines = data.Split('\n');

            //first line is the tileset filename
            var tilesetFilename = lines[0].Trim();
            TilesetTexture = content.Load<Texture2D>(tilesetFilename);

            //second line is the tile size
            var secondline = lines[1].Split(',');
            TileWidth = int.Parse(secondline[0]);
            TileHeight = int.Parse(secondline[1]);

            //determine tile bounds
            int columns = TilesetTexture.Width / TileWidth;
            int rows = TilesetTexture.Height / TileHeight;
            Tiles = new Rectangle[columns * rows];

            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int index = y * columns + x;
                    Tiles[index] = new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
                }
            }

            //third line is the map size
            var thirdline = lines[2].Split(',');
            MapWidth = int.Parse(thirdline[0]);
            MapHeight = int.Parse(thirdline[1]);

            //Now we can create the map
            Map = new int[MapWidth * MapHeight];
            Bounds = new CollisionRectangle[Map.Length];
            int mapIndex = 0;
            for (int i = 3; i < lines.Length; i++) 
            {
                var nextLine = lines[i].Split(',');
                int curIndex = 0;
                while (curIndex < nextLine.Length && mapIndex < MapHeight * MapWidth)
                {
                    int x = int.Parse(nextLine[curIndex]);
                    Map[mapIndex + curIndex] = x;
                    if(x != 0)
                        Bounds[mapIndex + curIndex] = new CollisionRectangle((mapIndex + curIndex) % MapWidth * TileWidth, (mapIndex + curIndex) / MapWidth * TileHeight, TileWidth, TileHeight);
                    curIndex++;
                }
                mapIndex += nextLine.Length;
                if (mapIndex >= MapHeight * MapWidth) break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool active)
        {
            float alpha = active ? 1.0f : 0.5f;
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int index = Map[y * MapWidth + x] - 1;
                    if (index == -1) continue;
                    spriteBatch.Draw(TilesetTexture, new Vector2(x * TileWidth, y * TileHeight), Tiles[index], Color.White * alpha);
                }
            }
        }
    }
}
