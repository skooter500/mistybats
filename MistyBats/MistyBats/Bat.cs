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
        Boolean aiControlled;
        Boolean mouseControlled;

        public Boolean MouseControlled
        {
            get { return mouseControlled; }
            set { mouseControlled = value; }
        }

        public Boolean AiControlled
        {
            get { return aiControlled; }
            set { aiControlled = value; }
        }
        Keys upKey;
        Keys downKey;

        float speed = 500.0f;

        public Bat(Keys upKey, Keys downKey)
        {
            this.upKey = upKey;
            this.downKey = downKey;
         
        }

        public override void LoadContent()
        {
            Sprite = Game1.Instance.Content.Load<Texture2D>("bat");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            

            float bottom = (Game1.Instance.ScreenBounds.Height - Sprite.Height) - 10;
            if (aiControlled)
            {
                Velocity.Y = Game1.Instance.Ball.Position.Y - Position.Y;
                Velocity.Y *= 50;
            }
            else if (mouseControlled)
            {
                Velocity.Y = mouseState.Y - (Game1.Instance.ScreenBounds.Height / 2);
                Velocity.Y *= 50;
                Game1.Instance.ResetMouse();
            }
            else
            {
                Velocity.Y = 0;
                if ((Position.Y > 10) && (keyState.IsKeyDown(upKey)))
                {
                    Velocity.Y = -speed;
                }

                if ((Position.Y < bottom) && (keyState.IsKeyDown(downKey)))
                {
                    Velocity.Y = speed;
                }
            }

            Position += Velocity * timeDelta;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.SpriteBatch.Draw(Sprite, Position, Game1.Instance.TextColour);
        }
    }
}
