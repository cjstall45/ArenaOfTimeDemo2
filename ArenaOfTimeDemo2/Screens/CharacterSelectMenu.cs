using ArenaOfTimeDemo2.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaOfTimeDemo2.Screens
{
    public class CharacterSelectMenu : GameScreen
    {
        private readonly InputAction _menuLeft;
        private readonly InputAction _menuRight;
        private readonly InputAction _menuSelect;
        private readonly InputAction _menuCancel;
        private TileMap _tileMap;
        private ContentManager _content;
        private bool player1Ready = false;
        private bool player2Ready = false;

        public CharacterSelectMenu()
        {
            _menuLeft = new InputAction(
                new[] { Buttons.DPadLeft, Buttons.LeftThumbstickLeft },
                new[] { Keys.A }, true);
            _menuRight = new InputAction(
                new[] { Buttons.DPadRight, Buttons.LeftThumbstickRight },
                new[] { Keys.D }, true);
            _menuSelect = new InputAction(
                new[] { Buttons.A, Buttons.Start },
                new[] { Keys.Enter, Keys.Space }, true);
            _menuCancel = new InputAction(
                new[] { Buttons.B, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);

            _tileMap = new TileMap("map.txt");
            TransitionOnTime = TimeSpan.FromSeconds(0);
            TransitionOffTime = TimeSpan.FromSeconds(20);
        }

        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _tileMap.LoadContent(_content);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;
            if (_menuLeft.Occurred(input, null, out playerIndex))
            {
                _tileMap.UpdatePosition(playerIndex, -1);
            }

            if (_menuRight.Occurred(input, null, out playerIndex))
            {
                _tileMap.UpdatePosition(playerIndex, 1);
            }

            if (_menuSelect.Occurred(input, null, out playerIndex))
            {
                if (playerIndex == PlayerIndex.One && _tileMap.player1Index == 0)
                {
                    player1Ready = true;
                }
                else if(playerIndex == PlayerIndex.Two && _tileMap.player2Index == 0)
                {
                    player2Ready = true;
                }
            }    
            else if (_menuCancel.Occurred(input, null, out playerIndex))
            {
                if (playerIndex == PlayerIndex.One)
                {
                    player1Ready = false;
                }
                else
                {
                    player2Ready = false;
                }
            }
            if(player1Ready && player2Ready) LoadingScreen.Load(ScreenManager, true, null, new GameplayScreen());
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            _tileMap.Draw(gameTime, ScreenManager.SpriteBatch);
            if(player1Ready) ScreenManager.SpriteBatch.DrawString(ScreenManager.Font,"Player1 Ready", new Vector2(60, 80),Color.Red, 0, Vector2.Zero, .75f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            if(player2Ready) ScreenManager.SpriteBatch.DrawString(ScreenManager.Font,"Player2 Ready", new Vector2(420, 80), Color.DarkBlue, 0, Vector2.Zero, .75f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Select your Warrior", new Vector2(120, 10), Color.White);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Viking", new Vector2(120, 140), Color.White, 0, Vector2.Zero, .325f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Coming Soon", new Vector2(300, 140), Color.White, 0, Vector2.Zero, .325f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Coming Soon", new Vector2(480, 140), Color.White, 0, Vector2.Zero, .325f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Coming Soon", new Vector2(660, 140), Color.White, 0, Vector2.Zero, .325f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.End();
        }

    }
}
