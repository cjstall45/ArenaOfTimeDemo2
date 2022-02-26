using ArenaOfTimeDemo2.Collisions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaOfTimeDemo2.Collisions
{
    /// <summary>
    /// a class added to create hitboxes for both attacks and hurtboxes 
    /// </summary>
    public class Hitbox
    {
        public bool Active = true;

        public BoundingRectangle Bounds;

        public Hitbox(float x, float y, float width, float height)
        {
            Bounds = new BoundingRectangle(x, y, width, height);
        }

        public Hitbox(Vector2 position, float width, float height)
        {
            Bounds = new BoundingRectangle(position, width, height);
        }
    }
}
