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

        Texture2D bubbleTexture, backgroundTexture, pufferfishTexture;

        SpriteFont scoreFont;

        Rectangle window, pufferfishRect;

        Screen screen;

        float seconds;

        int score, scrollValue, prevScrollValue, scrollChange, puffersfishPopped;


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



            pufferfishTexture = Content.Load<Texture2D>("Images/pufferfish");
            bubbleTexture = Content.Load<Texture2D>("Images/pinkbubble");
            backgroundTexture = Content.Load<Texture2D>("Images/underwater");

            scoreFont = Content.Load<SpriteFont>("scorefont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            prevScrollValue = scrollValue;
            scrollValue = mouseState.ScrollWheelValue;
            scrollChange = scrollValue - prevScrollValue;

            scrollValue += mouseState.ScrollWheelValue;

            if (screen == Screen.Game)
            {


                seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

              

                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    


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

                for (int i = 0; i < bubbles.Count; i++)
                {
                    if (bubbles[i].Intersects(pufferfishRect))
                    {
                        bubbles.RemoveAt(i);
                        score++;
                        puffersfishPopped++;
                        i--;
                    }
                }

                if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                {
                    pufferfishRect = new Rectangle(mouseState.X - 335, mouseState.Y - 335, 670, 670);
                }

                if (scrollChange >= 0)
                {
                    Rectangle tempBubble = new Rectangle(generator.Next(0, window.Width - 100), generator.Next(0, window.Height - 100), 100, 100);
                    bubbles.Add(tempBubble);

                }

                if (seconds >= 0.5 && bubbles.Count < 50)
                {

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

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (screen == Screen.Game)
            {
                _spriteBatch.Draw(backgroundTexture, window, Color.White);

                _spriteBatch.Draw(pufferfishTexture, pufferfishRect, Color.White);

                for (int i = 0; i < bubbles.Count; i++)
                {
                    _spriteBatch.Draw(bubbleTexture, bubbles[i], Color.White);
                }

                _spriteBatch.DrawString(scoreFont, $"Bubbles Popped: {score}", new Vector2 (10, 10), Color.Orange);
                _spriteBatch.DrawString(scoreFont, $"Popped by fish: {puffersfishPopped}", new Vector2(10, 50), Color.Orange);

            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
