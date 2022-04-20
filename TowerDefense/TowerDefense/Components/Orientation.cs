using Microsoft.Xna.Framework;
using System;
using Systems;

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

        public float radianGoal;

        public float radiansTurnSpeed
        {
            get { return degreeTurnSpeed * (float)(Math.PI / 180); }
            set { degreeTurnSpeed = value * (float)(180 / Math.PI); }
        }

        private const float CLOSE_ENOUGH_ANGLE = 5f;
        public bool rotateToGoal(GameTime gameTime)
        {
            float prevAngle = radians;
            float curAngle = radians;
            float goalAngle = radianGoal;

            if (Math.Abs(curAngle - goalAngle) > (CLOSE_ENOUGH_ANGLE * (float)(Math.PI / 180)))
            {
                radianGoal = goalAngle;
                float degreesToMove = degreeTurnSpeed * gameTime.ElapsedGameTime.Milliseconds;

                //which direction to turn, based on cross product
                if (CoordinateSystem.crossProduct(curAngle, goalAngle) < 0)
                {
                    degreesToMove *= -1;
                }

                curAngle += degreesToMove;

                //if you overshot the angle
                if ((CoordinateSystem.crossProduct(curAngle, goalAngle) < 0) != (CoordinateSystem.crossProduct(prevAngle, goalAngle) < 0))
                {
                    curAngle = goalAngle;
                }

                radians = curAngle;
            }
            
            if (Math.Abs(curAngle - goalAngle) <= (CLOSE_ENOUGH_ANGLE * (float)(Math.PI / 180)))
            {
                return true;
            }
            return false;
        }

        public void rotate(GameTime gameTime)
        {
            float prevAngle = radians;
            float curAngle = radians;
            float goalAngle = radians + (2f* (float)(Math.PI / 180));

            float degreesToMove = degreeTurnSpeed * gameTime.ElapsedGameTime.Milliseconds;
            curAngle += degreesToMove;

            //if you overshot the angle
            if ((CoordinateSystem.crossProduct(curAngle, goalAngle) < 0) != (CoordinateSystem.crossProduct(prevAngle, goalAngle) < 0))
            {
                curAngle = goalAngle;
            }

            radians = curAngle;
        }

        public Orientation(float  degrees = 90, float degreeTurnSpeed = 15/1000f)
        {
            this.degrees = degrees;
            this.degreeTurnSpeed = degreeTurnSpeed;

            this.radianGoal = this.radians;
        }
    }
}
