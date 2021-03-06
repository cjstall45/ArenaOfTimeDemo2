using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ArenaOfTimeDemo2.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using ArenaOfTimeDemo2.Fighters;

namespace ArenaOfTimeDemo2.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;
        private Texture2D HealthBarShell;
        private Texture2D HealthBar;
        private Texture2D background;
        private Song forestMusic;

        private FighterSprite Fighter1;
        private FighterSprite Fighter2;
        private Characters player1;
        private Characters player2;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public GameplayScreen(Characters player1, Characters player2)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
            this.player1 = player1;
            this.player2 = player2;
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            

            _gameFont = _content.Load<SpriteFont>("retroGaming");
            if(player1 == Characters.Viking)
            {
                Fighter1 = new VikingSprite(1, ScreenManager.GraphicsDevice.Viewport.Width);
            }
            else
            {
                Fighter1 = new NinjaSprite(1, ScreenManager.GraphicsDevice.Viewport.Width);
            }
            if (player2 == Characters.Viking)
            {
                Fighter2 = new VikingSprite(2, ScreenManager.GraphicsDevice.Viewport.Width);
            }
            else
            {
                Fighter2 = new NinjaSprite(2, ScreenManager.GraphicsDevice.Viewport.Width);
            }
           
            Fighter1.LoadContent(_content);
            Fighter2.LoadContent(_content);
            HealthBarShell = _content.Load<Texture2D>("emptyHealthBar");
            HealthBar = _content.Load<Texture2D>("HealthBarCenter");
            background = _content.Load<Texture2D>("image without mist");
            forestMusic = _content.Load<Song>("Mjolnir+-+320bit");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(forestMusic);


            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            
            var gamePadState1 = input.CurrentGamePadStates[1];
            var gamePadState2 = input.CurrentGamePadStates[2];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected1 = !gamePadState1.IsConnected && input.GamePadWasConnected[1];
            bool gamePadDisconnected2 = !gamePadState2.IsConnected && input.GamePadWasConnected[2];

            if (Fighter1.HealthPercent == 0 || Fighter2.HealthPercent == 0)
            {
                if (Fighter1.dead || Fighter2.dead)
                {
                    if (player1 == Characters.Viking)
                    {
                        Fighter1 = new VikingSprite(1, ScreenManager.GraphicsDevice.Viewport.Width);
                    }
                    else
                    {
                        Fighter1 = new NinjaSprite(1, ScreenManager.GraphicsDevice.Viewport.Width);
                    }
                    if (player2 == Characters.Viking)
                    {
                        Fighter2 = new VikingSprite(2, ScreenManager.GraphicsDevice.Viewport.Width);
                    }
                    else
                    {
                        Fighter2 = new NinjaSprite(2, ScreenManager.GraphicsDevice.Viewport.Width);
                    }
                    Fighter1.LoadContent(_content);
                    Fighter2.LoadContent(_content);
                }
            }

            PlayerIndex player;
            if (_pauseAction.Occurred(input, PlayerIndex.One, out player) || gamePadDisconnected1)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), PlayerIndex.One);
            }
            else if(_pauseAction.Occurred(input, PlayerIndex.Two, out player) || gamePadDisconnected2)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), PlayerIndex.Two);
            }
            else
            {
                Fighter1.Update(gameTime, input);
                Fighter2.Update(gameTime, input);
                //check collisions
                if (Fighter1.Hurtbox.Bounds.CollidesWith(Fighter2.Hurtbox.Bounds))
                {
                    Fighter1.CollidingRight = true;
                    Fighter2.CollidingLeft = true;
                }
                else
                {
                    Fighter1.CollidingRight = false;
                    Fighter2.CollidingLeft = false;
                }

                if (Fighter1.Hurtbox.Bounds.Left < 0 || Fighter1.Hurtbox.Bounds.Right > ScreenManager.GraphicsDevice.Viewport.Width )
                {
                    Fighter1.CollidingLeft = true;
                }
                else
                {
                    Fighter1.CollidingLeft = false;
                }

                if (Fighter2.Hurtbox.Bounds.Left < 0 || Fighter2.Hurtbox.Bounds.Right > ScreenManager.GraphicsDevice.Viewport.Width )
                {
                    Fighter2.CollidingRight = true;
                }
                else
                {
                    Fighter2.CollidingRight = false;
                }

                if (Fighter2.Attack1Hitbox.Active && Fighter1.Hurtbox.Bounds.CollidesWith(Fighter2.Attack1Hitbox.Bounds))
                {
                    if (Fighter1.Hurtbox.Active)
                    {
                        Fighter1.Hit(player2);
                    }
                }
                if (Fighter1.Attack1Hitbox.Active && Fighter2.Hurtbox.Bounds.CollidesWith(Fighter1.Attack1Hitbox.Bounds))
                {
                    if (Fighter2.Hurtbox.Active)
                    {
                        Fighter2.Hit(player1);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            var spriteBatch = ScreenManager.SpriteBatch;
            var viewport = ScreenManager.GraphicsDevice.Viewport;
            var viewportSize = new Vector2(viewport.Width, viewport.Height);
            var textSize = _gameFont.MeasureString("Player 2 wins!");
            var textPosition = (viewportSize - textSize) / 2;

            spriteBatch.Begin(SpriteSortMode.BackToFront);

            Fighter1.Draw(gameTime, spriteBatch);
            Fighter2.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(HealthBarShell, new Vector2(20, 5), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(HealthBar, new Vector2(23, 9), new Rectangle(0, 0, (int)(274 * Fighter1.HealthPercent), 27), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(HealthBarShell, new Vector2(viewport.Width - 300, 5), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(HealthBar, new Vector2(viewport.Width - 297, 9), new Rectangle(0, 0, (int)(274 * Fighter2.HealthPercent), 27), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(background, new Vector2(0, 30), null, Color.White, 0, Vector2.Zero, 1.3f, SpriteEffects.None, 1f);
            if (Fighter1.HealthPercent == 0)
            {
                spriteBatch.DrawString(_gameFont, "Player 2 wins!", textPosition, Color.Red);

            }
            if (Fighter2.HealthPercent == 0)
            {
                spriteBatch.DrawString(_gameFont, "Player 1 wins!", textPosition, Color.Red);
            }

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
