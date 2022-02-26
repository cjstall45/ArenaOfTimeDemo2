using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArenaOfTimeDemo2.Collisions
{
    /// <summary>
    /// a class used to help calculate collisions between bounding rectangles
    /// </summary>
    public static class CollisionHelper
    {
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom || a.Bottom < b.Top);
        }
    }
}
