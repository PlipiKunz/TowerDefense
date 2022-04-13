using System;

namespace Components
{
    public class Orientation : Component
    {
        public float degrees;
        public float radians { 
            get { return degrees * (float)(Math.PI/180); } 
            set { degrees = value * (float)(180 / Math.PI); } 
        }

        public Orientation(float  degrees = 90)
        {
            this.degrees = degrees;
        }
    }
}
