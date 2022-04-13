using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Entities
{
    public class SimpleCreep
    {

        private const float MOVE_AMOUNT = 1f/1000; // in game units to move each millisecond
        private const int STANDARD_COST = 10;
        private const int STANDARD_HEALTH = 10;

        public static Entity create(Texture2D square, int x, int y, Vector2 v)
        {
            var creep = new Entity();

            creep.Add(new Components.Sprite(square, Color.Green, Color.Black));

            creep.Add(new Components.Position(x, y, .5f, .75f));
            creep.Add(new Components.Orientation(90));
             
            creep.Add(new Components.PathMovable(MOVE_AMOUNT, v));
            creep.Add(new Components.Cost(STANDARD_COST));
            creep.Add(new Components.Health(STANDARD_HEALTH));
            creep.Add(new Components.CreepComponent());
            
            return creep;
        }
    }
}