using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkiingGameCoursework
{
    public static class keyboard
    {
        public static bool left()
        {
            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.Left))
            {
                return true;
            }
            return false;
        }
        public static bool right()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Right))
            {
                return true;
            }
            return false;
        }
        public static bool up()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Up))
            {
                return true;
            }
            return false;
        }
        public static bool down()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Down))
            {
                return true;
            }
            return false;
        }
        public static bool space()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Space))
            {
                return true;
            }

            return false;
        }
        public static bool n()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.N))
            {
                return true;
            }
            return false;
        }
        public static bool q()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Q))
            { 
                return true;
            }

            return false;
        }
        public static bool s()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.S))
            {
                return true;
            }

            return false;
        }
        public static bool r()
        {
            KeyboardState key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.R))
            {
                return true;
            }

            return false;
        }
    }
}
