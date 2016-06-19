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
    public class GameField : Skiing
    {
        List<Sprite> activeSprites = new List<Sprite>(); //THE SPRITES TO BE DRAWN

        public GameField()
        {

        }

        //DETERMINS IF THE FIELD IS ACTIVE
        private bool isActive = true;
        public bool Active
        {
            get { return isActive; }
            set { isActive = value; }
        }

        //USED TO MAKE SURE IF THE ASSETS IS UPDATED OR NOT
        private bool pause = false;
        public bool Pause
        {
            get { return pause; }
            set { pause = value; }
        }

        public bool RunOnce = false; //FOR THE START
        public new virtual void Initialize()
        {
            //INITIALIZATION LOGIC FOR ALL THE SPRIES
            if (RunOnce == false && Active == true)
            {
                foreach (Sprite sprite in activeSprites)
                {
                    sprite.Initialize();
                }
                RunOnce = true;
            }
            //
        }

        /// <summary>
        /// DRAWS ALL THE SPRITES IN THE FIELD
        /// </summary>
        /// <param name="spritebatch"></param>
        public virtual void Draw(SpriteBatch spritebatch)
        {
            //DRAW LOGIC
            //IF THE SPRITE IS ACTIVE THE PROGRAM WILL DRAW IT
            if (isActive == true)
            {
                foreach (Sprite sprite in activeSprites)
                {
                    if (sprite.Active == true)
                    {
                        sprite.Draw(spritebatch);
                    }
                }
            }
            //
        }

        /// <summary>
        /// UPDATES THE GAME FIELD
        /// </summary>
        /// <param name="gameTime"></param>
        public new virtual void Update(GameTime gameTime)
        {
            //UPDATE LOGIC
            //IF THE SPITE IS UNPAUSED AND ACTIVE THE PROGRAM WILL UPDATE
            if (isActive == true && Pause == false)
            {
                foreach (Sprite sprite in activeSprites)
                {
                    if (sprite.Active == true)
                    {
                        sprite.Update(this, gameTime);
                    }
                }
            }
        }
        public virtual void UpdateNoPause(GameTime gameTime)
        {
            //THIS UPDATES ALL THE SPRITES THAT DON'T NEED TO BE PAUSED

            if (IsActive == true)
            {
                foreach (Sprite sprite in activeSprites)
                {
                    sprite.UpdateNoPause(this, gameTime);
                }
            }
        }

        /// <summary>
        /// RESETS THE GAMEFIELD
        /// </summary>
        public virtual void Reset()
        {
            foreach (Sprite sprite in activeSprites)
            {
                sprite.Reset(this);
            }
        }

        /// <summary>
        /// LOADING AND SAVING THE DATA
        /// </summary>
        public List<object> SaveData = new List<object>();
        public virtual void Save()
        {
            //RUNS THE SAVE LOGIC
            foreach (Sprite sprite in activeSprites)
            {
                sprite.Save(); //THE DATA IS PUT INTO THE SAVEDATA LIST
            }

            string SaveLocation = @"mostRecentGame.txt";
            StreamWriter Save = new StreamWriter(SaveLocation);
            //WRITE TO FILE
            foreach (object x in SaveData)
            {
                Save.WriteLine(x);
            }

            Save.Close();
            SaveData.Clear();

        }
        public List<object> LoadData = new List<object>();
        public virtual void Load()
        {
            string saveLocation = @"mostRecentGame.txt";
            StreamReader Read = new StreamReader(saveLocation);

            //READS THE ENTRIE FILE
            while (Read.EndOfStream == false)
            {
                LoadData.Add(Read.ReadLine());
            }

            foreach (Sprite sprite in activeSprites)
            {
                sprite.Load(LoadData);
            }
            Read.Close();

            LoadData.Clear();
        }

        /// <summary>
        /// ADDING AND REMOVING THE SPRITES
        /// </summary>
        /// <param name="sprite"> THE SPRITE BEING INPUT </param>
        public void AddSprite(Sprite sprite)
        {
            //USED TO CREATE A NEW SPRITE
            activeSprites.Add(sprite);
        }
        public void DeleteSprite(Sprite sprite)
        {
            //HOW TO DELETE A SPRITE
            activeSprites.Remove(sprite);
        }
    }
    public class MainGame : GameField
    {
        ParticleSystem SnowShower;
        ParticleSystem SnowTrailL;
        ParticleSystem SnowTrailR;

        private void ResetParticles()
        {
            SnowTrailL.Reset();
            SnowTrailR.Reset();
        }

        public void LoadParticle()
        {
            //PARTICLE SYSTEM
            ParticleSystem Snow = new Snow(SkiingGameCoursework.Content.snow, new Vector2(0, 0), 800, 0, 0.1f, 5f, 200);
            SnowShower = Snow;

            //SNOW TRAILS
            //NOTE: LIFE TIME IS IN SECCCONDS
            ParticleSystem SnowTrailLeft = new SnowTrail(SkiingGameCoursework.Content.SnowTrail, new Vector2(363, 220), 0.001f, 5f, 1000); 
            SnowTrailL = SnowTrailLeft;
            ParticleSystem SnowTrailRight = new SnowTrail(SkiingGameCoursework.Content.SnowTrail, new Vector2(380, 220), 0.001f, 5f, 1000);
            SnowTrailR = SnowTrailRight;
        }

        //DRAW LOGIC
        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            SnowTrailL.Draw(spritebatch);
            SnowTrailR.Draw(spritebatch);
            SnowShower.Draw(spritebatch);

        }

        //UPDATE LOGIC
        public override void Update(GameTime gametime)
        {
            SnowShower.Update(gametime);

            if (GameRelated.GameOver == false)
            {
                //START THE GAME
                if (keyboard.space())
                {
                    Pause = false;
                }
            }
            
            if (Pause == false)
            {
                UserMessages message; //USED TO TELL THE USER WHEN THEY HAVE SAVED OR LOADED

                SnowTrailL.Update(gametime);
                SnowTrailR.Update(gametime);

                //SAVING
                if (keyboard.s())
                {
                    mainGame.Save();
                    mainGame.Reset();
                    mainGame.Pause = true;
                    message = new UserMessages(true);
                }

                //LOADING
                if (keyboard.r())
                {
                    mainGame.Reset();
                    mainGame.Load();
                    mainGame.Pause = true;
                    message = new UserMessages(false);
                }

                Function.time.update(gametime);
                GameRelated.speedUp(); //SPEEDS UP THE GAME OVER TIME
            }

            //NEW GAME
            if (keyboard.n())
            {
                userPrompts.Active = true;
                Pause = true;

                //RESET
                Reset();
                Res.Reset(this);
                
                ResetParticles();
                
            }

            //QUIT GAME
            if (keyboard.q())
            {
                Pause = true;
                mainGame.Active = false;
                menu.Active = true;

                //RESET
                Reset();
                Res.Reset(this);

                ResetParticles();

                userPrompts.Active = false;
            }


            base.Update(gametime);
        }
    }
    public class UserPrompt : GameField
    {
        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
    public class Menu : GameField
    {
        ParticleSystem SnowSh;

        public void LoadParticle()
        {
            //PARTICLE SYSTEM
            ParticleSystem Snow = new Snow(SkiingGameCoursework.Content.snow, new Vector2(0, 0), 800, 0, 0.1f, 5f, 200);

            SnowSh = Snow;
        }

        //DRAW LOGIC
        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            SnowSh.Draw(spritebatch);
        }

        //VARIABLES USED FOR THE ATTRACT MODE
        public static int MoveType; //1 == Left //2 == Right
        public bool startDone = false;
        public bool Moving = false;
        public override void Update(GameTime gameTime)
        {
            SnowSh.Update(gameTime);

            UpdateTime(gameTime);

            Background.BackgroundSpeed = 2;

            //START SECTION
            if (ElaspedTime > 3000f || startDone == true)
            {
                if (ElaspedTime > 100f)
                {
                    startDone = true; //SKIPS THE REPEAT
                    CheckPosition(); // UPDATES THE MOVEMENT
                    ElaspedTime = 0;
                }

                if (Moving == false)
                {
                    MoveType = 0; //MAKES IT GO FORWARD
                }
            }


            base.Update(gameTime);
        }

        public float ElaspedTime;
        public float Delay = 1000f;
        public void UpdateTime(GameTime gametime)
        {
            //USED TO CHECK HOW MANY SECCONDS HAVE ELAPSEED
            if (Active == true)
            {
                ElaspedTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                ElaspedTime = 0;
            }
        }

        /// <summary>
        /// THIS IS USED TO TELL THE PROGRAM WHERE THE SKIER NEEDS TO MOVE TOWARDS
        /// </summary>
        public void CheckPosition()
        {
            int LeftSide = Flag.ActiveFlags[14].X;
            int RightSide = Flag.ActiveFlags[15].X;
            int MiddlePoint = (RightSide + LeftSide) / 2; //AVERAGE / 2= MIDDLE POINT

            //THE POSITION THE SKIER NEEDS TO BE AT
            int SkierMiddle = (Skier.SkierRec.X + (Skier.SkierRec.X + Skier.SkierRec.Width)) / 2; //THE MIDDLE OF THE SKIER

            bool WorthMoving = false; //CHECKS IF THE SKIER IS FAR ENOUGH AWAY TO MAKE IT USEFUL TO MOVE

            int RangeCheck = MiddlePoint - SkierMiddle; //CHECKS THE DIFFERENCE
            if (RangeCheck < 0)//IF NEGATIVE
            {
                RangeCheck = RangeCheck * -1; //MAKES IT A POSITIVE VALUE IF NEGATIVE
            }

            //IF THE SKIER IS WITHIN THE RANGE
            if ((RangeCheck) < 30) //IN RANGE
            {
                Moving = false;
                WorthMoving = false;
                MoveType = 0;
            }
            else
            {
                WorthMoving = true; //IF THE SKIER IS OUT THE RANGE
            }

            if (WorthMoving == true) //IF THE COMPUTER HAS DECIDED THE PLAYER NEEDS TO MOVE
            {
                if (MiddlePoint > SkierMiddle) //MOVING RIGHT
                {
                    Moving = true;
                    MoveType = 2;
                }
                else
                {
                    if (MiddlePoint < SkierMiddle) //MOVING LEFT
                    {
                        Moving = true;
                        MoveType = 1;
                    }
                    else
                    {
                        MoveType = 0;
                    }
                }
            }
        }
    }
}
