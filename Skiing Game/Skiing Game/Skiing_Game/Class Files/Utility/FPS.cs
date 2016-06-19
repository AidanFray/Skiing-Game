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
using System.IO;
namespace SkiingGameCoursework
{
    public static class FPS
    {
        public static int totalFrames = 0;
        public static float ElaspedTime = 0.0f;
        public static int fps = 0;

        public static void Update(GameTime gameTime)
        {
            ElaspedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (ElaspedTime > 1000.0f)
            {
                fps = totalFrames;
                totalFrames = 0;
                ElaspedTime = 0;
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice Graphics)
        {
            spriteBatch.DrawString(Content.smallText, //FONT TYPE
                                    string.Format("FPS {0}", fps), //TEXT
                                    new Vector2(0, Graphics.Viewport.Height - 20), //POSITION
                                    Color.Black, //COLOUR
                                    0.0f, //ROTATION
                                    new Vector2(0, 0), //ORIGIN
                                    new Vector2(1, 1), //SCALE
                                    SpriteEffects.None, //EFFECTS
                                    1f); //LAYER DEPTH
        }
    }
}
