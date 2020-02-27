using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;


namespace MonoGameCrossPlatformTemplate
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        // Window dimensions
        int WINDOW_WIDTH = 1920;
        int WINDOW_HEIGHT = 1080;

        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        int asteroidCount;
        int level;
        List<Asteroid> asteroids;
        List<Asteroid> closeAsteroids;
        int maxAsteroids;
        Texture2D background;
        Texture2D SpaceBackground;
        SoundEffect gameOver;
        public float soundEffectVolume;
        SoundEffect asteroidDestroyed;
        Song backgroundMusic;
        float musicVolume;
        public int FRAME_RATE = 60;
        int score;
        SpriteFont scoreFont;
        bool isGameOver;
        int width;
        int height;
        int drawCount;
       
        bool nearX;
        bool nearY;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            //graphics.PreferredBackBufferWidth = 3840;
            //graphics.PreferredBackBufferHeight = 2050;
            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
            //graphics.PreferredBackBufferWidth = 3000;
            //graphics.PreferredBackBufferHeight = 2000;
            //graphics.ApplyChanges();

            SetGraphics();
            drawCount = 0;
            soundEffectVolume = 0.15f;
            musicVolume = 0.2f;
            level = 1;
            asteroidCount = 0;
            asteroids = new List<Asteroid>();
            closeAsteroids = new List<Asteroid>();
            width = graphics.PreferredBackBufferWidth;
            height = graphics.PreferredBackBufferHeight;
            SetMaxAsteroids();

            base.Initialize();
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.Volume = musicVolume;
            score = 0;
            isGameOver = false;
            player = new Player(this, Content);
          
            nearX = false;
            nearY = false;
        }

        private void SetMaxAsteroids()
        {
            if (WINDOW_WIDTH <= 2000)
            {
                maxAsteroids = 8;

            }
            else if (WINDOW_WIDTH <= 2500)
            {
                maxAsteroids = 10;
            }
            else if (WINDOW_WIDTH <= 3000)
            {
                maxAsteroids = 12;
            }
            else
            {
                maxAsteroids = 14;
            }
        }

        private void SetGraphics()
        {
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.ApplyChanges();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            background = Content.Load<Texture2D>("Space");
            //SpaceBackground = Content.Load<Texture2D>("AsteroidSpriteSheet");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameOver = Content.Load<SoundEffect>("Player hit");
            asteroidDestroyed = Content.Load<SoundEffect>("Asteroid Destroyed");
            backgroundMusic = Content.Load<Song>("Background Song");
            scoreFont = Content.Load<SpriteFont>("Score");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!isGameOver)
            {
                Console.WriteLine(closeAsteroids.Count);
                player.Update();
                List<Asteroid> AsteroidsDestroyedByPlayer = new List<Asteroid>();
                List<Asteroid> AsteroidsOffScreen = new List<Asteroid>();
                List<Bullet> BulletsHitAsteroid = new List<Bullet>();
                foreach (Asteroid asteroid in asteroids)
                {

                    // this adds asteroids to a spatially aware list

                    if (((asteroid.hitBox.X + (asteroid.hitBox.radius) > (player.X - player.hitBox.radius)) // right side of asteroid and left side of player
                        && (asteroid.hitBox.X + (asteroid.hitBox.radius) < (player.X + player.hitBox.radius))) // right side of asteroid and right side of player
                        || ((asteroid.hitBox.X - (asteroid.hitBox.radius) < (player.X + player.hitBox.radius)) // left side of asteroid and right side of player
                        && (asteroid.hitBox.X - (asteroid.hitBox.radius) > (player.X - player.hitBox.radius)))) // left side of asteroid and left side of player
                    {
                        //asteroid.color = Color.Green;
                        nearX = true;

                    }
                    else
                    {

                        nearX = false;
                    }

                    if (((asteroid.hitBox.Y + (asteroid.hitBox.radius) > (player.Y - player.hitBox.radius)) // bottom of asteroid and top of player
                        && (asteroid.hitBox.Y + (asteroid.hitBox.radius) < (player.Y + player.hitBox.radius))) // bottom of asteroid and bottom of player
                        || ((asteroid.hitBox.Y - (asteroid.hitBox.radius) < (player.Y + player.hitBox.radius)) // top of asteroid and bottom of player
                        && (asteroid.hitBox.Y - (asteroid.hitBox.radius) > (player.Y - player.hitBox.radius)))) // top of asteroid and top of player
                    {
                        // asteroid is near player in Y
                        //asteroid.color = Color.Blue;
                        nearY = true;
                    }
                    else
                    {
                        nearY = false;
                    }

                    if (nearX && nearY)
                    {
                        if (!closeAsteroids.Contains(asteroid))
                        {
                            closeAsteroids.Add(asteroid);
                        }
                        //asteroid.color = Color.Red;
                    }
                    else
                    {
                        try
                        {
                            closeAsteroids.Remove(asteroid);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    asteroid.Update(gameTime);

                    foreach (Bullet b in player.activeBullets)
                    {
                        if (asteroid.hitBox.CollidesWith(b.hitBox) && !asteroid.Hit)
                        {
                            AsteroidsDestroyedByPlayer.Add(asteroid);
                            b.Killed = true;
                            asteroid.Exploding = true;
                            asteroid.Hit = true;
                            // play asteroid destroyed noise
                            asteroidDestroyed.Play(soundEffectVolume, 0, 0);
                            level++;
                            score += 10;
                        }
                    }

                    if (asteroid.OffScreen)
                    {
                        AsteroidsOffScreen.Add(asteroid);
                    }

                }

                // spatial partitioned asteroid check if hitting player
                foreach (Asteroid asteroid in closeAsteroids)
                {
                    if (asteroid.hitBox.CollidesWith(player.hitBox) && !asteroid.Exploding) // game over
                    {
                        GameOver();
                        return;
                    }
                }

                RemoveAsteroids(AsteroidsDestroyedByPlayer);

                RemoveAsteroids(AsteroidsOffScreen);

                if (asteroids.Count < level && asteroids.Count < maxAsteroids)
                {
                    asteroids.Add(new Asteroid(this, Content, player.X, player.Y));

                }

                base.Update(gameTime);
            }
            else
            {
                // game is over here..

                var k = Keyboard.GetState();
                if (k.IsKeyDown(Keys.R))
                {
                    Initialize();
                }
                if (drawCount == 2)
                {
                    SuppressDraw(); // suppress draw to lower cpu consumption after draw() has happened twice
                }
                else
                {
                    drawCount++;
                }

            }

        }


        private void RemoveAsteroids(List<Asteroid> ToDeleteAsteroids)
        {
            foreach (Asteroid a in ToDeleteAsteroids)
            {
                if (!a.Exploding)
                {
                    asteroids.Remove(a);
                    try
                    {
                        closeAsteroids.Remove(a);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            Rectangle r = new Rectangle(new Point(0, 0), new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            spriteBatch.Draw(background, r, Color.White);


            if (!isGameOver)
            {
                player.Draw(spriteBatch);
                foreach (Asteroid a in asteroids)
                {
                    a.Draw(spriteBatch);
                }
                spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(graphics.PreferredBackBufferWidth / 2, 10), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2((graphics.PreferredBackBufferWidth / 2) - 200, (graphics.PreferredBackBufferHeight / 2) - 100), Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1);
                spriteBatch.DrawString(scoreFont, "Press R to play again or ESC to quit", new Vector2((graphics.PreferredBackBufferWidth / 2) - 530, (graphics.PreferredBackBufferHeight / 2)), Color.White, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 1);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }


        private void GameOver()
        {
            gameOver.Play(soundEffectVolume, 0, 0);
            asteroids = null;
            player = null;
            MediaPlayer.Stop();
            isGameOver = true;
        }

    }
}