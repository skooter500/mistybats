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
    public abstract class GameEntity
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Sprite;


        public virtual void LoadContent() {}        
        public virtual void Update(GameTime gameTime) {}
        public virtual void Draw(GameTime gameTime) { }

        public Rectangle MyRectangle()
        {
            Rectangle rect = Sprite.Bounds;
            rect.X = (int) Position.X;
            rect.Y = (int) Position.Y;

            return rect;
        }
    }
}
