using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4Content
{
    /// <summary>
    /// a bounding rectangle for detecting collision
    /// </summary>
    [ContentSerializerRuntimeType("Game4.CollisionRectangle, Game4")]
    public struct CollisionRectangleContent
    {
        public float X;

        public float Y;

        public float Width;

        public float Height;

        public float Left => X;

        public float Right => X + Width;

        public float Top => Y;

        public float Bottom => Y + Height;

        public Vector2 Center => new Vector2(X + Width / 2, Y + Height / 2);

        public CollisionRectangleContent(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public CollisionRectangleContent(Vector2 Position, float width, float height)
        {
            X = Position.X;
            Y = Position.Y;
            Width = width;
            Height = height;
        }

        public CollisionRectangleContent() 
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }
    }
}
