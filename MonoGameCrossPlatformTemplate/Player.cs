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
    class Player
    {
        Texture2D texture;
        SoundEffect shooting;
        float soundEffectVolume;
        public CircleHitBox hitBox;
        double rotation;
        Vector2 origin;
        public Color color;
        public double X;
        public double Y;

        static int screenWidth;
        static int screenHeight;

        public int playerWidth;
        public int playerHeight;

        double speed;

        public Stack<Bullet> inactiveBullets;
        public List<Bullet> activeBullets;
        ContentManager content;
        Game1 game;

        bool shootLock = false;
        static int maxBullets = 20;

        List<BoostParticle> particles = new List<BoostParticle>();

        public Player(Game1 game, ContentManager content)
        {
            screenHeight = game.graphics.PreferredBackBufferHeight;
            screenWidth = game.graphics.PreferredBackBufferWidth;
            this.content = content;



            LoadContent(content);
            this.game = game;
            this.soundEffectVolume = game.soundEffectVolume;

            X = game.GAME_WIDTH / 2;
            Y = game.GAME_HEIGHT / 2;
            rotation = 0;
            inactiveBullets = new Stack<Bullet>();
            activeBullets = new List<Bullet>();
            InitializeBullets(maxBullets);
            playerWidth = texture.Width;
            playerHeight = texture.Height;

            origin = new Vector2(playerWidth / 2, playerHeight / 2);

            speed = 8;

            hitBox = new CircleHitBox(40, X, Y);

            color = Color.White;
        }

        private void InitializeBullets(int n)
        {
            int counter = 0;
            while (counter < n)
            {
                inactiveBullets.Push(new Bullet(game, content));
                counter++;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)X, (int)Y, 300, 200), null, color, ConvertToRadians(rotation + 180), origin, SpriteEffects.None, 1f);
            foreach (Bullet b in activeBullets)
            {
                if (b.isActive)
                {
                    b.Draw(spriteBatch);
                }
            }

            foreach (BoostParticle p in particles)
            {
                p.Draw(spriteBatch);
            }
        }

        private float ConvertToRadians(double degrees)
        {
            return (float)(degrees * Math.PI / 180);
        }

        private double ConvertToDegrees(double degrees)
        {
            return -1;
        }

        public void Update()
        {

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                rotation -= speed * 0.7;

            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                rotation += speed * 0.7;

            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                X += Math.Sin(ConvertToRadians(rotation)) * speed;
                Y -= Math.Cos(ConvertToRadians(rotation)) * speed;
                SpawnBoost();
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                X -= Math.Sin(ConvertToRadians(rotation)) * speed;
                Y += Math.Cos(ConvertToRadians(rotation)) * speed;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (!shootLock)
                {
                    Shoot();
                    shootLock = true;
                }

            }
            else
            {
                shootLock = false;
            }

            if (X < 50)
            {
                X = 50;
            }
            if (X > game.GAME_WIDTH - 100)
            {
                X = game.GAME_WIDTH - 100;
            }
            if (Y < 50)
            {
                Y = 50;
            }
            if (Y > game.GAME_HEIGHT - 50)
            {
                Y = game.GAME_HEIGHT - 50;
            }

            hitBox.X = X;
            hitBox.Y = Y;

            List<Bullet> temp = new List<Bullet>();
            foreach (Bullet b in activeBullets)
            {
                b.Update();
                if (b.Killed)
                {
                    temp.Add(b);
                }
            }
            foreach (Bullet b in temp)
            {
                b.Killed = false;
                b.isActive = false;
                inactiveBullets.Push(b);
                activeBullets.Remove(b);
            }


            List<BoostParticle> deadBoostParticles = new List<BoostParticle>();
            foreach (BoostParticle particle in particles)
            {
                particle.Update();
                if (particle.life <= 0)
                {
                    deadBoostParticles.Add(particle);
                }
            }
            foreach (BoostParticle p in deadBoostParticles)
            {
                particles.Remove(p);
            }


        } // closes update method

        private void SpawnBoost()
        {
            particles.Add(new BoostParticle(game, content, X, Y, rotation));
        }

        private void Shoot()
        {
            Console.WriteLine("active: " + activeBullets.Count + "inactive: " + inactiveBullets.Count);
            try
            {
                Bullet b = inactiveBullets.Pop().SpawnBullet(X, Y, rotation);
                b.isActive = true;
                activeBullets.Add(b);

            }
            catch (Exception e)
            {
                Console.WriteLine("Failed");
                return;
            }
            shooting.Play(soundEffectVolume * (float)0.4, 0, 0);

        }


        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Player");
            shooting = content.Load<SoundEffect>("Gun shot");
        }




    }
}