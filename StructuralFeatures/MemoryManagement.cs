using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures
{
    public class MemoryManagement
    {
        #region in, ref keyword in Struct

        public struct Point
        {
            public int X { get; }
            public int Y { get; }
            public Point(int y, int x)
            {
                Y = y;
                X = x;
            }

            private static Point _origin = new Point(0, 0);
            public static ref readonly Point Origin => ref _origin; // It pass the value type by reference, without coping it
        }

        public double MesureDistance(in Point a, in Point b) // Parameters would be read-only
        {
            var dy = a.Y - b.Y;
            var dx = a.X - b.X;
            return Math.Sqrt(dy * dy + dx * dx);
        }

        [Test]
        public void TraInKeyword()
        {
            Point a = new Point(1, 9);

            var distance = MesureDistance(a, Point.Origin); // Variables are passed by reference rather than copy them

            Console.WriteLine($"Distance is {distance}");
        }


        #endregion
    }
}
