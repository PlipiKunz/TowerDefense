using System;

namespace Components
{
    public class Orientation : Component
    {
        private float m_degrees;

        public float degrees {
            get { return m_degrees % 360; }
            set { m_degrees = value % 360; }
        }

        //degrees per millisecond to turn
        public float degreeTurnSpeed;

        public float radians { 
            get { return (m_degrees * (float)(Math.PI/180)); } 
            set { m_degrees = (value * (float)(180 / Math.PI)) ; } 
        }

        public float radiansTurnSpeed
        {
            get { return degreeTurnSpeed * (float)(Math.PI / 180); }
            set { degreeTurnSpeed = value * (float)(180 / Math.PI); }
        }


        public Orientation(float  degrees = 90, float degreeTurnSpeed = 0)
        {
            this.degrees = degrees;
            this.degreeTurnSpeed = degreeTurnSpeed;
        }
    }
}
