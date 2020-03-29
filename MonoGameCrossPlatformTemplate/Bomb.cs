using System;
namespace MonoGameCrossPlatformTemplate
{
    public class Bomb
    {
        double x;
        double y;

        public Bomb()
        {

            Random random = new Random();

            x = random.Next(0, 3);

        }

        public void Update()
        {

        }

        public void Draw()
        {

        }
        
    }
}
