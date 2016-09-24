using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace com.dreamwagon.axe
{
    public interface Drawable
    {
         void LoadContent( ContentManager content );
         void Update(GameTime gameTime);
         void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
