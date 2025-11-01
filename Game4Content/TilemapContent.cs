using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4Content
{
    [ContentSerializerRuntimeType("Game4.Tilemap, Game4")]
    public class TilemapContent
    {
        /// <summary>
        /// The dimensions of the tiles and map
        /// </summary>
        public int TileWidth, TileHeight, MapWidth, MapHeight;

        /// <summary>
        /// array that provides collision rectangles for the tiles
        /// </summary>
        public CollisionRectangleContent[] Bounds;

        /// <summary>
        /// tileset texture
        /// </summary>
        public Texture2DContent TilesetTexture;

        /// <summary>
        /// The tile info in the tileset
        /// </summary>
        public Rectangle[] Tiles;

        /// <summary>
        /// the tile map data
        /// </summary>
        public int[] Map;

        /// <summary>
        /// the name of the file with the tile image
        /// </summary>
        [ContentSerializerIgnore]
        public string TileFilename;

        /// <summary>
        /// the name of the file with the map
        /// </summary>
        [ContentSerializerIgnore]
        public string MapFilename;
    }
}
