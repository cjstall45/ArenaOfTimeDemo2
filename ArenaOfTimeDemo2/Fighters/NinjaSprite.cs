using ArenaOfTimeDemo2.Collisions;
using ArenaOfTimeDemo2.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaOfTimeDemo2.Fighters
{
    class NinjaSprite : FighterSprite
    {
        private AnimationState animationState;

        private float animationSpeed = 0.1f;

        private Texture2D idleTexture;
        private Texture2D walkingTexture;
        private Texture2D attackTexture;
        private Texture2D hitTexture;
        private Texture2D deadTexture;


        private double animationTimer;

        private int animationFrame;
        private int playerNumber;
        public float HealthPercent { get; set; } = 1;
        public Vector2 Position { get; set; }
        public Hitbox Hurtbox { get; set; }
        public Hitbox Attack1Hitbox { get; set; }
        public bool CollidingLeft { get; set; } = false;
        public bool CollidingRight { get; set; } = false;
        public bool dead { get; set; } = false;
        private bool activeAnimation = false;
        public Characters attacker;
        private SoundEffect hitSound;
        private SoundEffect damageSound;

        /// <summary>
        /// initalized the sprite in its starting position. Adds Hurtboxes and hitboxes for the sprits attacks
        /// </summary>
        /// <param name="player">the index for player 1 or 2</param>
        public NinjaSprite(int player, int screenSize)
        {
            if (player == 1)
            {
                playerNumber = 1;
                Position = new Vector2(80, 290);
                Hurtbox = new Hitbox(Position.X, Position.Y, 80, 120);
                Attack1Hitbox = new Hitbox(Hurtbox.Bounds.Right, Hurtbox.Bounds.Top + 7, 120, 148);
                Attack1Hitbox.Active = false;
            }
            else
            {
                playerNumber = 2;
                Position = new Vector2(screenSize - 130, 290);
                Hurtbox = new Hitbox(Position.X + 20, Position.Y, 80, 120);
                Attack1Hitbox = new Hitbox(Hurtbox.Bounds.Left - 120, Hurtbox.Bounds.Top + 7, 120, 148);
                Attack1Hitbox.Active = false;
            }
        }
        /// <summary>
        /// a methoid called by the collision detection in the main game that indicates the player was hit and updates values and animations to match
        /// </summary>
        public void Hit(Characters attack)
        {
            attacker = attack;
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
            idleTexture = content.Load<Texture2D>("Idle");
            hitTexture = content.Load<Texture2D>("Take hit");
            attackTexture = content.Load<Texture2D>("Attack1");
            hitSound = content.Load<SoundEffect>("hit-impact-sword-2");
            walkingTexture = content.Load<Texture2D>("Run");
            deadTexture = content.Load<Texture2D>("Death");
            damageSound = content.Load<SoundEffect>("voice-adultmale-paingrunts-12");
        }

        /// <summary>
        /// adds controls for player one on keyboard and player 2 on controller. updates animation state based on input and moves hitboxes for collisions 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, InputState input)
        {

            var player = new PlayerIndex();
            if (playerNumber == 1 && !activeAnimation && input.IsButtonPressed(Buttons.A, PlayerIndex.One, out player))
            {
                activeAnimation = true;
                animationState = AnimationState.attack1;
                animationFrame = 0;
                animationSpeed = .125f;
            }
            else if (playerNumber == 1 && !activeAnimation && (input.IsButtonPressed(Buttons.DPadLeft, PlayerIndex.One, out player) || input.IsButtonPressed(Buttons.LeftThumbstickLeft, PlayerIndex.One, out player)))
            {
                if (!CollidingLeft) Position += new Vector2((float)-4, 0);
                animationState = AnimationState.backingup;
            }
            else if (playerNumber == 1 && !activeAnimation && (input.IsButtonPressed(Buttons.DPadRight, PlayerIndex.One, out player) || input.IsButtonPressed(Buttons.LeftThumbstickRight, PlayerIndex.One, out player)))
            {
                if (!CollidingRight) { Position += new Vector2((float)4, 0); }
                animationState = AnimationState.walking;
            }
            else if (playerNumber == 2 && !activeAnimation && input.IsButtonPressed(Buttons.A, PlayerIndex.Two, out player))
            {
                activeAnimation = true;
                animationState = AnimationState.attack1;
                animationFrame = 0;
                animationSpeed = .125f;
            }
            else if (playerNumber == 2 && !activeAnimation && (input.IsButtonPressed(Buttons.DPadLeft, PlayerIndex.Two, out player) || input.IsButtonPressed(Buttons.LeftThumbstickLeft, PlayerIndex.Two, out player)))
            {
                if (!CollidingLeft) { Position += new Vector2((float)-4, 0); }
                animationState = AnimationState.walking;
            }
            else if (playerNumber == 2 && !activeAnimation && (input.IsButtonPressed(Buttons.DPadRight, PlayerIndex.Two, out player) || input.IsButtonPressed(Buttons.LeftThumbstickRight, PlayerIndex.Two, out player)))
            {
                if (!CollidingRight) Position += new Vector2((float)4, 0);
                animationState = AnimationState.backingup;
            }
            else
            {
                if ((int)animationState < 3)
                {
                    animationState = AnimationState.idle;
                }

            }
            
            Hurtbox.Bounds.Y = Position.Y;
            if (playerNumber == 1)
            {
                Hurtbox.Bounds.X = Position.X;
                Attack1Hitbox.Bounds.X = Hurtbox.Bounds.Right;
            }
            else
            {
                Hurtbox.Bounds.X = Position.X + 20;
                Attack1Hitbox.Bounds.X = Hurtbox.Bounds.Left - 120;
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
                        if (animationFrame > 3) animationFrame = 0;
                        break;
                    case AnimationState.walking:
                        if (animationFrame > 7) animationFrame = 0;
                        break;
                    case AnimationState.backingup:
                        if (animationFrame > 7) animationFrame = 0;
                        break;
                    case AnimationState.attack1:
                        if (animationFrame > 3)
                        {
                            animationSpeed = 0.1f;
                            animationFrame = 0;
                            animationState = AnimationState.idle;
                            activeAnimation = false;
                        }
                        break;
                    case AnimationState.hit:
                        if (animationFrame > 2)
                        {
                            animationFrame = 0;
                            animationState = AnimationState.idle;
                            Hurtbox.Active = true;
                            Attack1Hitbox.Active = false;
                            activeAnimation = false;
                            if (attacker == Characters.Ninja)
                            {
                                HealthPercent -= .20f;
                            }
                            else
                            {
                                HealthPercent -= .50f;
                            }
                            if (HealthPercent <= 0)
                            {
                                HealthPercent = 0;
                                animationState = AnimationState.dead;
                                activeAnimation = true;
                                Hurtbox.Active = false;
                            }
                        }
                        break;
                    case AnimationState.dead:
                        if (animationFrame > 6)
                        {
                            animationFrame = 6;
                            dead = true;
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
                        var source = new Rectangle(animationFrame * 200, 0, 200, 200);
                        spriteBatch.Draw(idleTexture, Position, source, Color.White, 0, new Vector2(75, 64), 2.0f, SpriteEffects.None, 0);
                    }
                    else
                    {
                        var source = new Rectangle(animationFrame * 200, 0, 200, 200);
                        spriteBatch.Draw(idleTexture, Position, source, Color.White, 0, new Vector2(75, 64), 2.0f, SpriteEffects.FlipHorizontally, 0);
                    }
                    break;
                case AnimationState.walking:
                    if (playerNumber == 1)
                    {
                        var source = new Rectangle(animationFrame * 200, 0, 200, 200);
                        spriteBatch.Draw(walkingTexture, Position, source, Color.White, 0, new Vector2(75, 64), 2.0f, SpriteEffects.None, 0);
                    }
                    else
                    {
                        var source = new Rectangle(animationFrame * 200, 0, 200, 200);
                        spriteBatch.Draw(walkingTexture, Position, source, Color.White, 0, new Vector2(75, 64), 2.0f, SpriteEffects.FlipHorizontally, 0);
                    }
                    break;
                case AnimationState.backingup:
                    if (playerNumber == 1)
                    {
                        var source = new Rectangle((7 - animationFrame) * 200, 0, 200, 200);
                        spriteBatch.Draw(walkingTexture, Position, source, Color.White, 0, new Vector2(75, 64), 2.0f, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        var source = new Rectangle((7 - animationFrame) * 200, 0, 200, 200);
                        spriteBatch.Draw(walkingTexture, Position, source, Color.White, 0, new Vector2(75, 64), 2.0f, SpriteEffects.FlipHorizontally, 0f);
                    }
                    break;
                case AnimationState.attack1:
                    if (playerNumber == 1)
                    {
                        var source = new Rectangle(animationFrame * 200, 0, 200, 200);
                        spriteBatch.Draw(attackTexture, Position, source, Color.White, 0, new Vector2(75, 64), 2.0f, SpriteEffects.None, 0f);
                        if (animationFrame == 2)
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
                        var source = new Rectangle(animationFrame * 200, 0, 200, 200);
                        spriteBatch.Draw(attackTexture, Position, source, Color.White, 0, new Vector2(75, 64), 2.0f, SpriteEffects.FlipHorizontally, 0f);
                        if (animationFrame == 2)
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
                    if (playerNumber == 1)
                    {
                        var source = new Rectangle(animationFrame * 200, 0, 200, 200);
                        spriteBatch.Draw(hitTexture, Position, source, Color.Red, 0, new Vector2(75, 64), 2.0f, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        var source = new Rectangle(animationFrame * 200, 0, 200, 200);
                        spriteBatch.Draw(hitTexture, Position, source, Color.Red, 0, new Vector2(75, 64), 2.0f, SpriteEffects.FlipHorizontally, 0f);
                    }
                    break;
                case AnimationState.dead:
                    if (playerNumber == 1)
                    {
                        var source = new Rectangle(animationFrame * 200, 0, 200, 200);
                        spriteBatch.Draw(deadTexture, Position, source, Color.White, 0, new Vector2(75, 64), 2.0f, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        var source = new Rectangle(animationFrame * 200, 0, 200, 200);
                        spriteBatch.Draw(deadTexture, Position, source, Color.White, 0, new Vector2(75, 64), 2.0f, SpriteEffects.FlipHorizontally, 0f);
                    }
                    break;
            }
        }
    }
}
