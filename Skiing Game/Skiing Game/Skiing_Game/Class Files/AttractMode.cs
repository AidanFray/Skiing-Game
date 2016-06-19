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
    public class MenuSkier : Skier
    {
        public void Update(GameTime gametime)
        {
            //LOGIC RELATING TO THE PROGRAM MOVING THE PLAYER AROUND
            //MoveType == 0 - STRAIGHT
            //MoveType == 1 - LEFT
            //MoveType == 2 - RIGHT

            if (Menu.MoveType == 1)
            {
                MoveLeft();
            }
            if (Menu.MoveType == 2)
            {
                MoveRight();
            }

            if (Menu.MoveType == 0)
            {
                MoveCentre();
            }


        }
    }
    public class MenuBackground : Background
    {
        public void Update(GameTime gametime)
        {

        }
    }
    public class MenuFlags
    {
        public void Update(GameTime gametime)
        {

        }
    }
    public class Blur : Sprite
    {
        GraphicsDevice graphicsD;

        /// <summary>
        /// DRAWS A RECTANGLE OVER THE SCREEN
        /// THE ALPHA IS LOWERED TO MAKE IT TRANSPARENT
        /// </summary>
        /// <param name="graphics"></param>
        public Blur(GraphicsDevice graphics)
        {
            Active = true;
            Skiing.menu.AddSprite(this);

            graphicsD = graphics;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            var Rectangle = new Texture2D(graphicsD, 1, 1);
            Rectangle.SetData(new[] { Color.Azure });

            spritebatch.Draw(Rectangle, new Rectangle(0, 0, graphicsD.Viewport.Width, graphicsD.Viewport.Height),null, Color.Gray * 0.6f, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.8f);

            // base.Draw(spritebatch);
        }
    }
}
