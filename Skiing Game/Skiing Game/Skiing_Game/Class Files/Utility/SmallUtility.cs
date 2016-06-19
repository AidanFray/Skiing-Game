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

namespace Function
{
    public static class copy
    {
        public static void list(List<Rectangle> copy, List<Rectangle> target)
        {
            foreach (Rectangle rec in copy)
            {
                target.Add(rec);
            }
        }
    }
    public class time
    {
        public static float current = 0.00f;
        public static float elasped = 0.00f;

        public static void update(GameTime gametime)
        {
            current += (float)gametime.ElapsedGameTime.TotalSeconds;

            current = (float)Math.Round(current, 2);
        }  //USED TO UPDATE THE TOTAL TIME

        public static void reset()
        {
            current = 0.00f;
        }
    }
    public class math
    {
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
