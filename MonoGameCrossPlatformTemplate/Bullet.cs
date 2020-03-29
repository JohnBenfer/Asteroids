using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameCrossPlatformTemplate
{
    class Bullet
    {
        public static Texture2D texture;
        public static int screenWidth;
        public static int screenHeight;
        static int bulletWidth;
        static int bulletHeight;
        public static double speed;
        public static Vector2 origin;
        Game1 game;
        ContentManager content;

        //BulletModel bulletModel;

        public bool isActive;
        public double X;
        public double Y;
        public CircleHitBox hitBox;

        List<BulletParticle> particles = new List<BulletParticle>();

        public bool Killed = false;

        public double rotation;

        public Bullet(Game1 game, ContentManager content)
        {
            this.content = content;
            //bulletModel = game.bulletModel;
            texture = content.Load<Texture2D>("Bullet");
            screenHeight = game.graphics.PreferredBackBufferHeight;
            screenWidth = game.graphics.PreferredBackBufferWidth;
            bulletWidth = texture.Width;
            bulletHeight = texture.Height;
            speed = 18;
            origin = new Vector2(bulletWidth / 2, bulletHeight / 2);

            isActive = false;
            this.game = game;
        }

        /// <summary>
        /// activates the bullet and will start updating and drawing
        /// </summary>
        /// <param name="playerX"></param>
        /// <param name="PlayerY"></param>
        /// <param name="playerRotation"></param>
        public Bullet SpawnBullet(double playerX, double PlayerY, double playerRotation)
        {
            isActive = true;
            Killed = false;
            rotation = playerRotation;
            X = playerX;
            Y = PlayerY;
            hitBox = new CircleHitBox(10, X, Y);
            return this;
        }

        public void Update()
        {
            X += Math.Sin(ConvertToRadians(rotation)) * speed;
            Y -= Math.Cos(ConvertToRadians(rotation)) * speed;

            hitBox.X = X;
            hitBox.Y = Y;

            if (X > game.GAME_WIDTH + 10 || X < -10 || Y > game.GAME_HEIGHT + 10 || Y < -10)
            {
                Killed = true;
            }

            List<BulletParticle> deadBulletParticles = new List<BulletParticle>();
            foreach (BulletParticle b in particles)
            {
                b.Update();
                if(b.life <= 0)
                {
                    deadBulletParticles.Add(b);
                }
            }
            foreach(BulletParticle b in deadBulletParticles)
            {
                particles.Remove(b);
            }
            SpawnParticle();
            
        }

        private void SpawnParticle()
        {
            particles.Add(new BulletParticle(game, content, X, Y, rotation));
        }

        private float ConvertToRadians(double degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)X, (int)Y, 70, 70), null, Color.White, (float)ConvertToRadians(rotation), origin, SpriteEffects.None, 0.6f);
            foreach(BulletParticle b in particles)
            {
                b.Draw(spriteBatch);
            }
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Bullet");
        }
    }
}