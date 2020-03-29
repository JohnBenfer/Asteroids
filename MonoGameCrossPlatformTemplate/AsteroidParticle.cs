using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameCrossPlatformTemplate
{
    public class AsteroidParticle
    {
        private Texture2D texture;
        public double x;
        public double y;

        public double velocity;

        public double acceleration;

        public double life = 5;

        public double scale;

        public Color color;

        double rotation;


        Vector2 origin;

        public AsteroidParticle(Game1 game, ContentManager content, double x, double y)
        {
            LoadContent(content);

            this.y = y;
            this.x = x;



            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            Random random = new Random();


            int i = random.Next(0, 6);
            if (i < 3)
            {

                this.color = Color.SaddleBrown;

            }
            else if (i == 4 || i == 3)
            {

                this.color = Color.Orange;

            }
            else if (i == 5)
            {

                this.color = Color.Brown;

            }

            scale = random.Next(5, 15);
            velocity = random.Next(6, 9);
            rotation = random.Next(0, 360);

        }

        public void Update()
        {
            life -= 0.1;
            scale -= 0.5;

            x -= Math.Sin(ConvertToRadians(rotation)) * velocity;
            y += Math.Cos(ConvertToRadians(rotation)) * velocity;

            velocity -= 0.25;

            if (velocity <= 0)
            {
                life = 0;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)x, (int)y, (int)(scale), (int)(scale)), null, color, ConvertToRadians(rotation + 180), origin, SpriteEffects.None, 0.1f);

        }

        private float ConvertToRadians(double degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("whitesquare");
        }

    }
}
