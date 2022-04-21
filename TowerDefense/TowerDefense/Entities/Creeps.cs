using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private const int STANDARD_DAMAGE = 1;

        static Texture2D creepSprite;
        public static void init(ContentManager content)
        {
            creepSprite = content.Load<Texture2D>("Sprites/SquareSprite");
        }

        public static Entity createSimpleGround(int x, int y, Vector2 v)
        {
            var creep = new Entity();

            creep.Add(new Components.Sprite(creepSprite, Color.Green, Color.Black, rotatable:true));

            creep.Add(new Components.Position(x, y, .5f, .75f));
            creep.Add(new Components.Orientation(90));
             
            creep.Add(new Components.PathMovable(MOVE_AMOUNT, v));
            creep.Add(new Components.Cost(STANDARD_COST));
            creep.Add(new Components.Health(STANDARD_HEALTH));
            creep.Add(new Components.Damage(STANDARD_DAMAGE));
            creep.Add(new Components.CreepComponent(Components.TargetType.Ground));
            
            return creep;
        }

        public static Entity createSimpleFly(int x, int y, Vector2 v)
        {
            var creep = new Entity();

            creep.Add(new Components.Sprite(creepSprite, Color.CadetBlue, Color.Black, rotatable: true, priority: .6f));

            creep.Add(new Components.Position(x, y, .5f, .75f));
            creep.Add(new Components.Orientation(90));

            creep.Add(new Components.PathMovable(MOVE_AMOUNT, v));
            creep.Add(new Components.Cost(STANDARD_COST));
            creep.Add(new Components.Health(STANDARD_HEALTH));
            creep.Add(new Components.Damage(STANDARD_DAMAGE));
            creep.Add(new Components.CreepComponent(Components.TargetType.Air));

            return creep;
        }
    }
}