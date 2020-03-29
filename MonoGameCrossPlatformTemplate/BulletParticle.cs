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
    public class BulletParticle
    {
        private Texture2D texture;
        public double x;
        public double y;

        public double velocity;

        public double acceleration;

        public double life = 2;

        public double scale;

        public Color color;

        double rotation;

        double bulletRotation;

        Vector2 origin;

        public BulletParticle(Game1 game, ContentManager content, double x, double y, double bulletRotation)
        {
            LoadContent(content);

            this.y = y;
            this.x = x;
            this.x += Math.Sin(ConvertToRadians(bulletRotation)) * 5;
            this.y -= Math.Cos(ConvertToRadians(bulletRotation)) * 5;

            this.bulletRotation = bulletRotation;
            

            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            Random random = new Random();


            int i = random.Next(0, 6);
            if(i < 3) {

                this.color = Color.Red;

             }
            else if(i == 4 || i == 3){

                this.color = Color.LightSkyBlue;

            } else if(i==5) {

                this.color = Color.Yellow;

                }

            scale = random.Next(1, 9);
            velocity = random.Next(4, 6);
            rotation = random.Next(0, 360);
            this.bulletRotation += random.Next(-50, 50);
        }

        public void Update()
        {
            life -= 0.7;
            //scale -= 0.008;

            x -= Math.Sin(ConvertToRadians(bulletRotation)) * velocity;
            y += Math.Cos(ConvertToRadians(bulletRotation)) * velocity;

            velocity -= 0.7;

            if(velocity <= 0)
            {
                life = 0;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)x, (int)y, (int)(scale), (int)(scale)), null, color * 0.4f, ConvertToRadians(rotation + 180), origin, SpriteEffects.None, 0.8f);

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
