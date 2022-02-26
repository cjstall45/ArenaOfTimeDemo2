using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaOfTimeDemo2.MenuSprites
{
    public class VikingSprite
    {
        private const float ANIMATION_SPEED = 0.1f;

        private Texture2D[] texture = new Texture2D[6];

        private double animationTimer;

        private int animationFrame;

        public Vector2 Position = new Vector2(300, 495);

        public void LoadContent(ContentManager content)
        {
            texture[0] = content.Load<Texture2D>("ready_1");
            texture[1] = content.Load<Texture2D>("ready_2");
            texture[2] = content.Load<Texture2D>("ready_3");
            texture[3] = content.Load<Texture2D>("ready_4");
            texture[4] = content.Load<Texture2D>("ready_5");
            texture[5] = content.Load<Texture2D>("ready_6");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 5) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }

            spriteBatch.Draw(texture[animationFrame], Position, null, Color.White, 0, new Vector2(64, 64), 3.8f, SpriteEffects.None, 0);
        }
    }
}
