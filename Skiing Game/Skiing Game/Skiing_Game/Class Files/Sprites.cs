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
    //MAIN GAME
    public class Sprite
    {
        public Sprite(Texture2D texture, Vector2 position, int spriteWidth, int spriteHeight, float layerDepth)
        {
            SpriteTexture = texture;
            SpritePosition = position;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
            LayerDepth = layerDepth;

            spriteRec = new Rectangle((int)position.X, (int)position.Y, SpriteWidth, SpriteHeight);
        }
        public Sprite()
        {

        }

        //TEXTURE OF THE SPRITE
        Texture2D SpriteTexture;
        float LayerDepth;
        
        /// <summary>
        /// CHANGES THE PICTURE THAT IS TO BE DRAWN
        /// </summary>
        /// <param name="texture"> THE TEXTURE THATS USED WHEN DRAWING </param>
        public void changeSpriteTexture(Texture2D texture)
        {
            SpriteTexture = texture;
        }

        //POSITION OF THE SPRITE
        Vector2 SpritePosition;
        /// <summary>
        /// CHANGES THE VECTOR THAT IS USED WHEN DRAWING THE SKIER
        /// </summary>
        /// <param name="vec"></param>
        public void changeSpritePosition(Vector2 vec)
        {
            SpritePosition.X = vec.X;
            SpritePosition.Y = vec.Y;
        }

        //SPRITE DIMENTIONS
        int SpriteHeight;
        int SpriteWidth;
        /// <summary>
        /// CHANGES THE SIZE OF THE SPRITE
        /// </summary>
        /// <param name="w"> WIDTH INT </param>
        /// <param name="h"> HEIGHT INT </param>
        public void changeSpriteSize(int w, int h)
        {
            SpriteHeight = h;
            SpriteWidth = w;
        }

        //ACTIVE BOOL
        bool isActive = false;
        public bool Active
        {
            get { return isActive; }
            set { isActive = value; }
        }

        bool isPaused = false;
        public bool Pause
        {
            get { return isPaused; }
            set { isPaused = value; }
        }

        public Rectangle spriteRec;

        public virtual void Initialize()
        {

        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spriteRec = new Rectangle((int)SpritePosition.X, (int)SpritePosition.Y, SpriteWidth, SpriteHeight); //RE DEFINES THE SPRITE

            spritebatch.Draw(SpriteTexture, spriteRec, null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, LayerDepth);
        }

        public virtual void Update(Skiing game, GameTime gametime)
        {

        } //UPDATE LOGIC 
        public virtual void UpdateNoPause(Skiing game, GameTime gametime)
        {

        } //RUNS LOGIC EVEN IF THE SPRITE IS PAUSED

        public virtual void Save()
        {

        }

        public virtual void Load(List<object> Data)
        {

        }

        public virtual void Reset(Skiing game)
        {

        }
    }

    //SPRITES
    public class Skier : Sprite
    {
        //SKIER PROPERTY
        public static Rectangle SkierRec;
        public static Texture2D SkierTexture = Content.Skier;

        /// <summary>
        /// CONTUCTOR
        /// </summary>
        /// <param name="tex"> TEXTURE </param>
        /// <param name="pos"> POSITION (VECTOR) </param>
        /// <param name="width"> SPRITE WIDTH </param>
        /// <param name="height"> SPRITE HEIGHT </param>
        public Skier(Texture2D tex, Vector2 pos, int width, int height, float layerDepth)
        {
            SkierRec = new Rectangle((int)pos.X, (int)pos.Y, width, height);

            Active = true;

            Skiing.mainGame.AddSprite(this);
            Skiing.menu.AddSprite(this);
        }
        public Skier()
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (Crash == false)
            {
                spritebatch.Draw(SkierTexture, SkierRec, null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.1f); //DRAWS THE SPRITE
            }
            else
            {
                spritebatch.Draw(Content.crashSkier, SkierRec, TakeImage, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.1f); //DRAWS THE CRASH ANIMATION
            }

            //base.Draw(spritebatch);
        }

        public override void Update(Skiing game, GameTime gametime)
        {
            if (!(Skiing.menu.Active == true)) //IF THE MENU IS NOT ACTIVE
            {
                if (Pause == false)
                {
                    if (!(keyboard.left() && keyboard.right()))
                    {
                        if (keyboard.left()) //IF THE PLAYER PRESSES THE LEFT KEY
                        {
                            MoveLeft();
                        }
                        else
                        {
                            if (keyboard.right()) //IF THE PLAYER PRESSES THE RIGHT KEY
                            {
                                MoveRight();
                            }
                            else
                            {
                                if (!(keyboard.up() && keyboard.down())) //CAN'T PRESS BOTH AT THE SAME TIME
                                {
                                    if (keyboard.down()) //THE DOWN SKIER LOGIC (SPEEDS UP)
                                    {
                                        SkierTexture = Content.downskier;
                                        SkierRec.Width = 29;
                                        SkierRec.Height = 63;
                                        GameRelated.SpeedUpInstant(gametime);
                                    }
                                    else
                                    {
                                        if (keyboard.up()) //THE UP SKIER LOGIC (SLOWS DOWN)
                                        {
                                            SkierTexture = Content.upskier;
                                            SkierRec.Width = 60;
                                            SkierRec.Height = 63;
                                            GameRelated.SlowDown(gametime);
                                        }
                                        else
                                        {
                                            MoveCentre(); //RETURNS TO THE DEFAULT SPRITE
                                        }
                                    }
                                }
                                else
                                {
                                    MoveCentre(); //RETURNS TO THE DEFAULT SPRITE
                                }
                            }
                        }
                    }
                    else
                    {
                        MoveCentre(); //RETURNS TO THE DEFAULT SPRITE
                    }
                }

            }
            else
            {
                MenuSkier MSkier = new MenuSkier();

                MSkier.Update(gametime);
            }
            base.Update(game, gametime);
        }

        public override void UpdateNoPause(Skiing game, GameTime gametime)
        {
            //STUFF THAT WILL RUN EVEN IF THE SKIER IS PAUSED
            if (Flash == true)
            {
                BlinkingSprite(gametime);
            }
            if (Crash == true)
            {
                AnimateCrash(gametime);
            }

            base.UpdateNoPause(game, gametime);
        }

        public override void Reset(Skiing game)
        {
            SkierTexture = Content.Skier;
            SkierRec.Width = 29;
            SkierRec.Height = 63;

            SkierRec.X = 350;
            SkierRec.Y = 170;

            Flash = false;
            Active = true;
            Crash = false;

            GameRelated.GameOver = false;

            CrashFrame = 0;

            base.Reset(game);
        }

        //MOVES THE PLAYER LEFT
        public void MoveLeft()
        {
            SkierTexture = Content.leftskier;
            SkierRec.Width = 39;
            SkierRec.Height = 63;
        }

        //MOVES THE PLAYER RIGHT
        public void MoveRight()
        {
            SkierTexture = Content.rightskier;
            SkierRec.Width = 39;
            SkierRec.Height = 63;
        }

        //PUTS THE PLAYER BACK TO DEFAULT SPRITE
        public void MoveCentre()
        {
            SkierTexture = Content.Skier; //RETURNING TO NORMAL
            SkierRec.Width = 29;
            SkierRec.Height = 63;
        }

        //FLASHING LOGIC
        public static bool Flash = false;
        public float elaspedTimeBlink;
        public int flashNumber;
        /// <summary>
        /// MAKES THE SPRITE FLASH
        /// </summary>
        /// <param name="gametime"> TOTAL ELASPED TIME OF THE CURRENT PROGRAM </param>
        public void BlinkingSprite(GameTime gametime)
        {
            float blinkDelay = 200f;
            elaspedTimeBlink += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (elaspedTimeBlink >= blinkDelay) //MAKES SURE THE BLINKS ARE VISIBLE AND DOESN'T BLINK FOREVER
            {
                //IF THE SKIER IS VISIBLE
                if (Active == true)
                {
                    Active = false;
                }
                else
                {
                    Active = true;
                }

                flashNumber++;
                elaspedTimeBlink = 0;

                if (flashNumber == 11)
                {
                    Active = true;
                    Flash = false;
                    flashNumber = 0;
                }


            }
        }

        //CRASHING LOGIC
        public static bool Crash = false;
        public int CrashFrame = 0;
        public float ElaspedTime;
        public float Delay = 250f;
        Rectangle TakeImage;
        /// <summary>
        /// THE METHOD THAT ANIMATES THE CRASH
        /// </summary>
        /// <param name="gametime"> TOTAL ELASPED TIME OF THE CURRENT PROGRAM </param>
        public void AnimateCrash(GameTime gametime)
        {
            Rectangle crashskier = new Rectangle(350, 170, (261 / 3) - 15, 200 / 3);

            //TAKES THE TIME
            ElaspedTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            //CHANGES THE FRAME
            if (ElaspedTime >= Delay && Crash == true)
            {
                if (!(CrashFrame == 1))
                {
                    CrashFrame++;
                }
                if (Crash == true)
                {
                    ElaspedTime = 0;
                }
            }
            //
            Rectangle takeImage = new Rectangle(261 * CrashFrame, 0, 261, 200);
            crashskier.Y += 60 * CrashFrame;
            crashskier.X += 20 * CrashFrame;

            SkierRec = crashskier;
            TakeImage = takeImage;
        }
    }
    public class Snowball : Sprite
    {
        Texture2D SnowBallImage;
        Rectangle SnowBallRec;
        Rectangle TakeImage;
        public int Speed = Background.BackgroundSpeed + 2;

        /// <summary>
        /// CONTRUCTOR FOR THE SNOWBALL SPRITE
        /// </summary>
        /// <param name="Tex"> SPRITE TEXTURE </param>
        /// <param name="Pos"> POSITION OF THE SPRITE </param>
        /// <param name="W"> WIDTH </param>
        /// <param name="H"> HEIGHT </param>
        public Snowball(Texture2D Tex, Vector2 Pos, int W, int H)
        {
            SnowBallImage = Tex;
            SnowBallRec = new Rectangle((int)Pos.X, (int)Pos.Y, W, H);

            Active = true;
            Skiing.mainGame.AddSprite(this);
        }

        public override void Draw(SpriteBatch spritebatch)
        {

            //DRAWS THE ANIMATION
            if (Visible == true)
            {
                Rectangle TakeImage = new Rectangle(0, (100 * Frame), 100, 100);
                spritebatch.Draw(SnowBallImage, SnowBallRec, TakeImage, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.3f);
            }

            //base.Draw(spritebatch);
        }

        public float ElaspedTime = 0;
        public int Frame = 0;
        public override void Update(Skiing game, GameTime gametime)
        {
            if (Visible == true)
            {
                if (!(keyboard.left() && keyboard.right()))
                {
                    if (keyboard.left())
                    {
                        MoveLeft(); //MOVES THE SNOWBALL LEFT
                    }

                    if (keyboard.right())
                    {
                        MoveRight(); //MOVES THE SNOWBALL RIGHT 
                    }
                }

                Colide(); //LOGIC RUN IF THE PLAYER HITS THE SNOWBALL
            }
            else
            {
                Generate();
            }

            CheckPosition();

            base.Update(game, gametime);
        }

        public override void UpdateNoPause(Skiing game, GameTime gametime)
        {
            if (Visible == true)
            {
                MoveDown(); //ALWAYS MOVES DOWN

                UpdateAnimation(gametime); //ANIMATION DOESN'T PAUSE
            }

            base.UpdateNoPause(game, gametime);
        }

        public override void Reset(Skiing game)
        {

        }

        /// <summary>
        /// LOGIC RELATED TO CHANGING THE FRAME OF THE SNOWBALL ANIMATION
        /// </summary>
        /// <param name="gametime"> TOTAL TIME OF THE GAME </param>
        public void UpdateAnimation(GameTime gametime)
        {
            float delay = 50f;
            ElaspedTime += (float)gametime.ElapsedGameTime.Milliseconds;

            //LOOPS ROUND THE FRAME
            if (ElaspedTime > delay)
            {
                if (Frame == 19)
                {
                    Frame = 0;
                }
                else
                {
                    Frame++;
                }
                ElaspedTime = 0;
            }
            //

        }

        public bool Visible = false;
        /// <summary>
        /// LOGIC RELATING TO CREATING A SNOWBALL
        /// </summary>
        public void Generate()
        {
            Random rand = new Random();

            Visible = false;

            if (rand.Next(1800) == 3) //AN ODD OF 1/1800
            {
                Visible = true;
            }
        }

        /// <summary>
        /// IF THE SNOWBALL GOES DOWN TOO LOW IT DELTES IT AND ALLOWS THE GENERATION TO OCCUR
        /// </summary>
        public void CheckPosition()
        {
            if (SnowBallRec.Y > 600)
            {
                Visible = false;
                SnowBallRec.Y = -210;
                Hit = false;
            }
        }

        public bool Hit = false;
        /// <summary>
        /// LOGIC RELATING TO THE PLAYER HITTING THE SNOWBALL
        /// </summary>
        public void Colide()
        {
            if (Hit == false)
            {
                if (SnowBallRec.Intersects(Skier.SkierRec)) //IF THE SNOW BALL HITS ANY OF THE PLAYER REC'
                {
                    Hit = true;

                    if (Lives.Count == 1)
                    {
                        Lives.PlayerDie();
                        Content.SnowballHit.Play(); //PLAYS THE COLLISION SOUND
                    }
                    else
                    {
                        Lives.Count -= 1;
                        Skier.Flash = true;
                        Content.SnowballHit.Play(); //PLAYS THE COLLISION SOUND
                    }

                }
            }
        }

        /// <summary>
        /// LOGIC REALTING TO MOVING THE SNOWBALL
        /// </summary>
        public void MoveDown()
        {
            SnowBallRec.Y += Speed;
        }
        public void MoveLeft()
        {
            SnowBallRec.X += Background.SideSpeed;
        }
        public void MoveRight()
        {
            SnowBallRec.X -= Background.SideSpeed;
        }

    }
    public class Cheese : Sprite
    {
        //DATA USED FOR DRAWING THE CHEESE
        public static Rectangle CheeseRec;
        public static bool Visible = false;

        public Cheese()
        {
            CheeseRec = new Rectangle(Flag.Centre, 600, Content.cheese.Width / 6, Content.cheese.Height / 6);

            Active = true;
            Skiing.mainGame.AddSprite(this);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (Visible == true)
            {
                spritebatch.Draw(Content.cheese, CheeseRec, null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.4f);
            }

            //base.Draw(spritebatch);
        }

        public float ElaspedTime = 0;
        public int Frame = 0;
        public override void Update(Skiing game, GameTime gametime)
        {
            if (Visible == true)
            {
                CheeseRec.X = Flag.Centre;

                MoveDown(); //MOVES THE CHEESE DOWN 

                if (!(keyboard.left() && keyboard.right()))
                {
                    if (keyboard.left())
                    {
                        MoveLeft(); //MOVES THE CHEESE LEFT 
                    }

                    if (keyboard.right())
                    {
                        MoveRight(); //MOVES THE CHEESE RIGHT
                    }
                }

                Colide();
            }
            else
            {
                Generate();
            }

            CheckPosition();

            base.Update(game, gametime);
        }

        public override void Reset(Skiing game)
        {
            Visible = false;
            CheeseRec.Y = 610;
        }

        /// <summary>
        /// USED FOR GENERATING THE CHEESE
        /// </summary>
        public void Generate()
        {
            Random rand = new Random();

            Visible = false;
            if (rand.Next(1500) == 5) //ODDS OF 1/1500
            {
                Visible = true;
            }
        }

        /// <summary>
        /// IF THE POSITION OF CHEESE IS OFF THE SCREEN
        /// </summary>
        public void CheckPosition()
        {
            if (CheeseRec.Y < 0)
            {
                Visible = false;
                CheeseRec.Y = 610;
            }
        }

        public bool Hit = false;
        /// <summary>
        /// LOGIC RELATING TO THE PLAYER COLIDING WITH CHEESE
        /// A LIFE IS ADDED
        /// </summary>
        public void Colide()
        {
            if (CheeseRec.Intersects(Skier.SkierRec)) //IF THE SNOW BALL HITS ANY OF THE PLAYER REC'
            {
                Lives.Count++;
                Visible = false;
                CheeseRec.Y = 600;
                Content.CheesePing.Play();
            }
        }

        /// <summary>
        /// MOVES THE CHEESE AROUND
        /// </summary>
        public void MoveDown()
        {
            CheeseRec.Y -= Background.BackgroundSpeed;
        }
        public void MoveLeft()
        {
            CheeseRec.X += Background.SideSpeed;
        }
        public void MoveRight()
        {
            CheeseRec.X -= Background.SideSpeed;
        }
    }
    public class Flag : Sprite
    {
        public static int Centre = 350;
        public static List<Rectangle> ActiveFlags = new List<Rectangle>(); //THE LIST OF FLAGS TO DRAW

        //CONTRUCTOR
        public Flag()
        {
            Active = true;

            Skiing.mainGame.AddSprite(this);
            Skiing.menu.AddSprite(this);

            PathCreate();
        }

        //GAME LOGIC
        public override void Initialize()
        {
            base.Initialize();
        }

        private int DrawCount;
        public override void Draw(SpriteBatch spritebatch)
        {
            bool leftFlagCheck = true; //USED TO ALTERNATE BETWEEN LEFT AND RIGHT FLAGS
            foreach (Rectangle x in ActiveFlags)
            {
                if (DrawCount < 34)
                {
                    if (leftFlagCheck == true) //IF THE NEXT FLAG THAT NEEDS TO DRAWN IS A LEFT FLAG
                    {
                        spritebatch.Draw(Content.leftFlag, x, null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.2f);
                        DrawCount++;

                        flagType.Add(true); //A LEFT FLAG (TRUE)

                        leftFlagCheck = false;
                    }
                    else //IF THE NEXT FLAG IS THE RIGHT FLAG
                    {

                        spritebatch.Draw(Content.rightFlag, x, null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.2f);
                        DrawCount++;

                        flagType.Add(false); //A RIGHT FLAG (FALSE)

                        leftFlagCheck = true;
                    }
                }
            }
            DrawCount = 0;
            //base.Draw(spritebatch); 
        }
        public override void Update(Skiing game, GameTime gametime)
        {
            if (!(Skiing.menu.Active == true)) //THE LOGIC USED FOR THE MAIN GAME
            {
                if (Pause == false)
                {
                    PathCreate();
                    MoveDown();
                    DeleteOfScreen();
                    Colide(gametime);

                    if (!(keyboard.right() && keyboard.left()))
                    {
                        if (keyboard.right())
                        {
                            MoveRight();
                        }
                        else
                        {
                            if (keyboard.left())
                            {
                                MoveLeft();
                            }
                        }
                    }

                    base.Update(game, gametime);
                }
            }
            else //LOGIC FOR THE MENU 
            {
                MoveDown(); //PLAYS THE MOVEMENT
                PathCreate();

                DeleteOfScreen(); //REMOVES DEAD FLAGS

                Score.ScoreCurrrent = 0; //STOPS THE FLAGS FROM PROVIDING TO A SCORE

                //ADDS BEHAVIOUR FOR THE AI
                if (Menu.MoveType == 1)
                {
                    MoveLeft();
                }

                if (Menu.MoveType == 2)
                {
                    MoveRight();
                }
                //

            }
        }
        public override void Reset(Skiing game)
        {
            ActiveFlags.Clear();
            MovingFlags.Clear();

            nextRow = 200;
            flagsMade = 0;
            cycle = 0;
            isStraight = false;
            sharpness = 100;
            angleChange = 0;
            angle = 0;
            flagGap = 300;

            PathCreate();

            base.Reset(game);
        }
        public override void Save()
        {
            Skiing.mainGame.SaveData.Add(ActiveFlags.Count()); //THE NUMBER FLAGS BEING SAVED

            foreach (Rectangle x in ActiveFlags) //SAVES ALL THE FLAGS
            {
                Skiing.mainGame.SaveData.Add(x);
            }

            //base.Save();
        }
        public override void Load(List<object> Data)
        {
            int NumberOfFlag = int.Parse(Data[1].ToString()); //GRABS THE STRING THAT TELLS THE PROGRAM HOW MANY FLAGS HAVE BEEN SAVED

            //GRABS ALL THE FLAGS
            ActiveFlags.Clear();
            for (int i = 2; i < NumberOfFlag; i++)
            {
                ActiveFlags.Add(stringToRectangle(Data[i].ToString())); //ADDS THE FLAGS TO THE ACTIVE LIST
            }

            base.Load(Data);
        }

        /// <summary>
        /// THIS METHOD GRABS THE X AND Y VARIBALES FROM A SAVED RECTANGLE STRING
        /// </summary>
        /// <param name="str"></param> IS THE RECTANGLE STRING THAT IS BEING LOADED IN
        /// <returns></returns>
        private static Rectangle stringToRectangle(string str)
        {
            string xStr = "";
            string yStr = "";
            const int width = 25;
            const int height = 33;

            for (int p = 0; p < str.Length; p++)
            {
                if (str[p] == 'X') //LOOKS FOR THE X CHAR 
                {
                    for (int i = (p + 2); i < str.Length; i++)
                    {
                        if (!(str[i] == ' '))  //IT THEN TAKES ALL THE CHARS UP UNTIL IT ENDS
                        {
                            xStr = xStr + str[i]; //SAVES THE X VALUE
                        }
                        else
                        {
                            break; //IF IT FINDS AND EMPTY CHAR, IT ENDS THE LOOP
                        }
                    }
                }

                if (str[p] == 'Y') //Y VAL
                {
                    for (int i = (p + 2); i < str.Length; i++)
                    {
                        if (!(str[i] == ' '))
                        {
                            yStr = yStr + str[i]; //SAVES THE Y VALUE
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return new Rectangle(int.Parse(xStr), int.Parse(yStr), width, height);
        }

        public List<Rectangle> ActiveFlagToDraw = new List<Rectangle>();
        public List<Rectangle> MovingFlags = new List<Rectangle>();
        public Rectangle leftFlagRec;
        public Rectangle rightFlagRec;
        public List<bool> flagType = new List<bool>();

        //PATTERN GENERATION VARIABLES
        public int nextRow = 200; //LOOP VARIBALE
        public int cycle = 0; //HOLD HOW MANY FULL SINE WAVES ARE GENERATED

        //VARIBLES THAT ARE USED IN INCREASING DIFFICULTY
        public int sharpness = 100;
        public int angleChange = 0;
        public double angle = 0;
        public int flagGap = 300;


        /// <summary>
        /// THE STATIC GENERATION OF THE FLAG COURSE
        /// </summary>
        public void PathCreate()
        {
            int leftFlagX;
            int rightFlagX;
            for (int x = 0; nextRow <= 49990; x++)
            {
                int loopCount = 0;

                //WORKS OUT THE POSITION OF THE NEXT FLAG BY USING A SINE WAVE
                leftFlagX = (int)(Math.Sin(Function.math.DegreeToRadian(angle)) * sharpness) + 200;
                rightFlagX = ((int)(Math.Sin(Function.math.DegreeToRadian(angle)) * sharpness)) + flagGap + 200;
                angle += angleChange; //CHANGES THE ANGLE

                //CREATES THE FLAG RECS
                leftFlagRec = new Rectangle(leftFlagX, nextRow, Content.leftFlag.Width / 6, Content.leftFlag.Height / 6);
                rightFlagRec = new Rectangle(rightFlagX, nextRow, Content.rightFlag.Width / 6, Content.rightFlag.Height / 6);

                //ADDS THEM TO THE ACTIVE LIST TO BE DRAW
                ActiveFlags.Add(leftFlagRec);
                ActiveFlags.Add(rightFlagRec);

                //LOGIC RELATING TO HELPING THE PROGRAM NOT GET STUCK IN A GENERATING A STRAIGHT SECTION
                if (isStraight == true)
                {
                    flagsMade += 2;
                }

                //ADDS ON ANOTHER CYCLE
                if (angle > 360)
                {
                    cycle++;
                    angle = 0;
                    loopCount++;
                }

                //MAKES THE PATTERN
                Pattern();


                if (loopCount > 10)
                {
                    ReduceGap();
                    loopCount = 0;
                }


                //MOVES DOWN TO THE NEXT ROW
                nextRow += 30; //GAP BETWEEN FLAGS
            }

            //REPEATS THE PATTERN
            if (ActiveFlags.Count == 16)
            {
                nextRow = 200;
                cycle = 3;
            }
        }
        public void Pattern()
        {
            if (cycle == 0) //PATTERN 1
            {
                MakeStraightSection();
            }
            if (cycle == 1) //CHECK FOR STRAIGHT
            {
                if (flagsMade == 10) //10 ROWS
                {
                    isStraight = false;
                    flagsMade = 0;
                    MakeSmoothSection();
                }
            }

            if (cycle == 3) //PATTERN 2
            {
                MakeSharpSection();
            }

            if (cycle == 5) //PATTERN 3
            {
                MakeSharpSection();

            }

            if (cycle == 7) //PATTERN 4
            {
                MakeSmoothSection();
            }

            if (cycle == 9) //PATTERN 5
            {
                MakeSharpSection();
            }

            if (cycle == 40) //6
            {
                MakeSmoothSection();

            }

            if (cycle == 60) //7
            {
                MakeVerySharpSection();
            }

            if (cycle == 100) //8
            {
                cycle = 3;
            }
        }

        /// <summary>
        /// THE METHODS USED IN MAKING DIFFERENT SECTIONS OF THE COURSE
        /// THE ONLY VARIABLE CHANGED IS THE 'angleChange' THAT DETERMINS THE ANGLE INPUT IN THE SINE WAVE GENERATION
        /// </summary>
        public void MakeStraightSection()
        {
            cycle++;
            angle = 0;
            angleChange = 0;
            isStraight = true;
            flagsMade = 0;
        }
        public void MakeSmoothSection()
        {
            angleChange = 15;
        }
        public int flagsMade = 0; //VAR USED TO GET OUT OF A STARIGHT SECTION
        public bool isStraight = false; //USED TO TELL THE PROGRAM IF IT'S MAKING A STRAIGHT SECTION
        public void MakeSharpSection()
        {
            angleChange = 25;
        }
        public void MakeVerySharpSection()
        {
            angleChange = 30;
        }

        public void ReduceGap() //USES THE GAMETIME TO CHECK HOW MUCH TIME HAS PASSED
        {
            if (flagGap > 250)
            {
                flagGap -= 3; //MAKES THE GAP SMALLER
            }
        }

        /// <summary>
        /// REMOVES THE FLAGS OF THE SCREEN
        /// </summary>
        public void DeleteOfScreen()
        {
            List<Rectangle> toDelete = new List<Rectangle>(); //TEMP LIST
            foreach (Rectangle flag in ActiveFlags)
            {
                if (flag.Y < -30)
                {
                    toDelete.Add(flag); //SAVES IT TO DELETE LATER
                }
                else
                {
                    break; //STOP UNESSISARY LOOPING
                }
            }

            //RUNS ROUND THE LIST AND DELETES THE FLAGS THAT ARE NEEDED TO BE DELETED
            foreach (Rectangle flag in toDelete)
            {
                ActiveFlags.Remove(flag);
                Score.ScoreCurrrent += 1; //EVERY ROW PASSED ADDS TO THE SCORE
            }
            toDelete.Clear(); //DELETES THE DATA FROM THE TEMP LIST
        }

        /// <summary>
        /// THE LOGIC INVOLVED IN CHECKING AND DEALING WITH THE PLAYER TOUCHING FLAGS 
        /// </summary>
        public void Colide(GameTime gametime)
        {
            Rectangle SkierRec = Skier.SkierRec;
            int index = 0;
            int SaveIndex = 0;
            bool movePlayer = false;

            foreach (Rectangle flag in ActiveFlags)
            {
                //HOLD THE SIZE OF THE SKIER
                int skierHeight = SkierRec.Height;
                int skierWidth = SkierRec.Width;

                if (SkierRec.Intersects(flag)) //ONLY CHECKS IF THE PLAYER AND FLAG ARE CLOSE
                {
                    if (SkierRec.Y + skierHeight > flag.Y + flag.Height && SkierRec.Y < flag.Y + 5)
                    {
                        for (int X = SkierRec.X; X <= SkierRec.X + skierWidth; X++) //THIS FOR NEXT LOOP CHECKS THE ENTIRE FRONT OF THE PLAYER
                        {
                            if (X > flag.X && X < flag.X + 5)
                            {
                                if (Lives.JustCrashed == false)
                                {
                                    if (Lives.Count > 1) //WHEN IT'S NOT THE LAST LIFE
                                    {
                                        //REMOVES A LIFE
                                        Lives.Count -= 1;

                                        //SLOWS DOWN THE GAME
                                        Background.BackgroundSpeed = 2;
                                        Background.SideSpeed = 3;

                                        //MAKES SURE THIS LOGIC DOESN'T TRIGGER MULTIPLE TIMES IN A ROW
                                        Lives.JustCrashed = true;

                                        //GRABS THE POSISION OF THE FLAG HIT
                                        SaveIndex = index;

                                        //MOVES THE FLAGS TO MAKE IT SEEM THAT PLAYER HAS MOVES
                                        movePlayer = true;

                                        //MAKES THE SKIER SPRITE FLASH
                                        Skier.Flash = true;

                                        //REMOVES ANY CHEESE 
                                        Cheese.CheeseRec.Y = 610;
                                        Cheese.Visible = false;
                                        break;
                                    }
                                    else //LAST LIFE
                                    {
                                        Lives.PlayerDie();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                index++;
            }
            if (movePlayer == true)
            {
                MovePlayerToCentre(SaveIndex);
                movePlayer = false;
                SaveIndex = 0;
                Lives.JustCrashed = false;
            }

        }

        /// <summary>
        /// THESE METHODS ARE USED TO MOVE THE FLAGS IN DIFFERENT DIRECTIONS
        /// </summary>
        private float DeltaTime; 
        public void MoveDown()
        {
            //ALTERS ALL THE FLAGS RECTANGLES DEPENDING ON HOW FAST THE BACKGROUND IS MOVING
            foreach (Rectangle rec in ActiveFlags)
            {
                Rectangle newRec = new Rectangle(rec.X, rec.Y - Background.BackgroundSpeed, rec.Width, rec.Height);
                MovingFlags.Add(newRec);
            }

            //CLEARS THE ACTIVE FLAGS
            ActiveFlags.Clear();

            //COPIES THE CONTENT OF THE MOVED DOWN FLAGS
            Function.copy.list(copy: MovingFlags, target: ActiveFlags);

            MovingFlags.Clear();
        }
        public void MoveLeft()
        {
            //ALTERS THE FLAGS TO MOVE THEM LEFT I.E ADDING THE BACKGROUND SPEED TO THEIR X VALUE
            foreach (Rectangle rec in ActiveFlags)
            {
                Rectangle newRec = new Rectangle(rec.X + Background.SideSpeed, rec.Y, rec.Width, rec.Height);

                MovingFlags.Add(newRec);
            }
            Centre += Background.SideSpeed;

            //COPIES THE CONTENT OF THE ALTERED LIST TO THE ACTIVE LIST
            ActiveFlags.Clear();
            Function.copy.list(copy: MovingFlags, target: ActiveFlags);
            MovingFlags.Clear();
        }
        public void MoveRight()
        {
            //ALTERS THE FLAGS TO MOVE THEM RIGHT
            foreach (Rectangle rec in ActiveFlags)
            {
                Rectangle newRec = new Rectangle(rec.X - Background.SideSpeed, rec.Y, rec.Width, rec.Height);
                MovingFlags.Add(newRec); //SAVES TO A TEMP LIST
            }
            Centre -= Background.SideSpeed;

            //COPIES THE ALTERED LIST TO THE ACTIVE FLAGS LIST
            ActiveFlags.Clear();
            Function.copy.list(copy: MovingFlags, target: ActiveFlags);
            MovingFlags.Clear();
        }

        public List<Rectangle> changeFlags = new List<Rectangle>(); //USED AS A BUFFER LIST
        public void MovePlayerToCentre(int index)
        {
            changeFlags.Clear();

            if (flagType[index] == true) //LEFT
            {
                //MOVES THE FLAG CLUMP TO THE RIGHT
                foreach (Rectangle x in ActiveFlags)
                {
                    Rectangle newRec;
                    int xVal = x.X;
                    newRec = new Rectangle((xVal -= (flagGap / 2)), x.Y, x.Width, x.Height);

                    changeFlags.Add(newRec);
                }
                ActiveFlags.Clear();

                foreach (Rectangle y in changeFlags)
                {
                    ActiveFlags.Add(y);
                }
                changeFlags.Clear();

            }
            else //RIGHT
            {
                foreach (Rectangle x in ActiveFlags)
                {
                    Rectangle newRec;
                    int xVal = x.X;
                    newRec = new Rectangle((xVal += (flagGap / 2)), x.Y, x.Width, x.Height);

                    changeFlags.Add(newRec);
                }
                ActiveFlags.Clear();

                foreach (Rectangle y in changeFlags)
                {
                    ActiveFlags.Add(y);
                }
                changeFlags.Clear();
            }

            Flag.Centre = 350;

        } //USED TO MOVE THE FLAGS TO MAKE THE PLAYER CENTRAL AFTER A LIFE IS LOST
    }

    //UI
    public class Background : Sprite
    {
        GraphicsDevice Screen;

        Rectangle backgroundMiddle;
        Rectangle backgroundBottom;
        Rectangle backgroundBottomLeft;
        Rectangle backgroundBottomRight;
        Rectangle backgroundLeft;
        Rectangle backgroundRight;

        /// <summary>
        /// CONTRUCTOR 
        /// </summary>
        /// <param name="texture"> SPRIRE TEXTURE </param>
        /// <param name="position"> SPRITE POSITION </param>
        /// <param name="spriteWidth"> WIDTH </param>
        /// <param name="spriteHeight"> HEIGHT </param>
        public Background(Texture2D texture, Vector2 position, int spriteWidth, int spriteHeight, float layerDepth)
        {
            Active = true;
            BackgroundSpeed = 2;

            Skiing.mainGame.AddSprite(this);
            Skiing.menu.AddSprite(this);
        }
        public Background()
        {

        }

        //SPEED
        public static int SideSpeed;
        private static void SideSpeedChange()
        {
            SideSpeed = BackgroundSpeed + (BackgroundSpeed / 2) + 3;
        } //CHANGES THE SIDE SPEED
        private static int backgroundSpeed;
        public static int BackgroundSpeed //USED TO ALTER THE SPEED OF THE BACKGROUND
        {
            get
            {
                return backgroundSpeed;
            }
            set
            {
                backgroundSpeed = value;
                SideSpeedChange();
            }
        }
        //

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Initialize(GraphicsDevice screen)
        {
            int H = screen.Viewport.Height;
            int W = screen.Viewport.Width;

            //DRAWS THE BACKGROUNDS
            backgroundMiddle = new Rectangle(0, 0,
                screen.Viewport.Width, screen.Viewport.Height);

            backgroundBottom = new Rectangle(0, H,
                screen.Viewport.Width, screen.Viewport.Height);

            backgroundBottomLeft = new Rectangle(-W, H,
                screen.Viewport.Width, screen.Viewport.Height);

            backgroundBottomRight = new Rectangle(W, H,
                screen.Viewport.Width, screen.Viewport.Height);

            backgroundLeft = new Rectangle(-W, 0,
                screen.Viewport.Width, screen.Viewport.Height);

            backgroundRight = new Rectangle(W, 0,
                screen.Viewport.Width, screen.Viewport.Height);
            //--------------------------

            //GRABS THE SCREEN
            Screen = screen;
        }

        /// <summary>
        /// MOVES THE BACKGROUND X AXIS 
        /// </summary>
        /// <param name="screen">THE GRAPHICS DEVICE</param>
        private void resetBackgroundX(GraphicsDevice screen)
        {
            backgroundMiddle.X = 0;
            backgroundBottom.X = 0;
            backgroundBottomLeft.X = -screen.Viewport.Width;
            backgroundBottomRight.X = screen.Viewport.Width;
            backgroundLeft.X = -screen.Viewport.Width;
            backgroundRight.X = screen.Viewport.Width;
        }

        /// <summary>
        /// MOVES THE BACKGROUND TO THEIR ORIGINAL POSITIONS
        /// </summary>
        /// <param name="screen"> THE CURRENT GRAPHICS DEVICE </param>
        private void resetBackgroundY(GraphicsDevice screen)
        {
            backgroundMiddle.Y = 0;
            backgroundBottom.Y = screen.Viewport.Height;
            backgroundBottomLeft.Y = screen.Viewport.Height;
            backgroundBottomRight.Y = screen.Viewport.Height;
            backgroundLeft.Y = 0;
            backgroundRight.Y = 0;
        }

        //MOVES THE BACKGROUND DOWN
        public void moveDown(GraphicsDevice screen)
        {
            backgroundMiddle.Y -= backgroundSpeed;
            backgroundBottom.Y -= backgroundSpeed;
            backgroundRight.Y -= backgroundSpeed;
            backgroundLeft.Y -= backgroundSpeed;
            backgroundBottomRight.Y -= backgroundSpeed;
            backgroundBottomLeft.Y -= backgroundSpeed;

            //RESET All Y VALUES IF THEY GO TOO LOW
            if (backgroundMiddle.Y < -screen.Viewport.Height)
            {
                resetBackgroundY(screen);
            }

            //RESET ALL X VALUES IF THE BACKGROUND GO TOO FAR LEFT OR RIGHT
            if (backgroundMiddle.X > screen.Viewport.Width || backgroundMiddle.X + Content.Background.Width < screen.Viewport.Width)
            {
                resetBackgroundX(screen);
            }
        }

        //MOVE THE BACKGROUND LEFT
        public void moveLeft()
        {
            backgroundMiddle.X += SideSpeed;
            backgroundRight.X += SideSpeed;
            backgroundLeft.X += SideSpeed;
            backgroundBottom.X += SideSpeed;
            backgroundBottomLeft.X += SideSpeed;
            backgroundBottomRight.X += SideSpeed;
        }

        //MOVE THE BACKGROUND RIGHT
        public void moveRight()
        {
            backgroundMiddle.X -= SideSpeed;
            backgroundRight.X -= SideSpeed;
            backgroundLeft.X -= SideSpeed;
            backgroundBottom.X -= SideSpeed;
            backgroundBottomLeft.X -= SideSpeed;
            backgroundBottomRight.X -= SideSpeed;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Content.Background, backgroundMiddle, null, Color.White, 0.0f, new Vector2(0,0), SpriteEffects.None, 0.0f);
            spritebatch.Draw(Content.Background, backgroundBottom, null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);
            spritebatch.Draw(Content.Background, backgroundBottomLeft, null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);
            spritebatch.Draw(Content.Background, backgroundBottomRight, null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);
            spritebatch.Draw(Content.Background, backgroundLeft, null, Color.White, 0.0f, new Vector2(0,0), SpriteEffects.None, 0.0f);
            spritebatch.Draw(Content.Background, backgroundRight, null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 0.0f);

            //DEBUG
            //spritebatch.DrawString(Texture.PlayerName, backgroundSpeed.ToString() + "-" + SideSpeed.ToString(), new Vector2(700, 0), Color.Black);

            //base.Draw(spritebatch);
        }

        public override void Update(Skiing game, GameTime gametime)
        {
            if (!(Skiing.menu.Active == true))
            {
                if (Pause == false)
                {
                    moveDown(Screen);

                    if (keyboard.left())
                    {
                        moveLeft();
                    }

                    if (keyboard.right())
                    {
                        moveRight();
                    }
                }
            }
            else
            {
                MenuBackground MBackground = new MenuBackground();

                MBackground.Update(gametime);

                moveDown(Screen);

                if (Menu.MoveType == 1)
                {
                    moveLeft();
                }

                if (Menu.MoveType == 2)
                {
                    moveRight();
                }
            }

            base.Update(game, gametime);
        }

        public override void Reset(Skiing game)
        {
            SideSpeed = 3;
            BackgroundSpeed = 2;

            base.Reset(game);
        }

        public override void Save()
        {
            Skiing.mainGame.SaveData.Add(backgroundSpeed);

            //base.Save();
        }
        public override void Load(List<object> Data)
        {
            BackgroundSpeed = int.Parse(Data[0].ToString());
        }
    }
    public class Banner : Sprite
    {
        /// <summary>
        /// THE BANNER CONTRUCTOR
        /// </summary>
        /// <param name="texture"> THE BANNER TEXTURE </param>
        /// <param name="vec"> THE POSITION VECTOR </param>
        /// <param name="bannerW"> WIDTH </param>
        /// <param name="bannerH"> HEIGHT </param>
        public Banner(Texture2D texture, Vector2 vec, int bannerW, int bannerH, float layerDepth)
            : base(texture, vec, bannerW, bannerH, layerDepth)
        {

            Active = true;
            Skiing.mainGame.AddSprite(this);
        }

        string playerName = "SLIM DOWNHILL";
        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            //DRAWS THE PLAYERS NAME
            spritebatch.DrawString(Content.PlayerName, playerName, new Vector2(120, 405), Color.White, 0.0f, new Vector2(0, 0), new Vector2(1,1), SpriteEffects.None, 0.9f);

            //DRAWS THE PLAYERS TIME
            spritebatch.DrawString(Content.PlayerName, Score.ScoreCurrrent.ToString(), new Vector2(720, 405), Color.White, 0.0f, new Vector2(0, 0), new Vector2(1, 1), SpriteEffects.None, 0.9f);
        }

        public override void Update(Skiing game, GameTime gametime)
        {
            base.Update(game, gametime);
        }

        public override void Reset(Skiing game) //TO-DO
        {
            base.Reset(game);
        }
    }
    public class Lives : Sprite
    {
        public static int Count = 3; //THE AMOUNT OF LIVES
        public static bool JustCrashed = false; //TO PREVENT THE LOSING OF MULTIPLE LIVES AFTER ONE COLLISION

        public Lives()
        {
            Active = true;
            Skiing.mainGame.AddSprite(this);
        }

        //USED TO HELP DRAW THE LIVES DYNAMICALLY
        private int LoopX = 65;
        public override void Draw(SpriteBatch spritebatch)
        {
            //THIS IS USED TO DRAW THE LIVES STRING
            spritebatch.DrawString(Content.smallText, "LIVES", new Vector2(10, 15), Color.Black, 0.0f, new Vector2(0, 0), new Vector2(1, 1), SpriteEffects.None, 1f);

            for (int x = 0; x < Count; x++) //LOOPS AROUND DEPENING ON HOW MANY LIVES
            {
                Rectangle position = new Rectangle(LoopX, 10, Content.Skier.Width / 4, Content.Skier.Height / 4);

                spritebatch.Draw(Content.Skier, position, null, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1f);

                LoopX += 20;
            }

            LoopX = 65;

            //base.Draw(spritebatch);
        }

        public override void Update(Skiing game, GameTime gametime)
        {
            //PLAYS THE CRASH ANIMATION
            if (GameRelated.GameOver == true)
            {
                Skier.Crash = true;


            }

            base.Update(game, gametime);
        }

        public override void Reset(Skiing game)
        {
            Count = 3;
            LoopX = 65;
            JustCrashed = false;
            Score.ScoreCurrrent = 0;

            base.Reset(game);
        }

        public override void Save() //SAVE LOGIC
        {
            Skiing.mainGame.SaveData.Add(Count);

            Skiing.mainGame.SaveData.Add(Score.ScoreCurrrent);

            //base.Save();
        }

        public override void Load(List<object> Data)
        {
            int numberOfFlags = int.Parse(Data[1].ToString()) + 2;

            Count = int.Parse(Data[numberOfFlags].ToString());
            Score.ScoreCurrrent = int.Parse(Data[numberOfFlags + 1].ToString());

            base.Load(Data);
        } //LOAD LOGIC

        /// <summary>
        /// THIS RUNS WHEN THE PLAYER LOSES THEIR LAST LIFE
        /// </summary>
        public static void PlayerDie()
        {
            Count -= 1;
            GameRelated.GameOver = true;
            Skiing.mainGame.Pause = true;
            Skiing.Res.Active = true;
            HighestScore.HighScore = Score.ScoreCurrrent;
        }
    }
    public class Score : Sprite
    {
        public static int ScoreCurrrent = 0;

        public override void Reset(Skiing game)
        {
            ScoreCurrrent = 0;

            base.Reset(game);
        }

        //USED TO SAVE THE AND LOAD THE SCORE
        public override void Save()
        {
            Skiing.mainGame.SaveData.Add(ScoreCurrrent);
            //base.Save();
        }
        public override void Load(List<object> Data)
        {
            base.Load(Data);
        }
    }
   
    //FONTS
    public class HighestScore : Sprite
    {
        public static int HighScore;

        public HighestScore()
        {
            Skiing.menu.AddSprite(this);
            Active = true;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(Content.smallText, "HIGHEST SCORE : " + HighScore.ToString(), new Vector2(10, 10), Color.Black, 0.0f, new Vector2(0, 0), new Vector2(1, 1), SpriteEffects.None, 1.0f);
            //base.Draw(spritebatch);
        }

        public override void Update(Skiing game, GameTime gametime)
        {
            if (Score.ScoreCurrrent > HighScore)
            {
                HighScore = Score.ScoreCurrrent;
            }

            //base.Update(game, gametime);
        }
    }
    public class UserMessages : Sprite
    {
        bool save; //BOOL THATS USED TO DETERMING
        public UserMessages(bool SaveOrLoad)
        {
            save = SaveOrLoad;
            //TRUE == SAVE
            //FALSE == LOAD     

            Active = true;
            Skiing.mainGame.AddSprite(this);
        }

        //DRAWS THE DATA
        public override void Draw(SpriteBatch spritebatch)
        {
            if (save == true)
            {
                Save(spritebatch);
            }
            else
            {
                Load(spritebatch);
            }

            //base.Draw(spritebatch);
        }

        //CAN'T BE PAUSED SO THE UPDATENOPAUSE IS USED
        public override void UpdateNoPause(Skiing game, GameTime gametime)
        {
            elaspedMessageTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            base.UpdateNoPause(game, gametime);
        }

        private float delaySL = 1200f; //HOW LONG THE MESSAGE WILL APPEAR FOR
        private float elaspedMessageTime;
        public void Save(SpriteBatch sb)
        {
            if (elaspedMessageTime <= delaySL)
            {
                sb.DrawString(Content.saveAndLoad, "GAME SAVED", new Vector2(275, 250), Color.Green, 0.0f, new Vector2(0, 0), new Vector2(1,1), SpriteEffects.None, 1.0f);
            }
            else
            {
                Active = false;
                Skiing.mainGame.Active = false;
                Skiing.menu.Active = true;
            }
        }
        public void Load(SpriteBatch sb)
        {
            if (elaspedMessageTime <= delaySL)
            {
                sb.DrawString(Content.saveAndLoad, "GAME LOADED", new Vector2(260, 250), Color.Red, 0.0f, new Vector2(0, 0), new Vector2(1, 1), SpriteEffects.None, 1.0f);
            }
            else
            {
                Active = false;
            }
        }

    }
}
