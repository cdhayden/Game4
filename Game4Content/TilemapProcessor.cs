using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Game4Content
{
    /// <summary>
    /// Processes a BasicTilemapContent object, building and linking the associated texture 
    /// and setting up the tile information.
    /// </summary>
    [ContentProcessor(DisplayName = "TilemapProcessor")]
    public class TilemapProcessor : ContentProcessor<TilemapContent, TilemapContent>
    {
        public override TilemapContent Process(TilemapContent map, ContentProcessorContext context)
        {
            // We need to build the tileset texture associated with this tilemap
            // This will create the binary texture file and link it to this tilemap so 
            // they get loaded together by the ContentProcessor.  
            //map.TilesetTexture = context.BuildAsset<Texture2DContent, Texture2DContent>(map.TilesetTexture, "Texture2DProcessor");
            map.TilesetTexture = context.BuildAndLoadAsset<TextureContent, Texture2DContent>(new ExternalReference<TextureContent>(map.TileFilename), "TextureProcessor");

            // Determine the number of rows and columns of tiles in the tileset texture            
            int tilesetColumns = map.TilesetTexture.Mipmaps[0].Width / map.TileWidth;
            int tilesetRows = map.TilesetTexture.Mipmaps[0].Height / map.TileWidth;

            // We need to create the bounds for each tile in the tileset image
            // These will be stored in the tiles array
            map.Tiles = new Rectangle[tilesetColumns * tilesetRows];
            context.Logger.LogMessage($"{map.Tiles.Length} Total tiles");
            for (int x = 0; x < tilesetColumns; x++)
            {
                for (int y = 0; y < tilesetRows; y++)
                {
                    int index = y * tilesetColumns + x;
                    map.Tiles[index] = new Rectangle(x * map.TileWidth, y * map.TileHeight, map.TileWidth, map.TileHeight);
                }
            }

            for (int i = 0; i < map.Map.Length; i++)
            {
                if (map.Map[i] != 0)
                    map.Bounds[i] = new CollisionRectangleContent((i) % map.MapWidth * map.TileWidth, (i) / map.MapWidth * map.TileHeight, map.TileWidth, map.TileHeight);
            }

            // Return the fully processed tilemap
            return map;
        }
    }
}