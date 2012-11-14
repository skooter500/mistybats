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
        public Texture2D Sprite;

        public virtual void LoadContent() {}        
        public virtual void Update(GameTime gameTime) {}
        public virtual void Draw(GameTime gameTime) { }
    }
}
