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
    public class LeftBat:GameEntity
    {
        float speed;

        public LeftBat()
        {
            Position.X = 10;
            Position.Y = 10;
            speed = 500.0f;
        }

        public override void LoadContent()
        {
            Sprite = Game1.Instance.Content.Load<Texture2D>("Bone");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            KeyboardState state = Keyboard.GetState();
            float timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if ((Position.Y > 10) && (state.IsKeyDown(Keys.Q)))
            {
                Position.Y -= timeDelta * speed;
            }
            float bottom = (Game1.Instance.ScreenBounds.Height - Sprite.Height) - 10;
            if ((Position.Y < bottom) && (state.IsKeyDown(Keys.A)))
            {
                Position.Y += timeDelta * speed;
            }
            
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.SpriteBatch.Draw(Sprite, Position, Color.White);
        }
    }
}
