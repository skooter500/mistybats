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

namespace MistyBats
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GAMESTATE
        {
            START, MIDDLE, END
        }

        GAMESTATE state;

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

        SoundEffect[] barks = new SoundEffect[10];

        Texture2D background;

        Ball ball;

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
            leftScore++;
            ResetGame();
        }

        public void RightBatScores()
        {
            rightScore++;
            ResetGame();
        }

        SpriteFont spriteFont;
        
        public Game1()
        {
            Instance = this;
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
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            screenBounds = GraphicsDevice.Viewport.Bounds;

            base.Initialize();

            //graphics.ToggleFullScreen();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Misty");

            // the left bat
            Vector2 leftStartPos = new Vector2(10, (screenBounds.Height / 2) - 52);
            leftBat = new Bat(Keys.Q, Keys.A, leftStartPos);
            leftBat.LoadContent();
            children.Add(leftBat);

            Vector2 rightStartPos = new Vector2(screenBounds.Width - 57, (screenBounds.Height / 2) - 52);
            rightBat = new Bat(Keys.P, Keys.L, rightStartPos);
            rightBat.LoadContent();
            children.Add(rightBat);


            ball = new Ball();
            ball.LoadContent();
            children.Add(ball);

            spriteFont = Content.Load<SpriteFont>("SpriteFont1");

            for (int i = 0; i < 10; i++)
            {
                barks[i] = Content.Load<SoundEffect>("Bark" + i);
            }
            // TODO: use this.Content to load your game content here
        }

        public void Bark()
        {
            int which = (int) (new Random().NextDouble() * 9);
            barks[which].Play();
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
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }


            for (int i = 0; i < children.Count; i++)
            {
                children[i].Update(gameTime);
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            
            float scale = (float) screenBounds.Height / (float) background.Bounds.Height;
            //spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0, Vector2.Zero, scale , SpriteEffects.None, 0);
            spriteBatch.Draw(background, screenBounds, background.Bounds, Color.White);
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Draw(gameTime);
            }


            int midX = screenBounds.Width / 2;
            spriteBatch.DrawString(spriteFont, "" + leftScore, new Vector2(midX - 100, 20), Color.Red);
            spriteBatch.DrawString(spriteFont, "" + rightScore, new Vector2(midX + 100, 20), Color.Red);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
