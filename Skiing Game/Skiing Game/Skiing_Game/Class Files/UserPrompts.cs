using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SkiingGameCoursework
{
    public class SpaceKey : Sprite
    {
        Texture2D SpaceTexture;
        Rectangle SpaceKeyRec;

        float ElaspedTime;
        float Delay = 1000f;
        int Frame = 0;

        public SpaceKey(Texture2D Tex, Vector2 pos, int W, int H)
        {
            SpaceTexture = Tex;
            SpaceKeyRec = new Rectangle((int)pos.X, (int)pos.Y, W, H);

            Active = true;
            Skiing.userPrompts.AddSprite(this);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            Rectangle takeImage = new Rectangle(1000 * Frame, 0, 1000, 160);

            spritebatch.Draw(SpaceTexture, SpaceKeyRec, takeImage, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1f);

            //base.Draw(spritebatch);
        }

        public override void Update(Skiing game, GameTime gametime)
        {
            if (keyboard.space())
            {
                Active = false;
            }

            ElaspedTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (ElaspedTime >= Delay) //AFTER THE ELASPED TIME IS GREATER THAN THE DELAY
            {
                //CHANGES THE FRAMES
                if (Frame == 1)
                {
                    Frame = 0;
                }
                else
                {
                    Frame = 1;
                }
                //

                ElaspedTime = 0; //RESETS THE TIME
            }

            base.Update(game, gametime);
        }
    }
    public class RestartPrompt : Sprite
    {
        //POSITIONS FOR THE RECTANGLES
        public Rectangle NKey;
        public Rectangle QKey;
        public RestartPrompt()
        {
            NKey = new Rectangle(200, 300, 560 / 6, 528 / 6);
            QKey = new Rectangle(500, 300, 560 / 6, 528 / 6);

            Active = false;
            Skiing.userPrompts.AddSprite(this);
        }

        //RECTANGLES USED TO ANIMATE
        Rectangle TakeImageNKey;
        Rectangle TakeImageQKey;
        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(Content.saveAndLoad, "QUIT GAME", new Vector2(QKey.X - 35, 250), Color.Black, 0.0f, new Vector2(0, 0), new Vector2(1, 1), SpriteEffects.None, 1f);
            spritebatch.Draw(Content.qKey, QKey, TakeImageQKey, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1f);

            spritebatch.DrawString(Content.saveAndLoad, "NEW GAME", new Vector2(NKey.X - 25, 250), Color.Black, 0.0f, new Vector2(0, 0), new Vector2(1, 1), SpriteEffects.None, 1f);
            spritebatch.Draw(Content.nKey, NKey, TakeImageNKey, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1f);

            //base.Draw(spritebatch);
        }

        public override void Update(Skiing game, GameTime gametime)
        {
            nKey(gametime);
            qKey(gametime);

            base.Update(game, gametime);
        }

        public override void Reset(Skiing game)
        {
            Active = false;

            base.Reset(game);
        }

        private float elaspedTime;
        private float delay = 1000f;
        private int frame = 0;
        public void nKey(GameTime gametime)
        {
            elaspedTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (elaspedTime >= delay) //AFTER THE ELASPED TIME IS GREATER THAN THE DELAY
            {
                //CHANGES THE FRAMES
                if (frame == 1)
                {
                    frame = 0;
                }
                else
                {
                    frame = 1;
                }
                //

                elaspedTime = 0; //RESETS THE TIME
            }

            TakeImageNKey = new Rectangle(560 * frame, 0, 560, 528);
        }
        public void qKey(GameTime gametime)
        {
            elaspedTime += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (elaspedTime >= delay) //AFTER THE ELASPED TIME IS GREATER THAN THE DELAY
            {
                //CHANGES THE FRAMES
                if (frame == 1)
                {
                    frame = 0;
                }
                else
                {
                    frame = 1;
                }
                //

                elaspedTime = 0; //RESETS THE TIME
            }

            TakeImageQKey = new Rectangle(560 * frame, 0, 560, 528);
        }

    }
}
