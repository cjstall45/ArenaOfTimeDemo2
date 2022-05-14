using ArenaOfTimeDemo2.Collisions;
using ArenaOfTimeDemo2.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaOfTimeDemo2.Fighters
{
    public interface FighterSprite
    {
        public float HealthPercent { get; set; }
        public Vector2 Position { get; set; }
        public Hitbox Hurtbox { get; set; }
        public Hitbox Attack1Hitbox { get; set; }
        public bool CollidingLeft { get; set; }
        public bool CollidingRight { get; set; }
        public bool dead { get; set; }
        public void Hit(Characters attack);
        public void LoadContent(ContentManager content);
        public void Update(GameTime gameTime, InputState input);
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
