using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MistyBats
{
    public class Ball:GameEntity
    {
        float maxVelocity = 1000;
        public void Reset()
        {
            float speed = 200.0f;
            Random r = new Random(DateTime.Now.Millisecond);

            
            Velocity.Y = (float)r.NextDouble() - 0.5f;
            Velocity.X = r.Next() % 2 == 0 ? -1 : 1;
            Velocity *= speed;

            Rectangle screenBounds = Game1.Instance.ScreenBounds;
            Position = new Vector2(screenBounds.Width / 2, screenBounds.Height / 2);
        }

        public Ball()
        {
            Reset();
        }

        public override void LoadContent()
        {
            Sprite = Game1.Instance.Content.Load<Texture2D>("ball");
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            

            // Now check to see if we hit the top or the bottom
            if (Position.Y < 0)
            {
                Position.Y = 0;
                Velocity.Y = -Velocity.Y;
                Game1.Instance.Bark();
            }

            if (Position.Y > Game1.Instance.ScreenBounds.Height- Sprite.Height)
            {
                Position.Y = Game1.Instance.ScreenBounds.Height - Sprite.Height;
                Velocity.Y = -Velocity.Y;
                Game1.Instance.Bark();
            }

            if (Position.X < 0)
            {
                Game1.Instance.RightBatScores();
            }

            if (Position.X > Game1.Instance.ScreenBounds.Width - Sprite.Width)
            {
                Game1.Instance.LeftBatScores();
            }
            
            if (MyRectangle().Intersects(Game1.Instance.LeftBat.MyRectangle()))
            {
                Position.X = Game1.Instance.LeftBat.MyRectangle().X + Game1.Instance.LeftBat.MyRectangle().Width;
                Velocity.X = -Velocity.X;
                Velocity += Game1.Instance.LeftBat.Velocity;
                Game1.Instance.Bark();
            }

            if (MyRectangle().Intersects(Game1.Instance.RightBat.MyRectangle()))
            {
                Position.X = Game1.Instance.RightBat.MyRectangle().X - MyRectangle().Width;
                Velocity.X = -Velocity.X;
                Velocity += Game1.Instance.RightBat.Velocity;
                Game1.Instance.Bark();
            }

            if (Velocity.Y > maxVelocity)
            {
                Velocity.Y = maxVelocity;
            }
            if (Velocity.Y < -maxVelocity)
            {
                Velocity.Y = -maxVelocity;
            }
            Position += Velocity * timeDelta;

        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.SpriteBatch.Draw(Sprite, Position, Game1.Instance.TextColour);
        }
    }
}
