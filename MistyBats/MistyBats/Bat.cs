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
    public class Bat:GameEntity
    {
        Keys upKey;
        Keys downKey;

        float speed = 500.0f;

        public Bat(Keys upKey, Keys downKey, Vector2 startPos)
        {
            Position = startPos;
            this.upKey = upKey;
            this.downKey = downKey;
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

            Velocity.Y = 0;
            if ((Position.Y > 10) && (state.IsKeyDown(upKey)))
            {
                Velocity.Y = -speed;
            }
            float bottom = (Game1.Instance.ScreenBounds.Height - Sprite.Height) - 10;
            if ((Position.Y < bottom) && (state.IsKeyDown(downKey)))
            {
                Velocity.Y = speed;
            }

            Position += Velocity * timeDelta;
            
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.SpriteBatch.Draw(Sprite, Position, Color.White);
        }
    }
}
