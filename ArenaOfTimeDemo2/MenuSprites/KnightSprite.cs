using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaOfTimeDemo2.MenuSprites
{
    public class KnightSprite
    {
        private const float ANIMATION_SPEED = 0.1f;

        private Texture2D texture;

        private double animationTimer;

        private int animationFrame;

        public Vector2 Position = new Vector2(700, 360);

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("_Idle");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 9) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }

            var source = new Rectangle(animationFrame * 120, 0, 120, 80);
            spriteBatch.Draw(texture, Position, source, Color.White, 0, new Vector2(64, 64), 3.0f, SpriteEffects.None, 0);
        }

    }
}
