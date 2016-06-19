using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SkiingGameCoursework
{
    public class ParticleSystem
    {
        protected List<Particle> ActiveParticles = new List<Particle>(); //THE LIST OF ACTIVE PARTICLES

        public Texture2D Texture;
        protected float LifeTime; //THE LIFE TIME OF EACH PARTICLE
        protected Random Randomizer = new Random(); //USED IN RANDOM CACULATIONS
        protected float SpawnTime;
        protected int MaxParticles;
        protected bool Active = true;

        //POINT
        public Vector2 EmitterPosition; //IF THE PARTICLE SYSTEM JUST EMITTS PARTICLES FROM A SINGLE POINT
        public ParticleSystem(Texture2D PTexture, Vector2 EmissionPosition, float SpawnRate, float GeneralLifeTime, int maxParticles)
        {
            EmitterPosition = EmissionPosition;
            SpawnTime = SpawnRate;
            LifeTime = GeneralLifeTime;
            MaxParticles = maxParticles;
            Texture = PTexture;
        }

        //AREA
        protected Vector2 startPosition;
        protected int topLength;
        protected int sideLength;
        public ParticleSystem(Texture2D PTexture, Vector2 EmissionStartPoint, int TopLength, int SideLength, float SpawnRate, float GeneralLifeTime, int maxParticles)
        {
            Texture = PTexture;

            startPosition = EmissionStartPoint;
            topLength = TopLength;
            sideLength = SideLength;

            SpawnTime = SpawnRate;
            LifeTime = GeneralLifeTime;
            MaxParticles = maxParticles;
        }

        private List<Particle> Delete = new List<Particle>();
        public virtual void Update(GameTime gameTime)
        {
            foreach (Particle Particle in ActiveParticles)
            {
                if (Particle.Alive == true)
                {
                    Particle.Update(gameTime);
                }
                else
                {
                    Delete.Add(Particle); //TO DELETE WHEN THE FOREACH IS DONE
                }
            }

            RemoveDeadParticle();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach(Particle Particle in ActiveParticles)
            {
                if (Particle.Alive == true)
                {
                    Particle.Draw(spriteBatch);
                }
            }
        }

        public virtual void AddParticle()
        {

        }

        private void RemoveDeadParticle()
        {
            foreach (Particle Particle in Delete)
            {
                ActiveParticles.Remove(Particle); //TURN INTO FOR LOOP 
            }
            Delete.Clear();
        }

        public void Reset()
        {
            ActiveParticles.Clear();
        }
    }

    public class Particle
    {
        //ATTRIBUTES
        public Texture2D Texture;
        private float LifeTime;
        private float DeltaTime; //THE REAL WORLD TIME PASSED
        public bool Alive;
        public float RotationAngle;
        public float AngleVelocity;
        public Vector2 Scale;
        public Vector2 Velocity;
        public Vector2 Position;
        public Color Color;
        public float Alpha;
        public float LayerDepth;

        public Particle(Texture2D texture, float lifeTime)
        {
            Texture = texture; //SETS THE TEXTURE
            LifeTime = lifeTime; //SETS THE LIFETIME

            Alive = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            DeltaTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //COUNTS THE TIME, THIS IS USED TO CHECK HOW OLD THE PARTICLE IS
            
            if (DeltaTime >= LifeTime)
            {
                Alive = false;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture, 
                Position, 
                null, 
                Color * Alpha,
                RotationAngle, 
                new Vector2(Texture.Width, Texture.Height), 
                Scale, 
                SpriteEffects.None, 
                LayerDepth);
        }
    }

    public class Snow : ParticleSystem
    {
        public Snow(Texture2D PTexture, Vector2 EmissionStartPoint, int TopLength, int SideLength, float SpawnRate, float GeneralLifeTime, int maxParticles)
            : base(PTexture, EmissionStartPoint, TopLength, SideLength, SpawnRate, GeneralLifeTime, maxParticles)
        {
           //NO EXTRAS NEEDED
        }

        float ElaspedTime;
        public override void Update(GameTime gameTime)
        {
            //DELTA TIME
            ElaspedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < ActiveParticles.Count; i++)
            {
                //MOVEMENT
                ActiveParticles[i].Position += ActiveParticles[i].Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //AFTER THE TIME HAS ELASPED
            if (ElaspedTime >= SpawnTime)
            {
                if (ActiveParticles.Count <= MaxParticles)
                {
                    AddParticle();
                }

                ElaspedTime = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //DEBUG
            //spriteBatch.DrawString(Content.PlayerName, ActiveParticles.Count.ToString(), new Vector2(10, 10), Color.Black);

            base.Draw(spriteBatch);
        }

        public override void AddParticle()
        {
            Particle Snowflake = new Particle(Texture, LifeTime);

            Snowflake.Color = Color.White;

            Snowflake.Scale = new Vector2(1, 1);

            Snowflake.Alpha = 1.0f;

            Snowflake.LayerDepth = 0.81f;

            //RANDOM POSITION OF SNOW
            float X = (float)Randomizer.Next((int)startPosition.X, topLength);
            float Y = (float)Randomizer.Next((int)startPosition.Y, sideLength);

            Snowflake.Position = new Vector2(X, Y);

            //RANDOM VELOCITY OF SNOW
            Snowflake.Velocity.X = 0;
            Snowflake.Velocity.Y = 100;

            Snowflake.Alive = true;

            ActiveParticles.Add(Snowflake); //MAKES THE PARTICLE ACTIVE
        } //THE LOGIC BEHIND WHEN TO ADD MORE PARTICLES
    }

    public class SnowTrail : ParticleSystem
    {
        public SnowTrail(Texture2D PTexture, Vector2 EmitterPositon, float SpawnRate, float GeneralLifeTime, int maxParticles)
            : base(PTexture, EmitterPositon, SpawnRate, GeneralLifeTime, maxParticles)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        float ElaspedTime;
        public override void Update(GameTime gameTime)
        {
            ElaspedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            for (int i = 0; i < ActiveParticles.Count; i++)
            {
                //MOVES THE PARTICLES UP
                ActiveParticles[i].Position.Y -= Background.BackgroundSpeed;

                if (keyboard.left())
                {
                    ActiveParticles[i].Position.X += Background.SideSpeed;
                }
                else
                {
                    if (keyboard.right())
                    {
                        ActiveParticles[i].Position.X -= Background.SideSpeed;
                    }
                }
            }

            //ADDS A NEW PARTICLE
            if (true)
            {
                if (ActiveParticles.Count <= MaxParticles)
                {
                    AddParticle();
                }
                else
                {

                }

                ElaspedTime = 0;
            }





            base.Update(gameTime);
        }

        public override void AddParticle()
        {
            Particle SnowTrail = new Particle(Texture, LifeTime);

            SnowTrail.Color = Color.White;

            SnowTrail.Scale = new Vector2(0.9f, 0.7f);

            SnowTrail.Alpha = 1.0f;

            SnowTrail.Position = EmitterPosition;

            SnowTrail.LayerDepth = 0.01f;

            SnowTrail.Alive = true;

            ActiveParticles.Add(SnowTrail);
        }
    }
}
