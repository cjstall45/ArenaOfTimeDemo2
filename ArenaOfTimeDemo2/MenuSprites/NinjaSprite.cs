using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaOfTimeDemo2.MenuSprites
{
    public class NinjaSprite
    {
        private const float ANIMATION_SPEED = 0.2f;

        private Texture2D texture;

        private double animationTimer;

        private int animationFrame;

        public Vector2 Position = new Vector2(450, 280);

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Idle");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }

            var source = new Rectangle(animationFrame * 200, 0, 200, 200);
            spriteBatch.Draw(texture, Position, source, Color.White, 0, new Vector2(64, 64), 2.0f, SpriteEffects.None, 0);
        }
    }
}
