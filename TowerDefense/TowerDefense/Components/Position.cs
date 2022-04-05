using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Components
{
    public class Position : Component
    {
        
        public int x;
        public int y;
        public int w;
        public int h;
        public Rectangle r { get { return new Rectangle(x,y,w,h); } }

        public Position(int x, int y, int w = 1, int h = 1)
        {
            this.x = x; 
            this.y = y;
            this.w = w;
            this.h = h;
        }
    }
}
