using System.Drawing;
using System.Xml.Serialization;

namespace ForgeBot
{
    using System;

    public class Coordinate
    {
        public string Name;
        public int X;
        public int Y;

        public Coordinate()
        {
        }

        public Coordinate(int newX, int newY)
        {
            this.X = newX;
            this.Y = newY;
        }

        public Coordinate(int newX, int newY, string newName)
        {
            this.X = newX;
            this.Y = newY;
            this.Name = newName;
        }
    }
}
