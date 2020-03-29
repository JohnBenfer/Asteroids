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
    public class BoostParticle
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

        public BoostParticle(Game1 game, ContentManager content, double x, double y, double playerRotation)
        {
            LoadContent(content);

            this.y = y;
            this.x = x;

            this.playerRotation = playerRotation;
            

            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            Random random = new Random();


            int i = random.Next(0, 6);
            if(i < 3) {

                this.color = Color.Red;

             }
            else if(i == 4 || i == 3){

                this.color = Color.Orange;

            } else if(i==5) {

                this.color = Color.Yellow;

                }

            scale = 0.02 * random.Next(1, 5);
            velocity = random.Next(4, 6);
            rotation = random.Next(0, 90);

        }

        public void Update()
        {
            life -= 0.1;
            scale -= 0.008;

            x -= Math.Sin(ConvertToRadians(playerRotation)) * velocity;
            y += Math.Cos(ConvertToRadians(playerRotation)) * velocity;

            velocity -= 0.46;

            if(velocity <= 0)
            {
                life = 0;
            }

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
