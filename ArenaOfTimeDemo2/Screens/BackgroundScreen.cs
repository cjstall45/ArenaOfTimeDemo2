﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ArenaOfTimeDemo2.StateManagement;
using ArenaOfTimeDemo2.MenuSprites;

namespace ArenaOfTimeDemo2.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    public class BackgroundScreen : GameScreen
    {
        private ContentManager _content;
        private KnightSprite knightSprite;
        private NinjaSprite ninjaSprite;
        private PersianSprite persianSprite;
        private VikingSprite vikingSprite;



        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(5);
            TransitionOffTime = TimeSpan.FromSeconds(5);
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, whereas if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            knightSprite = new KnightSprite();
            ninjaSprite = new NinjaSprite();
            persianSprite = new PersianSprite();
            vikingSprite = new VikingSprite();
            knightSprite.LoadContent(_content);
            ninjaSprite.LoadContent(_content);
            persianSprite.LoadContent(_content);
            vikingSprite.LoadContent(_content);

        }

        public override void Unload()
        {
            _content.Unload();
        }

        // Unlike most screens, this should not transition off even if
        // it has been covered by another screen: it is supposed to be
        // covered, after all! This overload forces the coveredByOtherScreen
        // parameter to false in order to stop the base Update method wanting to transition off.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
           
           // Logo.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin();
            
            knightSprite.Draw(gameTime, spriteBatch);
            ninjaSprite.Draw(gameTime, spriteBatch);
            persianSprite.Draw(gameTime, spriteBatch);
            vikingSprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
