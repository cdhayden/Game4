using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    /// <summary>
    /// a bounding rectangle for detecting collision
    /// </summary>
    public struct CollisionRectangle
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

        public CollisionRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public CollisionRectangle(Vector2 Position, float width, float height)
        {
            X = Position.X;
            Y = Position.Y;
            Width = width;
            Height = height;
        }

        public CollisionRectangle() 
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }

        /// <summary>
        /// determines collision between this rectangle and another
        /// </summary>
        /// <param name="other">the other rectangle</param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(CollisionRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// determines collision between this rectangle and a circle
        /// </summary>
        /// <param name="other">the circle</param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(CollisionCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public CollisionRectangle ShiftRight(float offset) 
        { 
            return new CollisionRectangle((int)(X + offset), (int)Y, (int)Width, (int)Height);
        }

        public CollisionRectangle ShiftLeft(float offset)
        {
            return new CollisionRectangle((int)(X - offset), (int)Y, (int)Width, (int)Height);
        }

        public CollisionRectangle ShiftUp(float offset)
        {
            return new CollisionRectangle((int)X, (int)(Y - offset), (int)Width, (int)Height);
        }

        public CollisionRectangle ShiftDown(float offset)
        {
            return new CollisionRectangle((int)X, (int)(Y + offset), (int)Width, (int)Height);
        }
    }
}
