using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Systems
{
    class CoordinateSystem : System
    {
        static CoordinateSystem instance;
        private static object lockObj = new object();
        public static CoordinateSystem Instance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = new CoordinateSystem();
                }
            }

            return instance;
        }
        public static void reset()
        {
            lock (lockObj)
            {
                instance = new CoordinateSystem();
            }
        }

        public static int SWIDTH { get; protected set; }
        public static int SHEIGHT { get; protected set; }
        public static int GRID_SIZE { get; protected set; }
        public static int CELL_SIZE { get; protected set; }
        public static int OFFSET_X { get; protected set; }
        public static int OFFSET_Y { get; protected set; }

        protected CoordinateSystem() :
            base()
        {
        }

        public void initialize(int screen_width, int screen_height, int gridSize)
        {
            SWIDTH = screen_width;
            SHEIGHT = screen_height;

            GRID_SIZE = gridSize;
            CELL_SIZE = screen_height / gridSize;
            OFFSET_X = (screen_width - (gridSize * CELL_SIZE)) / 2;
            OFFSET_Y = (screen_height - (gridSize * CELL_SIZE)) / 2;
        }

        public override void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// converts from game coords to pixel coords
        /// </summary>
        public static Rectangle convertGameToPix(float x, float y, float w, float h) {

            Rectangle area = new Rectangle();
            area.X = OFFSET_X + (int)(x * CELL_SIZE);
            area.Y = OFFSET_Y + (int)(y * CELL_SIZE);
            area.Width = (int)(w * CELL_SIZE);
            area.Height = (int)(h * CELL_SIZE);
            return area;
        }
        
        /// <summary>
        /// converts from pixel coords to game cords
        /// </summary>
        public static Vector2 convertPixToGame(int x, int y) { 
            Vector2 area = new Vector2();

            //out of bounds check
            if (x < OFFSET_X) {
                area.X = 0;
            }
            else if (x > (OFFSET_X + (GRID_SIZE * CELL_SIZE))){

                area.X = GRID_SIZE;
            }
            else {
                area.X = (float)(x - OFFSET_X) / (float)CELL_SIZE;
            }

            //out of bounds check
            if (y < OFFSET_Y)
            {
                area.Y = 0;
            }
            else if (y > (OFFSET_Y + (GRID_SIZE * CELL_SIZE))) {
                area.Y = GRID_SIZE;
            }
            else
            {
                area.Y = (float)(y - OFFSET_Y) / (float)CELL_SIZE;
            }

            return area;
        }

        public static bool inGameBounds(int pix_x, int pix_y) {
            if ((pix_x < OFFSET_X) || (pix_x > (OFFSET_X + (GRID_SIZE * CELL_SIZE))) || (pix_y < OFFSET_Y) || (pix_y > (OFFSET_Y + (GRID_SIZE * CELL_SIZE))))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// if 2 entities collide
        /// </summary>
        public static bool collides(Entity a, Entity b)
        {
            var aPosition = a.GetComponent<Components.Position>();
            var bPosition = b.GetComponent<Components.Position>();


            Rectangle aRect = convertGameToPix(aPosition.x, aPosition.y, aPosition.w, aPosition.h);
            Rectangle bRect = convertGameToPix(bPosition.x, bPosition.y, bPosition.w, bPosition.h);

            //
            // A movable cannot collide with itself
            if (a == b)
            {
                return false;
            }

            return aRect.Intersects(bRect);
        }

        /// <summary>
        /// Returns a collection of all the tower entities.
        /// </summary>
        public  List<Entity> findTowers()
        {
            var towers = new List<Entity>();

            foreach (var entity in m_entities.Values)
            {
                if (entity.ContainsComponent<Components.TowerComponent>() && entity.ContainsComponent<Components.Position>())
                {
                    towers.Add(entity);
                }
            }

            return towers;
        }

        /// <summary>
        /// Returns a collection of all the creep entities.
        /// </summary>
        public  List<Entity> findCreeps()
        {
            var creep = new List<Entity>();

            foreach (var entity in m_entities.Values)
            {
                if (entity.ContainsComponent<Components.CreepComponent>())
                {
                    creep.Add(entity);
                }
            }

            return creep;
        }

        /// <summary>
        /// distance between 2 points
        /// </summary>
        public static int distance(Vector2 pointA, Vector2 pointB)
        {
            return (int)Math.Sqrt(Math.Pow((pointA.X - pointB.X), 2) + Math.Pow((pointA.Y - pointB.Y), 2));
        }

        /// <summary>
        /// angle between 2 points
        /// </summary>
        public static float angle(Vector2 originPoint, Vector2 goalPoint) {
            return (float)Math.Atan2(goalPoint.Y - originPoint.Y, goalPoint.X - originPoint.X);
        }

        /// <summary>
        /// cross product of 2 radian angles
        /// </summary>
        public static float crossProduct(float angleA, float angleB)
        {
            Vector2 vA = new Vector2((float)Math.Cos(angleA), (float)Math.Sin(angleA));
            Vector2 vB = new Vector2((float)Math.Cos(angleB), (float)Math.Sin(angleB));

            return (float)((vA.X * vB.Y) - (vA.Y * vB.X));

        }

    }
}
