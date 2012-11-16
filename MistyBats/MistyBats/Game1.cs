using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#if __MonoCS__
using Microsoft.Xna.Framework.Input.Touch;
#endif
namespace MistyBats
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private Color textColour;
        private Color highlightColour;
        Song startSong;

        public Color TextColour
        {
            get { return textColour; }
            set { textColour = value; }
        }

        enum GAMESTATE
        {
            START, MIDDLE, END
        }

        GAMESTATE gameState;

        private Rectangle screenBounds;

        public Rectangle ScreenBounds
        {
            get { return screenBounds; }
            set { screenBounds = value; }
        }

        public static Game1 Instance;
        private GraphicsDeviceManager graphics;

        public GraphicsDeviceManager Graphics
        {
            get { return graphics; }
            set { graphics = value; }
        }
        private SpriteBatch spriteBatch;

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            set { spriteBatch = value; }
        }
        private List<GameEntity> children = new List<GameEntity>();

        public List<GameEntity> Children
        {
            get { return children; }
            set { children = value; }
        }

        string[] menuOptions = {"One Player", "Two Players", "Quit"};
        SoundEffect[] barks = new SoundEffect[10];
        SoundEffect winSound;

        Texture2D gameBackground;
        Texture2D endBackground;
        Texture2D startBackground;
        Texture2D cursor;

        Ball ball;
        int highlighted = -1;

        public Ball Ball
        {
            get { return ball; }
            set { ball = value; }
        }
        Bat leftBat;

        public Bat LeftBat
        {
            get { return leftBat; }
            set { leftBat = value; }
        }
        Bat rightBat;

        public Bat RightBat
        {
            get { return rightBat; }
            set { rightBat = value; }
        }

        int maxScore = 5;
        int leftScore = 0;
        int rightScore = 0;

        public void ResetGame()
        {
            ball.Reset();
        }

        public void LeftBatScores()
        {
            winSound.Play();
            leftScore++;
            if (leftScore == maxScore)
            {
                gameState = GAMESTATE.END;
                MediaPlayer.Play(startSong);
            }
            else
            {
                ResetGame();
            }
        }

        public void RightBatScores()
        {
            winSound.Play();
            rightScore++;
            if (rightScore == maxScore)
            {
                gameState = GAMESTATE.END;
                MediaPlayer.Play(startSong);
            }
            else
            {
                ResetGame();
            }
        }

        SpriteFont spriteFont;
        
        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SoundEffect.MasterVolume = 1.0f;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            screenBounds = GraphicsDevice.Viewport.Bounds;

            textColour = Color.Cornsilk;
            highlightColour = Color.Red;

            base.Initialize();

            gameState = GAMESTATE.START;
            graphics.ToggleFullScreen();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameBackground = Content.Load<Texture2D>("Misty");
            startBackground = Content.Load<Texture2D>("splash");
            endBackground = Content.Load<Texture2D>("endgame");

            // the left bat            
            leftBat = new Bat(Keys.Q, Keys.A);
            leftBat.LoadContent();
            Vector2 leftStartPos = new Vector2(10, (screenBounds.Height / 2) - leftBat.Sprite.Height / 2);
            leftBat.Position = leftStartPos;
            children.Add(leftBat);

            
            rightBat = new Bat(Keys.P, Keys.L);
            rightBat.LoadContent();
            Vector2 rightStartPos = new Vector2((screenBounds.Width - rightBat.Sprite.Width) - 10, (screenBounds.Height / 2) - rightBat.Sprite.Height / 2);
            rightBat.Position = rightStartPos;
            children.Add(rightBat);


            ball = new Ball();
            ball.LoadContent();
            children.Add(ball);

            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            cursor = Content.Load<Texture2D>("Cursor");

            for (int i = 0; i < 10; i++)
            {
                barks[i] = Content.Load<SoundEffect>("Bark" + i);
            }

            winSound = Content.Load<SoundEffect>("3Barks");

            startSong = Content.Load<Song>("droidmarch");
            MediaPlayer.Play(startSong);

            // TODO: use this.Content to load your game content here
        }

        public void Bark()
        {
            int which = (int) (new Random().NextDouble() * 9);

			SoundEffectInstance soundEngineInstance = barks[which].CreateInstance();
			soundEngineInstance.Volume = 1f;
			soundEngineInstance.Play();
        }

        public void ResetMouse()
        {
            int midX = ScreenBounds.Width / 2;
            int midY = ScreenBounds.Height / 2;
            Mouse.SetPosition(midX, midY);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
        protected override void Update (GameTime gameTime)
		{
			KeyboardState keyState = Keyboard.GetState ();
			MouseState mouseState = Mouse.GetState ();
			Vector2 touchPos = new Vector2 (mouseState.X, mouseState.Y);
            bool touched = false;
#if __MonoCS__
			TouchCollection touchCollection = TouchPanel.GetState ();
			
			if (touchCollection.Count > 0) {
				touchPos.X = touchCollection [0].Position.X;
				touchPos.Y = touchCollection [0].Position.Y;
				touched = true;
			}
#endif

			if (mouseState.LeftButton == ButtonState.Pressed) {
				touched = true;
			}

            switch (gameState)
            {
                case GAMESTATE.START:
                {
                    int startAt = 200;
                    int border = 20;
                    highlighted = -1;
                    for (int i = 0; i < menuOptions.Count(); i++)
                    {
                        Vector2 textSize = spriteFont.MeasureString(menuOptions[0]);
                        Vector2 pos = new Vector2(50, startAt + ((textSize.Y + border) * i));
                        Rectangle textBounds = new Rectangle((int)pos.X, (int)pos.Y, (int)textSize.X, (int) textSize.Y);
                        Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);
                        if ((touchPos.X > textBounds.X) && (touchPos.X < textBounds.X + textBounds.Width)
                            && (touchPos.Y > textBounds.Y) && (touchPos.Y < textBounds.Y + textBounds.Height))
                        {
                            highlighted = i;
                        }
                    }

                    if (touched)
                    {
                        if (highlighted == 0)
                        {
                            leftScore = 0;
                            rightScore = 0;
                            rightBat.AiControlled = true;
                            leftBat.MouseControlled = true;
                            gameState = GAMESTATE.MIDDLE;
                            MediaPlayer.Stop();
                            ResetMouse();
                        }
                        if (highlighted == 1)
                        {
                            gameState = GAMESTATE.MIDDLE;
                            MediaPlayer.Stop();
                            ResetMouse();
                        }
                        if (highlighted == 2)
                        {
                            this.Exit();
                        }
                    }

                    break;
                }
                case GAMESTATE.MIDDLE:
                {             
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].Update(gameTime);
                    }
                    break;
                }
                case GAMESTATE.END:
                {
                    if (touched)
                    {
                        gameState = GAMESTATE.START;
                    }
                    break;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            switch (gameState)
            {
                case GAMESTATE.START:
                {
					float scale = (float) screenBounds.Width / (float)  startBackground.Bounds.Width;
					spriteBatch.Draw(startBackground, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(scale, scale), SpriteEffects.None, 0);
                    Vector2 textSize = spriteFont.MeasureString("Hello");
                    int startAt = 200;
                    int border = 20;
                    CentreText("Misty Bats!!!", 5, Color.Black);
                    for (int i = 0; i < menuOptions.Count(); i++)
                    {
                        Vector2 pos = new Vector2(50, startAt + ((textSize.Y + border) * i));
                        if (i == highlighted)
                        {
                            spriteBatch.DrawString(spriteFont, menuOptions[i], pos, highlightColour);
                        }
                        else
                        {
                            spriteBatch.DrawString(spriteFont, menuOptions[i], pos, textColour);
                        }

                        spriteBatch.Draw(cursor, new Vector2(mouseState.X, mouseState.Y), Color.White);
                    }
                    break;
                }
                case GAMESTATE.MIDDLE:
                {
                    spriteBatch.Draw(gameBackground, screenBounds, gameBackground.Bounds, Color.White);
                    for (int i = 0; i < children.Count; i++)
                    {
                        children[i].Draw(gameTime);
                    }
                    int midX = screenBounds.Width / 2;
                    spriteBatch.DrawString(spriteFont, "" + leftScore, new Vector2(midX - 100, 20), textColour);
                    spriteBatch.DrawString(spriteFont, "" + rightScore, new Vector2(midX + 100, 20), textColour);
                    
                    break;
                }
                case GAMESTATE.END:
                {
                    spriteBatch.Draw(endBackground, screenBounds, endBackground.Bounds, Color.White);

                    CentreText("Game Over!", 150, Color.Yellow);
                    CentreText("Score: " + leftScore + " to " + rightScore, 250, Color.Yellow);
                    string winner = "Misty loves the " + ((leftScore > rightScore) ? "left" : "right") + " bat";
                    CentreText(winner, 350, Color.Yellow);
                    spriteBatch.Draw(cursor, new Vector2(mouseState.X, mouseState.Y), Color.Yellow);
                    break;
                }
            }
            spriteBatch.End();            
            base.Draw(gameTime);
        }

        public void CentreText(string text, float y, Color color)
        {
            Vector2 textSize = spriteFont.MeasureString(text);
            int midX = screenBounds.Width / 2;
            spriteBatch.DrawString(spriteFont, text, new Vector2(midX - (textSize.X / 2), y), color);
        }
    }
}
