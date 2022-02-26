using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ArenaOfTimeDemo2.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
        private Fighters.VikingSprite vikingSprite1;
        private Fighters.VikingSprite vikingSprite2;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            

            _gameFont = _content.Load<SpriteFont>("retroGaming");
            vikingSprite1 = new Fighters.VikingSprite(1, ScreenManager.GraphicsDevice.Viewport.Width);
            vikingSprite2 = new Fighters.VikingSprite(2, ScreenManager.GraphicsDevice.Viewport.Width);
            vikingSprite1.LoadContent(_content);
            vikingSprite2.LoadContent(_content);
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
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            if (vikingSprite1.HealthPercent == 0 || vikingSprite2.HealthPercent == 0)
            {
                if (vikingSprite1.dead || vikingSprite2.dead)
                {
                    vikingSprite1 = new Fighters.VikingSprite(1, ScreenManager.GraphicsDevice.Viewport.Width);
                    vikingSprite2 = new Fighters.VikingSprite(2, ScreenManager.GraphicsDevice.Viewport.Width);
                    vikingSprite1.LoadContent(_content);
                    vikingSprite2.LoadContent(_content);
                }
            }

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                vikingSprite1.Update(gameTime);
                vikingSprite2.Update(gameTime);
                //check collisions
                if (vikingSprite1.Hurtbox.Bounds.CollidesWith(vikingSprite2.Hurtbox.Bounds))
                {
                    vikingSprite1.CollidingRight = true;
                    vikingSprite2.CollidingLeft = true;
                }
                else
                {
                    vikingSprite1.CollidingRight = false;
                    vikingSprite2.CollidingLeft = false;
                }

                if (vikingSprite1.Hurtbox.Bounds.Left < 20 || vikingSprite1.Hurtbox.Bounds.Right > ScreenManager.GraphicsDevice.Viewport.Width - 20)
                {
                    vikingSprite1.CollidingLeft = true;
                }
                else
                {
                    vikingSprite1.CollidingLeft = false;
                }

                if (vikingSprite2.Hurtbox.Bounds.Left < 20 || vikingSprite2.Hurtbox.Bounds.Right > ScreenManager.GraphicsDevice.Viewport.Width - 20)
                {
                    vikingSprite2.CollidingRight = true;
                }
                else
                {
                    vikingSprite2.CollidingRight = false;
                }

                if (vikingSprite2.Attack1Hitbox.Active && vikingSprite1.Hurtbox.Bounds.CollidesWith(vikingSprite2.Attack1Hitbox.Bounds))
                {
                    if (vikingSprite1.Hurtbox.Active)
                    {
                        vikingSprite1.Hit();
                    }
                }
                if (vikingSprite1.Attack1Hitbox.Active && vikingSprite2.Hurtbox.Bounds.CollidesWith(vikingSprite1.Attack1Hitbox.Bounds))
                {
                    if (vikingSprite2.Hurtbox.Active)
                    {
                        vikingSprite2.Hit();
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

            vikingSprite1.Draw(gameTime, spriteBatch);
            vikingSprite2.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(HealthBarShell, new Vector2(20, 5), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(HealthBar, new Vector2(23, 9), new Rectangle(0, 0, (int)(274 * vikingSprite1.HealthPercent), 27), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(HealthBarShell, new Vector2(viewport.Width - 300, 5), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(HealthBar, new Vector2(viewport.Width - 297, 9), new Rectangle(0, 0, (int)(274 * vikingSprite2.HealthPercent), 27), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(background, new Vector2(0, 30), null, Color.White, 0, Vector2.Zero, 1.3f, SpriteEffects.None, 1f);
            if (vikingSprite1.HealthPercent == 0)
            {
                spriteBatch.DrawString(_gameFont, "Player 2 wins!", textPosition, Color.Red);

            }
            if (vikingSprite2.HealthPercent == 0)
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
