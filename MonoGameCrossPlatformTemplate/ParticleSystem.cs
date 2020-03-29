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
    public class ParticleSystem
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

        double playerRotation;

        Vector2 origin;

        public ParticleSystem(Game1 game, ContentManager content, double x, double y, Color color)
        {
            LoadContent(content);

            this.y = y;
            this.x = x;
            
            

            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            Random random = new Random();


            int i = random.Next(0, 3);
            if(i == 1) {

                this.color = Color.Red;

             }
            else if(i == 2){

                this.color = Color.Orange;

            } else if(i==0) {

                this.color = Color.Yellow;

                }

            scale = 0.01 * random.Next(1, 5);
            velocity = random.Next(4, 6);
            rotation = random.Next(0, 90);

        }

        public void Update()
        {
            life -= 0.1;

            x += Math.Sin(ConvertToRadians(rotation)) * velocity;
            y -= Math.Cos(ConvertToRadians(rotation)) * velocity;

            velocity -= 0.5;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)x, (int)y, (int)(texture.Width*scale), (int)(texture.Height * scale)), null, color, ConvertToRadians(rotation + 180), origin, SpriteEffects.None, 0.1f);

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
