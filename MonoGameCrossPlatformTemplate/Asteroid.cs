using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameCrossPlatformTemplate
{
    class Asteroid
    {
        double X;
        double Y;
        double XDelta;
        double YDelta;
        static Texture2D texture;
        static Texture2D Explosion1;
        static Texture2D Explosion2;
        static Texture2D Explosion3;
        static Texture2D Explosion4;
        Texture2D currentTexture;
        static Texture2D[] sprites;
        static int gameHeight;
        static int gameWidth;

        int frame;

        TimeSpan timer;

        static int screenWidth;
        static int screenHeight;

        public bool OffScreen = false;
        public bool Exploding = false;
        public bool Hit;

        public int width;
        public int height;

        static double speed;

        public CircleHitBox hitBox;

        public Color color;

        static Vector2 origin;

        double rotation;

        static double scale;

        Game1 game;

        public Asteroid(Game1 game, ContentManager content, double playerX, double playerY)
        {
            screenHeight = game.graphics.PreferredBackBufferHeight;
            screenWidth = game.graphics.PreferredBackBufferWidth;
            LoadContent(content);
            SetScale();
            timer = new TimeSpan(0);
            gameWidth = game.GAME_WIDTH;
            gameHeight = game.GAME_HEIGHT;
            Hit = false;

            speed = (int)(42 * scale);


            width = texture.Width;
            height = texture.Height;

            Random random = new Random();

            X = random.Next(0, 3);

            if (X == 0)
            {
                X = random.Next(-100, -1);
                Y = random.Next(-100, gameHeight + 100);
            }
            else if (X == 1)
            {
                X = random.Next(0, gameWidth);

                if (random.Next(0, 2) == 1)
                {
                    Y = random.Next(-100, -1);
                }
                else
                {
                    Y = random.Next(gameHeight + 1, gameHeight + 100);
                }
            }
            else
            {
                X = random.Next(gameWidth + 1, gameWidth + 100);
                Y = random.Next(-100, gameHeight + 100);
            }


            color = Color.White;

            origin = new Vector2(width / 2, height / 2);

            hitBox = new CircleHitBox((int)(52 * scale), X, Y);

            double slope = (playerY - Y) / (playerX - X);
            XDelta = Math.Sqrt(speed / ((slope * slope) + 1));
            YDelta = slope * XDelta;

            if (playerX < X)
            {
                XDelta *= -1;
            }
            if (playerY < Y)
            {
                YDelta *= -1;
            }

            this.game = game;
            sprites = new Texture2D[5];
            sprites[0] = texture;
            sprites[1] = Explosion1;
            sprites[2] = Explosion2;
            sprites[3] = Explosion3;
            sprites[4] = Explosion4;

            currentTexture = sprites[0];
        }




        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Asteroid2");
            Explosion1 = content.Load<Texture2D>("Explosion1");
            Explosion2 = content.Load<Texture2D>("Explosion2");
            Explosion3 = content.Load<Texture2D>("Explosion1");
            Explosion4 = content.Load<Texture2D>("Explosion3");
        }

        public void Update(GameTime gameTime)
        {
            if (!Exploding)
            {
                rotation += 0.1;

                X += XDelta;
                Y += YDelta;



                hitBox.X = X;
                hitBox.Y = Y;

                if (X > game.GAME_WIDTH + 130 || X < -130 || Y > game.GAME_HEIGHT + 130 || Y < -130)
                {
                    OffScreen = true;
                }

                currentTexture = texture;

            }
            else
            {

                timer += gameTime.ElapsedGameTime;
                Explode();
            }

        }

        private void Explode()
        {



            if ((timer.TotalMilliseconds * 7) > (game.FRAME_RATE - 58))
            {

                frame++;
                currentTexture = sprites[frame];
                timer -= new TimeSpan(0, 0, 0, 0, game.FRAME_RATE);
            }



            if (frame == 4)
            {
                Exploding = false;
                OffScreen = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentTexture, new Rectangle((int)X, (int)Y, (int)(100 * scale), (int)(scale * 120)), null, color, (float)rotation, origin, SpriteEffects.None, 0.5f);
        }

        private void SetScale()
        {
            if (screenWidth < 2000)
            {
                scale = 1;
            }
            else if (screenWidth <= 3000)
            {
                scale = 1.2;
            }
            else
            {
                scale = 1.4;
            }
        }

    }
}