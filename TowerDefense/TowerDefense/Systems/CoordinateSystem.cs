using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Systems
{
    

    class CoordinateSystem : System
    {
        public static int GRID_SIZE { get; protected set;}
        public static int CELL_SIZE { get; protected set;}
        public static int OFFSET_X { get; protected set;}
        public static int OFFSET_Y { get; protected set;}


        public CoordinateSystem( int screen_width, int screen_height, int gridSize) :
            base()
        {
            GRID_SIZE = gridSize;
            CELL_SIZE = screen_height / gridSize;
            OFFSET_X = (screen_width - (gridSize * CELL_SIZE)) / 2;
            OFFSET_Y = (screen_height - (gridSize * CELL_SIZE)) / 2;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public static Rectangle convertGameToPix(float x, float y, float w, float h) {

            Rectangle area = new Rectangle();
            area.X = OFFSET_X + (int)(x * CELL_SIZE);
            area.Y = OFFSET_Y + (int)(y * CELL_SIZE);
            area.Width = (int)(w * CELL_SIZE);
            area.Height = (int)(h * CELL_SIZE);
            return area;
        }

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


        public bool collides(Entity a, Entity b)
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
        public static List<Entity> findTowers(Dictionary<uint, Entity> entities)
        {
            var towers = new List<Entity>();

            foreach (var entity in entities.Values)
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
        public static List<Entity> findCreeps(Dictionary<uint, Entity> entities)
        {
            var creep = new List<Entity>();

            foreach (var entity in entities.Values)
            {
                if (entity.ContainsComponent<Components.CreepComponent>())
                {
                    creep.Add(entity);
                }
            }

            return creep;
        }
    }
}
