using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Components
{
    public class Position : Component
    {
        
        public float x;
        public float y;
        public float w;
        public float h;

        public float CenterX { 
            get { return x + (w / 2); } 
        }
        public float centerY
        {
            get { return y + (h / 2); }
        }
        public Position(float x, float y, float w = 1, float h = 1)
        {
            this.x = x; 
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public void fixBounds() {

            if (x + w > Systems.CoordinateSystem.GRID_SIZE)
            {
                x = Systems.CoordinateSystem.GRID_SIZE - w;
            }
            if (y + h > Systems.CoordinateSystem.GRID_SIZE)
            {
                y = Systems.CoordinateSystem.GRID_SIZE - h;
            }       
        }
    }
}
