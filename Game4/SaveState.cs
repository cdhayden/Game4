using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Game4
{
    public class SaveState
    {
        public float PlayerPositionX { get; init; }

        public float PlayerPositionY { get; init; }

        public GameColor CurrentColor { get; init; }

        public bool LeftFacing { get; init; }

        public Vector2 PersistentVelocity { get; init; }

        public SaveState(float playerPositionX, float playerPositionY, GameColor currentColor, bool leftFacing, Vector2 persistentVelocity)
        {
            PlayerPositionX = playerPositionX;
            PlayerPositionY = playerPositionY;
            CurrentColor = currentColor;
            LeftFacing = leftFacing;
            PersistentVelocity = persistentVelocity;
        }

        public string Serialize() 
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}
