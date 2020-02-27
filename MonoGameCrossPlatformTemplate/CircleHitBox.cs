using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameCrossPlatformTemplate
{
    class CircleHitBox
    {
        public double radius;
        public double X;
        public double Y;

        public CircleHitBox(double radius, double X, double Y)
        {
            this.radius = radius;
            this.X = X;
            this.Y = Y;
        }

        public bool CollidesWith(CircleHitBox c2)
        {
            double distance = Math.Sqrt((X - c2.X) * (X - c2.X) + (Y - c2.Y) * (Y - c2.Y));
            if (radius + c2.radius > distance)
            {
                return true;
            }

            return false;

        }
    }
}