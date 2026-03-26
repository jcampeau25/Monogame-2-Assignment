using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Monogame_2___Assignment
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        MouseState mouseState, prevMouseState;

        Texture2D bubbleTexture, backgroundTexture, bombTexture, explosionTexture;

        Rectangle window, bombRect, explosionRect;

        Screen screen;

        float seconds, bombSeconds;

        int score, scrollValue;

        bool bomb = false;
        bool explosion = false;
        bool collision = false;

        Random generator = new Random();

        List<Rectangle> bubbles = new List<Rectangle>();

        enum Screen
        {
            Intro,
            Game,
            End
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
 
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            seconds = 0f;
            screen = Screen.Game;
            score = 0;
            scrollValue = 0;

            base.Initialize();

            window = new Rectangle(0, 0, 900, 600);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            

            bombTexture = Content.Load<Texture2D>("Images/blackbomb");
            explosionTexture = Content.Load<Texture2D>("Images/explosion");
            bubbleTexture = Content.Load<Texture2D>("Images/pinkbubble");
            backgroundTexture = Content.Load<Texture2D>("Images/underwater");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            scrollValue += mouseState.ScrollWheelValue;

            if (screen == Screen.Game)
            {


                seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (bomb == false)
                {
                    if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                    {
                        bomb = true;
                        bombRect = new Rectangle(mouseState.X - 62, mouseState.Y - 50, 125, 100);
                        explosionRect = new Rectangle(mouseState.X - 150, mouseState.Y - 150, 300, 300);
                    }
                }

                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    if (bombRect.Contains(mouseState.Position))
                    {
                        explosion = true;

                    }

                    for (int i = 0; i < bubbles.Count; i++)
                    {
                        if (bubbles[i].Contains(mouseState.Position))
                        {
                            bubbles.RemoveAt(i);
                            score++;
                            i--;
                        }
                    }
                }


                if ((seconds >= 0.5 && bubbles.Count < 50) || scrollValue >=1)
                {
                    scrollValue -= scrollValue;

                    Rectangle tempBubble = new Rectangle(generator.Next(0, window.Width - 100), generator.Next(0, window.Height - 100), 100, 100);

                    for (int i = 0; i < bubbles.Count; i++)
                    {
                        if (bubbles[i].Intersects(tempBubble))
                        {
                            collision = true;
                        }


                    }

                    while (collision == true)
                    {
                        tempBubble = new Rectangle(generator.Next(0, window.Width - 100), generator.Next(0, window.Height - 100), 100, 100);
                        collision = false;
                    }

                    bubbles.Add(tempBubble);
                    seconds = 0;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();

            if (screen == Screen.Game)
            {
                _spriteBatch.Draw(backgroundTexture, window, Color.White);

                if (bomb == true)
                {
                    _spriteBatch.Draw(bombTexture, bombRect, Color.White);
                }

                if (explosion == true)
                {
                    _spriteBatch.Draw(explosionTexture, explosionRect, Color.White);

                    if (bombSeconds >= 1)
                    {
                        explosion = false;
                    }
                }

                for (int i = 0; i < bubbles.Count; i++)
                {
                    _spriteBatch.Draw(bubbleTexture, bubbles[i], Color.White);
                }
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
