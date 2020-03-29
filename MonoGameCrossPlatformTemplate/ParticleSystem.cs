using System;
using Microsoft.Xna.Framework;

namespace MonoGameCrossPlatformTemplate
{
    public class ParticleSystem
    {

        public double x;
        public double y;

        public double velocity;

        public double acceleration;

        public float life;

        public float scale;

        public Color color;

        public ParticleSystem(double x, double y, Color color, float scale)
        {
            this.y = y;
            this.x = x;
            this.color = color;
            this.scale = scale;


        }







    }
}
