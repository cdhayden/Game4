using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Game4Content
{
    /// <summary>
    /// An importer for a tilemap file. The purpose of an importer to to load all important data 
    /// from a file into a content object; any processing of that data occurs in the subsequent content
    /// processor step. 
    /// </summary>
    [ContentImporter(".tmap", DisplayName = "TilemapImporter", DefaultProcessor = "TilemapProcessor")]
    public class TilemapImporter : ContentImporter<TilemapContent>
    {
        public override TilemapContent Import(string filename, ContentImporterContext context)
        {
            // Create a new BasicTilemapContent
            TilemapContent map = new();

            string data = File.ReadAllText(filename);
            var lines = data.Split('\n');

            //firest line is the tileset filename
            map.TileFilename = lines[0].Trim();

            //second line is the tile size
            var secondline = lines[1].Split(',');
            map.TileWidth = int.Parse(secondline[0]);
            map.TileHeight = int.Parse(secondline[1]);

            //third line is the map size
            var thirdline = lines[2].Split(',');
            map.MapWidth = int.Parse(thirdline[0]);
            map.MapHeight = int.Parse(thirdline[1]);

            //Now we can create the map
            int count = lines.Length - 2;      // Number of elements to join
            string result = string.Join(",", lines.Skip(3).Take(count));
            map.Map = result.Split(',').Select(index => int.Parse(index)).ToArray();

            map.Bounds = new CollisionRectangleContent[map.Map.Length];
            int mapIndex = 0;
            for (int i = 3; i < lines.Length; i++)
            {
                var nextLine = lines[i].Split(',');
                int curIndex = 0;
                while (curIndex < nextLine.Length && mapIndex < map.Map.Length)
                {
                    int x = int.Parse(nextLine[curIndex]);
                    map.Map[mapIndex + curIndex] = x;
                    if (x != 0)
                        map.Bounds[mapIndex + curIndex] = new CollisionRectangleContent((mapIndex + curIndex) % map.MapWidth * map.TileWidth, (mapIndex + curIndex) / map.MapWidth * map.TileHeight, map.TileWidth, map.TileHeight);
                    curIndex++;
                }
                mapIndex += nextLine.Length;
                if (mapIndex >= map.Map.Length) break;
            }

            return map;
        }
    }
}
