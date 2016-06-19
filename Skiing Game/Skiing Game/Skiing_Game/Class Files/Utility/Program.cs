using System;

namespace SkiingGameCoursework
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Skiing game = new Skiing())
            {
                game.Run();
            }
        }
    }
#endif
}

