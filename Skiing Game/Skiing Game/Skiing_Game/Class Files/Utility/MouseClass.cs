using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkiingGameCoursework
{
    public static class MouseClass
    {
        public static ButtonState lastMouseState;
        public static Boolean click()
        {
            MouseState mouseState = Mouse.GetState(); //GRABS THE CURRENT STATE OF THE MOUSE

            //IF CHECKS FOR THE VERY FIRST CLICK
            if (mouseState.LeftButton == ButtonState.Pressed &&
                    lastMouseState == ButtonState.Released)
            {
                lastMouseState = Mouse.GetState().LeftButton; //SAVES THE STATE BEFORE EXITNG [1a]
                return true;
            }

            lastMouseState = Mouse.GetState().LeftButton; //[2a]
            return false;
        }
        public static Boolean click(Rectangle buttonRec)
        {
            //GRABS THE MOUSE POSIION
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            //WIDTH
            if (mouseX >= buttonRec.X &&
                mouseX <= buttonRec.X + buttonRec.Width)
            {
                //HEIGHT
                if (mouseY >= buttonRec.Y &&
                    mouseY <= buttonRec.Y + buttonRec.Height)
                {
                    return true;
                }
            }
            return false;
        }
        public static Boolean hover()
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Released &&
                    lastMouseState == ButtonState.Released)
            {
                lastMouseState = Mouse.GetState().LeftButton;
                return true;
            }

            lastMouseState = Mouse.GetState().LeftButton;

            return false;
        }
        public static Boolean hover(Rectangle obj)
        {
            //GRABS THE MOUSE POSIION
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            //WIDTH
            if (mouseX >= obj.X &&
                mouseX <= obj.X + obj.Width)
            {
                //HEIGHT
                if (mouseY >= obj.Y &&
                    mouseY <= obj.Y + obj.Height)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
