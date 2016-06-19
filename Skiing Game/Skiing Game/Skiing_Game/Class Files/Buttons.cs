using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SkiingGameCoursework
{
    public class Buttons : Sprite
    {
        Texture2D texture;
        Rectangle ButtonRec;

        /// <summary>
        /// CONTRUCTOR 
        /// </summary>
        /// <param name="ButtonTexture"> TEXTURE </param>
        /// <param name="Position"> VECTOR POSITION </param>
        /// <param name="Width"> WIDTH OF THE BUTTON</param>
        /// <param name="Height"> HEIGHT OF THE BUTTON</param>
        public Buttons(Texture2D ButtonTexture, Vector2 Position, int Width, int Height)
        {
            texture = ButtonTexture;
            ButtonRec = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public override void Draw(SpriteBatch spritebatch)
        {

            //base.Draw(spritebatch);
        }

        public override void Update(Skiing game, GameTime gametime)
        {
            base.Update(game, gametime);
        }
    }
    public class Start : Buttons
    {
        Texture2D texture;
        Rectangle ButtonRec;
        Color ButtonColor = Color.White; //USED TO MAKE IT SHOW THE USER THAT THEY'RE HOVERING

        /// <summary>
        /// CONTRUCTOR
        /// </summary>
        /// <param name="tex"> BUTTON TEXTURE</param>
        /// <param name="pos"> POSITION OF THE BUTTON </param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public Start(Texture2D tex, Vector2 pos, int Width, int Height)
            : base(tex, pos, Width, Height)
        {
            texture = tex;
            ButtonRec = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);

            Pause = false;
            Active = true;
            Skiing.menu.AddSprite(this);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, ButtonRec, null, ButtonColor, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1.0f);
        }

        public override void Update(Skiing game, GameTime gametime)
        {
            if (MouseClass.click())
            {
                if (MouseClass.click(ButtonRec)) //WHEN THE USER CLIKCS THE BUTTON
                {
                    Skiing.menu.Active = false;

                    Skiing.mainGame.Active = true;
                    Skiing.mainGame.Pause = true;
                    Skiing.mainGame.RunOnce = false; //INITILISE VARIABLES
                    Skiing.mainGame.Initialize();
                    Skiing.mainGame.Reset();

                    Skiing.userPrompts.Active = true;
                }
            }
            else
            {
                if (MouseClass.hover()) //IF THE USER IS HOVERING OVER THE BUTTON
                {
                    if (MouseClass.hover(ButtonRec))
                    {
                        ButtonColor = Color.Red;
                    }
                    else
                    {
                        ButtonColor = Color.White;
                    }
                }
            }



            base.Update(game, gametime);
        }
    }
}
