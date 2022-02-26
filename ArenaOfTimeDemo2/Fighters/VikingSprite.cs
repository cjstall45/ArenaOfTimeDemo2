using ArenaOfTimeDemo2.StateManagement;
using ArenaOfTimeDemo2.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ArenaOfTimeDemo2.Fighters
{
    public class VikingSprite
    {
        private AnimationState animationState;

        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState;
        private GamePadState gamePadState;
        private GamePadState previousGamePadState;

        private float animationSpeed = 0.15f;

        private Texture2D[] idleTextures = new Texture2D[6];
        private Texture2D[] walkingTextures = new Texture2D[6];
        private Texture2D[] attackTextures = new Texture2D[6];
        private Texture2D[] hitTextures = new Texture2D[3];
        private Texture2D[] deadTextures = new Texture2D[4];
        private Texture2D[] blockTextures = new Texture2D[5];

        private double animationTimer;

        private int animationFrame;
        private int playerNumber;
        public float HealthPercent = 1;
        public Vector2 Position;
        public Hitbox Hurtbox;
        public Hitbox Attack1Hitbox;
        public bool CollidingLeft = false;
        public bool CollidingRight = false;
        public bool dead = false;
        private bool activeAnimation = false;
        private SoundEffect hitSound;
        private SoundEffect damageSound;

        /// <summary>
        /// initalized the sprite in its starting position. Adds Hurtboxes and hitboxes for the sprits attacks
        /// </summary>
        /// <param name="player">the index for player 1 or 2</param>
        public VikingSprite(int player, int screenSize)
        {
            if (player == 1)
            {
                playerNumber = 1;
                Position = new Vector2(80, 300);
                Hurtbox = new Hitbox(Position.X, Position.Y - 60, 76, 120);
                Attack1Hitbox = new Hitbox(Hurtbox.Bounds.Right, Hurtbox.Bounds.Top + 7, 76, 148);
                Attack1Hitbox.Active = false;
            }
            else
            {
                playerNumber = 2;
                Position = new Vector2(screenSize - 130, 300);
                Hurtbox = new Hitbox(Position.X , Position.Y, 76, 120);
                Attack1Hitbox = new Hitbox(Hurtbox.Bounds.Left - 76, Hurtbox.Bounds.Top + 7, 76, 148);
                Attack1Hitbox.Active = false;
            }
        }
        /// <summary>
        /// a methoid called by the collision detection in the main game that indicates the player was hit and updates values and animations to match
        /// </summary>
        public void Hit()
        {
            animationFrame = 0;
            Hurtbox.Active = false;
            animationState = AnimationState.hit;
            hitSound.Play();
            damageSound.Play();
            activeAnimation = true;
        }


        /// <summary>
        /// loads content for all the animations into seperate arrays for each one
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            idleTextures[0] = content.Load<Texture2D>("ready_1.1");
            idleTextures[1] = content.Load<Texture2D>("ready_2.1");
            idleTextures[2] = content.Load<Texture2D>("ready_3.1");
            idleTextures[3] = content.Load<Texture2D>("ready_4.1");
            idleTextures[4] = content.Load<Texture2D>("ready_5.1");
            idleTextures[5] = content.Load<Texture2D>("ready_6.1");
            walkingTextures[0] = content.Load<Texture2D>("walk_1");
            walkingTextures[1] = content.Load<Texture2D>("walk_2");
            walkingTextures[2] = content.Load<Texture2D>("walk_3");
            walkingTextures[3] = content.Load<Texture2D>("walk_4");
            walkingTextures[4] = content.Load<Texture2D>("walk_5");
            walkingTextures[5] = content.Load<Texture2D>("walk_6");
            attackTextures[0] = content.Load<Texture2D>("attack1_1");
            attackTextures[1] = content.Load<Texture2D>("attack1_2");
            attackTextures[2] = content.Load<Texture2D>("attack1_3");
            attackTextures[3] = content.Load<Texture2D>("attack1_4");
            attackTextures[4] = content.Load<Texture2D>("attack1_5");
            attackTextures[5] = content.Load<Texture2D>("attack1_6");
            hitTextures[0] = content.Load<Texture2D>("hit_1");
            hitTextures[1] = content.Load<Texture2D>("hit_2");
            hitTextures[2] = content.Load<Texture2D>("hit_3");
            deadTextures[0] = content.Load<Texture2D>("dead_1");
            deadTextures[1] = content.Load<Texture2D>("dead_2");
            deadTextures[2] = content.Load<Texture2D>("dead_3");
            deadTextures[3] = content.Load<Texture2D>("dead_4");
            blockTextures[0] = content.Load<Texture2D>("block_1");
            blockTextures[1] = content.Load<Texture2D>("block_2");
            blockTextures[2] = content.Load<Texture2D>("block_3");
            blockTextures[3] = content.Load<Texture2D>("block_4");
            blockTextures[4] = content.Load<Texture2D>("block_5");
            hitSound = content.Load<SoundEffect>("hit-impact-sword-2");
            damageSound = content.Load<SoundEffect>("voice-adultmale-paingrunts-12");
        }

        /// <summary>
        /// adds controls for player one on keyboard and player 2 on controller. updates animation state based on input and moves hitboxes for collisions 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(0); 
            if (!activeAnimation && playerNumber == 1 && keyboardState.IsKeyDown(Keys.J) && previousKeyboardState.IsKeyUp(Keys.J))
            {
                activeAnimation = true;
                animationState = AnimationState.attack1;
                animationFrame = 0;
                animationSpeed = .125f;
            }
            else if (playerNumber == 1 && !activeAnimation && keyboardState.IsKeyDown(Keys.A))
            {
                if (!CollidingLeft) Position += new Vector2((float)-2, 0); 
                animationState = AnimationState.backingup;
            }
            else if (playerNumber == 1 && !activeAnimation &&  keyboardState.IsKeyDown(Keys.D))
            {
                if (!CollidingRight) { Position += new Vector2((float)2, 0); }
                animationState = AnimationState.walking;
            }
            else if (playerNumber == 1 && !activeAnimation && keyboardState.IsKeyDown(Keys.K))
            {
                activeAnimation = true;
                animationState = AnimationState.block;
                animationFrame = 0;
                Hurtbox.Active = false;
            }
            else if (!activeAnimation && playerNumber != 1 && gamePadState.IsButtonDown(Buttons.A) && previousGamePadState.IsButtonUp(Buttons.A))
            {
                activeAnimation = true;
                animationState = AnimationState.attack1;
                animationFrame = 0;
                animationSpeed = .125f;
            }
            else if (playerNumber != 1 && !activeAnimation && gamePadState.IsButtonDown(Buttons.B) && previousGamePadState.IsButtonUp(Buttons.B))
            {
                activeAnimation = true;
                animationState = AnimationState.block;
                animationFrame = 0;
                Hurtbox.Active = false;
            }
            else if (playerNumber!= 1 && !activeAnimation && gamePadState.IsButtonDown(Buttons.DPadLeft))
            {
                if (!CollidingLeft) { Position += new Vector2((float)-2, 0); }
                animationState = AnimationState.backingup;
            }
            else if (playerNumber != 1 && !activeAnimation && gamePadState.IsButtonDown(Buttons.DPadRight))
            {
                if (!CollidingRight) Position += new Vector2((float)2, 0); 
                animationState = AnimationState.walking;
            }
            else
            {
                if ((int)animationState < 3)
                {
                    animationState = AnimationState.idle;
                }

            }
            previousKeyboardState = keyboardState;
            Hurtbox.Bounds.X = Position.X;
            Hurtbox.Bounds.Y = Position.Y;
            if(playerNumber == 1)
            {
                Attack1Hitbox.Bounds.X = Hurtbox.Bounds.Right;
            }
            else
            {
                Attack1Hitbox.Bounds.X = Hurtbox.Bounds.Left - 76;
            }
        }

        /// <summary>
        /// draws all animations. Creates one switch for timing and handleing the looping or ending of the animation and one switch method for the actual draw function
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > animationSpeed)
            {
                animationFrame++;
                switch (animationState)
                {
                    case AnimationState.idle:
                        if (animationFrame > 5) animationFrame = 0;
                        break;
                    case AnimationState.walking:
                        if (animationFrame > 5) animationFrame = 0;
                        break;
                    case AnimationState.backingup:
                        if (animationFrame > 5) animationFrame = 0;
                        break;
                    case AnimationState.attack1:
                        if (animationFrame > 5)
                        {
                            animationSpeed = 0.15f;
                            animationFrame = 0;
                            animationState = AnimationState.idle;
                            activeAnimation = false;
                        }
                        break;
                    case AnimationState.hit:
                        if(animationFrame > 2)
                        {
                            animationFrame = 0;
                            animationState = AnimationState.idle;
                            Hurtbox.Active = true;
                            Attack1Hitbox.Active = false;
                            activeAnimation = false;
                            HealthPercent -= .33f;
                            if(HealthPercent < 0)
                            {
                                HealthPercent = 0;
                                animationState = AnimationState.dead;
                                activeAnimation = true;
                                Hurtbox.Active = false;
                            }
                        }
                        break;
                    case AnimationState.dead:
                        if(animationFrame > 3)
                        {
                            animationFrame = 3;
                            dead = true;
                        }
                        break;
                    case AnimationState.block:
                        if(animationFrame > 4)
                        {
                            animationFrame = 0;
                            animationState = AnimationState.idle;
                            animationSpeed = .15f;
                            Hurtbox.Active = true;
                            activeAnimation = false;
                        }
                        break;
                }
                animationTimer -= animationSpeed;
            }
            switch (animationState)
            {
                case AnimationState.idle:
                    if (playerNumber == 1)
                    {
                        spriteBatch.Draw(idleTextures[animationFrame], Position, null, Color.White, 0, new Vector2(0, 0), 3.8f, SpriteEffects.None, .5f);
                    }
                    else
                    {
                        spriteBatch.Draw(idleTextures[animationFrame], Position, null, Color.White, 0, new Vector2(0, 0), 3.8f, SpriteEffects.FlipHorizontally, .5f);
                    }
                    break;
                case AnimationState.walking:
                    if (playerNumber == 1)
                    {
                        spriteBatch.Draw(walkingTextures[animationFrame], Position, null, Color.White, 0, new Vector2(0, 1), 3.8f, SpriteEffects.None, .5f);
                    }
                    else
                    {
                        spriteBatch.Draw(walkingTextures[5 - animationFrame], Position, null, Color.White, 0, new Vector2(0, 1), 3.8f, SpriteEffects.FlipHorizontally, .5f);
                    }
                    break;
                case AnimationState.backingup:
                    if (playerNumber == 1)
                    {
                        spriteBatch.Draw(walkingTextures[5 - animationFrame], Position, null, Color.White, 0, new Vector2(0, 1), 3.8f, SpriteEffects.None, .5f);
                    }
                    else
                    {
                        spriteBatch.Draw(walkingTextures[animationFrame], Position, null, Color.White, 0, new Vector2(0, 1), 3.8f, SpriteEffects.FlipHorizontally, .5f);
                    }
                    
                    break;
                case AnimationState.attack1:
                    if(playerNumber == 1)
                    {
                        spriteBatch.Draw(attackTextures[animationFrame], Position, null, Color.White, 0, new Vector2(16, 7), 3.8f, SpriteEffects.None, 0);
                        if(animationFrame == 3)
                        {
                            Attack1Hitbox.Active = true;
                        }
                        else
                        {
                            Attack1Hitbox.Active = false;
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(attackTextures[animationFrame], Position, null, Color.White, 0, new Vector2(22, 7), 3.8f, SpriteEffects.FlipHorizontally, 0);
                        if (animationFrame == 3)
                        {
                            Attack1Hitbox.Active = true;
                        }
                        else
                        {
                            Attack1Hitbox.Active = false;
                        }
                    }
                    break;
                case AnimationState.hit:
                    if(playerNumber == 1)
                    {
                        spriteBatch.Draw(hitTextures[animationFrame], Position, null, Color.Red, 0, new Vector2(3, -1), 3.8f, SpriteEffects.None, .5f);
                    }
                    else
                    {
                        spriteBatch.Draw(hitTextures[animationFrame], Position, null, Color.Red, 0, new Vector2(0, -1), 3.8f, SpriteEffects.FlipHorizontally, .5f);
                    }
                    break;
                case AnimationState.dead:
                    if (playerNumber == 1)
                    {
                        spriteBatch.Draw(deadTextures[animationFrame], Position, null, Color.White, 0, new Vector2(3, -1), 3.8f, SpriteEffects.None, .5f);
                    }
                    else
                    {
                        spriteBatch.Draw(deadTextures[animationFrame], Position, null, Color.White, 0, new Vector2(0, -1), 3.8f, SpriteEffects.FlipHorizontally, .5f);
                    }
                    break;
                case AnimationState.block:
                    if (playerNumber == 1)
                    {
                        spriteBatch.Draw(blockTextures[animationFrame], new Vector2(Position.X - 4 , Position.Y - 34), null, Color.Gray, 0, new Vector2(0, 0), 3.8f, SpriteEffects.None, .5f);
                    }
                    else
                    {
                        spriteBatch.Draw(blockTextures[animationFrame], new Vector2(Position.X - 19 , Position.Y - 34), null, Color.Gray, 0, new Vector2(0, 0), 3.8f, SpriteEffects.FlipHorizontally, .5f);
                    }
                    break;
            }
        }
    }
}
