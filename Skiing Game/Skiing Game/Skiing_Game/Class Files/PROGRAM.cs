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

//------------------THINGS TO IMPROVE ON NEXT GAME----------------------------
//
// - USE DELTA TIME & VELOCITY FOR ANY MOVEMENT OF ANY SPRITES, THIS MEANS THE UPDATE CAN
//   UNLIMITED AND NOT MAKE THE GAME EXECUTE SUPER FAST
//
//----------------------------------------------------------------------------

namespace SkiingGameCoursework
{
    public class Skiing : Game
    {
        #region TEXTURES
        SoundEffect BackgroundMusic;

        //SNOWBALL
        public Texture2D snowballSpriteSheet;

        public Texture2D spaceBar;
        #endregion

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Skiing()
        {
            Window.Title = "Skiing"; // CHANGES THE WINDOW TITLE

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //DIFFERENT WINDOWS
        public static MainGame mainGame;
        public static UserPrompt userPrompts;
        public static Menu menu;

        /// <summary>
        /// IS RUN ONCE AT THE START AND INITIALIZES THE PROGRAM
        /// </summary>
        protected override void Initialize()
        {
            //IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;
            //graphics.ApplyChanges();

            IsMouseVisible = true; //MAKES THE MOUSE VIEWABLE TO THE USER

            //DEFINES THE GAME WINDOWS
            mainGame = new MainGame();
            userPrompts = new UserPrompt();
            menu = new Menu();

            base.Initialize();
            Mouse.WindowHandle = Window.Handle; //MAKES THE POSITION CACULATIONS FROM THE ACTIVE WINDOW
            
        }

        /// <summary>
        /// METHOD USED TO LOAD IN CONTENT
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SkiingGameCoursework.Content.TVBanner = Content.Load<Texture2D>(@"Banner\TV_Banner");
            SkiingGameCoursework.Content.Background = Content.Load<Texture2D>(@"Background\SnowField");
            SkiingGameCoursework.Content.StartButton = Content.Load<Texture2D>(@"Buttons\StartButton");
            SkiingGameCoursework.Content.EndButton = Content.Load<Texture2D>(@"Buttons\EndButton");
            SkiingGameCoursework.Content.HighScore = Content.Load<Texture2D>(@"Buttons\HighScoresButton");
            SkiingGameCoursework.Content.cheese = Content.Load<Texture2D>(@"Assets\cheese");

            //FLAGS
            SkiingGameCoursework.Content.leftFlag = Content.Load<Texture2D>(@"Assets\leftBlueFlag");
            SkiingGameCoursework.Content.rightFlag = Content.Load<Texture2D>(@"Assets\rightRedFlag");

            //SKIER
            SkiingGameCoursework.Content.Skier = Content.Load<Texture2D>(@"Assets\skier");
            SkiingGameCoursework.Content.leftskier = Content.Load<Texture2D>(@"Assets\leftSkier");
            SkiingGameCoursework.Content.rightskier = Content.Load<Texture2D>(@"Assets\rightSkier");
            SkiingGameCoursework.Content.downskier = Content.Load<Texture2D>(@"Assets\downSkier");
            SkiingGameCoursework.Content.upskier = Content.Load<Texture2D>(@"Assets\upSkier");
            SkiingGameCoursework.Content.crashSkier = Content.Load<Texture2D>(@"Assets\crashSpriteSheet");

            //SNOWBALL
            snowballSpriteSheet = Content.Load<Texture2D>(@"Assets\SnowBallFrames");

            //KEYS
            spaceBar = Content.Load<Texture2D>(@"Assets\spaceBar");
            SkiingGameCoursework.Content.nKey = Content.Load<Texture2D>(@"Assets\nKey");
            SkiingGameCoursework.Content.qKey = Content.Load<Texture2D>(@"Assets\qKey");

            //TEXT
            SkiingGameCoursework.Content.PlayerName = Content.Load<SpriteFont>(@"Banner\playerName");
            SkiingGameCoursework.Content.Score = Content.Load<SpriteFont>(@"Fonts\score");
            SkiingGameCoursework.Content.saveAndLoad = Content.Load<SpriteFont>(@"Fonts\saveAndLoad");
            SkiingGameCoursework.Content.smallText = Content.Load<SpriteFont>(@"Fonts\smallFont");

            //SOUNDS
            BackgroundMusic = Content.Load<SoundEffect>(@"Sounds\BackgroundMusic");
            SkiingGameCoursework.Content.CheesePing = Content.Load<SoundEffect>(@"Sounds\CheesePing");
            SkiingGameCoursework.Content.SnowballHit = Content.Load<SoundEffect>(@"Sounds\SnowballHit16");

            SkiingGameCoursework.Content.snow = Content.Load<Texture2D>(@"Assets\SnowTexture");
            SkiingGameCoursework.Content.SnowTrail = Content.Load<Texture2D>(@"Assets\SnowTrail");

            //==============================================
            //PlayBackgroundMusic(); //THANKS TO ANGUS NEWMAN
            //==============================================

            StartUp();

        } //LOADS ALL THE CONTENT

        /// <summary>
        /// METHOD THAT IS USED TO UPDATE THE GAME
        /// IT'S CALLED AS MANY TIMES AS POSSIBLE
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            FPS.totalFrames++;

            //MAIN GAME
            if (mainGame.Active == true)
            {
                mainGame.Update(gameTime);
                mainGame.UpdateNoPause(gameTime);
            }

            //MENU
            if (menu.Active == true)
            {
                menu.Update(gameTime);
            }

            //USER PROMPTS
            userPrompts.Update(gameTime);

            FPS.Update(gameTime);

            base.Update(gameTime);

        }

        /// <summary>
        /// THIS METHOD DRAWS THE ACTIVE SPRITES
        /// IT'S CALLED 60 TIMES A SECCOND
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            

            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null);

            //ALL THE DRAW LOGIC
            if (menu.Active == true)
            {
                menu.Draw(spriteBatch);
            }

            if (mainGame.Active == true)
            {
                mainGame.Draw(spriteBatch);
            }
            userPrompts.Draw(spriteBatch);

            FPS.Draw(spriteBatch, GraphicsDevice);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected static Skier skier;
        protected static Background background;
        public static RestartPrompt Res;
        public static Start startButton;
        /// <summary>
        /// THIS METHOD CREATES AND INITALIZES ALL THE SPRITES, MUSIC, ECT
        /// </summary>
        private void StartUp()
        {
            Banner banner;
            Flag flag;
            Lives lives;
            SpaceKey space;
            HighestScore HScore;
            Snowball snow;

            //BACKGROUND
            background = new Background(SkiingGameCoursework.Content.Background, new Vector2(0, 0), SkiingGameCoursework.Content.Background.Width, SkiingGameCoursework.Content.Background.Height, 0.0f);
            background.Initialize(GraphicsDevice);

            skier = new Skier(SkiingGameCoursework.Content.Skier, new Vector2(350, 170), 29, 63, 0.2f);
            flag = new Flag();
            lives = new Lives();
            space = new SpaceKey(spaceBar, new Vector2(150, 300), 500, 80);
            Res = new RestartPrompt();
            Cheese cheese = new Cheese();
            snow = new Snowball(snowballSpriteSheet, new Vector2(310, -210), 65, 65);
            banner = new Banner(SkiingGameCoursework.Content.TVBanner, new Vector2(50, 400), 1252, 42, 0.82f);
            Blur blur = new Blur(GraphicsDevice);


            //MENU
            startButton = new Start(SkiingGameCoursework.Content.StartButton, new Vector2(280, 60), SkiingGameCoursework.Content.StartButton.Width, SkiingGameCoursework.Content.StartButton.Height);
            HScore = new HighestScore();

            mainGame.Active = false;
            userPrompts.Active = false;
            menu.Active = true;

            mainGame.LoadParticle();
            menu.LoadParticle();

            mainGame.Initialize();

            PlayBackgroundMusic();
        } //CREATES ALL THE INSTANCES OF ALL THE ASSETS 

        SoundEffectInstance Instance;
        /// <summary>
        /// PLAYS THE MUSIC IN THE BACKGROUND
        /// </summary>
        private void PlayBackgroundMusic()
        {
            //CREATES AN INSTANCE, LOOPS AND PLAYS THE SOUND
            Instance = BackgroundMusic.CreateInstance();
            Instance.IsLooped = true;

            Instance.Play();
        }
    }

    public class Content
    {
        public static Texture2D Skier;
        public static Texture2D TVBanner;
        public static Texture2D Background;

        //SKIER
        public static Texture2D leftskier;
        public static Texture2D rightskier;
        public static Texture2D downskier;
        public static Texture2D upskier;
        public static Texture2D crashSkier;

        //FLAGS
        public static Texture2D leftFlag;
        public static Texture2D rightFlag;

        //CHESSE
        public static Texture2D cheese;

        //USER BUTTON PROMPTS
        public static Texture2D nKey;
        public static Texture2D qKey;

        //BUTTONS
        public static Texture2D StartButton;
        public static Texture2D HighScore;
        public static Texture2D EndButton;

        //SPRITE FONTS
        public static SpriteFont Score;
        public static SpriteFont PlayerName;
        public static SpriteFont saveAndLoad;
        public static SpriteFont smallText;

        public static SoundEffect CheesePing;
        public static SoundEffect SnowballHit;
        public static SoundEffect Wind;

        public static Texture2D snow;
        public static Texture2D SnowTrail;
        //
    }

    public class GameRelated
    {
        public static bool GameOver = false;

        private static int speedUpInterval = 10;
        public static void speedUp()
        {
            if (Function.time.current > speedUpInterval)
            {
                if (Background.BackgroundSpeed < 10)
                {
                    Background.BackgroundSpeed += 1;
                    speedUpInterval += 10;
                }
            }
        }

        static float ElaspedTime;
        /// <summary>
        /// LOGIC USED TO SPEED UP THE GAME WHEN THE USER PRESSES THE DOWN KEY
        /// </summary>
        /// <param name="gametime"> TOTAL GAMETIME</param>
        public static void SpeedUpInstant(GameTime gametime)
        {
            float delay = 600f;
            ElaspedTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (ElaspedTime > delay)
            {
                if (Background.BackgroundSpeed < 10) //MAKES SURE YOU CANNOT GET TOO FAST
                {
                    Background.BackgroundSpeed += 1;
                }
                ElaspedTime = 0; //RESET
            }
        } //USED WHEN THE PLAYER IS SPEEDING UP
        /// <summary>
        /// THE AUTOMATED PROCESS OF UPDATING THE SPEEDING UP THE GAME
        /// </summary>
        /// <param name="gametime"> TOTAL SPEED </param>
        public static void SlowDown(GameTime gametime)
        {
            float delay = 600f;
            ElaspedTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (ElaspedTime > delay)
            {
                if (Background.BackgroundSpeed > 5) //MAKES SURE YOU CANNOT STOP
                {
                    Background.BackgroundSpeed -= 1;
                }
                ElaspedTime = 0;
            }
        } //USED WHEN THE PLAYER IS SLOWING DONW
    }
}

