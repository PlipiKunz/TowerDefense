using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Entities
{
    public class Creeps
    {
        private const float MOVE_AMOUNT = 1f/1000; // in game units to move each millisecond
        private const int STANDARD_COST = 10;
        private const int STANDARD_HEALTH = 5;
        private const int STANDARD_DAMAGE = 1;

        static Texture2D normalCreep;
        static Texture2D flyCreep;
        static Texture2D fastCreep;
        public static void init(ContentManager content)
        {
            normalCreep = content.Load<Texture2D>("Sprites/NormalCreep");
            flyCreep = content.Load<Texture2D>("Sprites/FlyingCreep");
            fastCreep = content.Load<Texture2D>("Sprites/FastCreep");
        }

        private static float scale(int level)
        {
            return 1 + (.25f * (level - 1));
        }

        public static Entity createSimpleGround(float x, float y, Vector2 goal, int level)
        {
            float scaleAmount = scale(level);

            var creep = new Entity();

            creep.Add(new Components.Drawable());
            creep.Add(new Components.AnimatedSprite(normalCreep, 3, 25, Color.White, Color.Black, rotatable:true));

            creep.Add(new Components.Position(x, y, .75f, .75f));
            creep.Add(new Components.Orientation(90));
             
            creep.Add(new Components.PathMovable(MOVE_AMOUNT * scaleAmount , goal));
            creep.Add(new Components.Cost((uint)(STANDARD_COST * scaleAmount)));
            creep.Add(new Components.Health((int)(STANDARD_HEALTH * scaleAmount)));

            creep.Add(new Components.Damage(STANDARD_DAMAGE));
            creep.Add(new Components.CreepComponent(Components.TargetType.Ground));
            
            return creep;
        }

        public static Entity createFastGround(float x, float y, Vector2 goal, int level)
        {
            float scaleAmount = scale(level);

            var creep = new Entity();

            creep.Add(new Components.Drawable());
            creep.Add(new Components.AnimatedSprite(fastCreep, 3, 25, Color.White, Color.Black, rotatable: true));

            creep.Add(new Components.Position(x, y, .6f, .6f));
            creep.Add(new Components.Orientation(90));

            creep.Add(new Components.PathMovable(MOVE_AMOUNT * scaleAmount * 1.25f, goal));
            creep.Add(new Components.Cost((uint)(STANDARD_COST * scaleAmount)));
            creep.Add(new Components.Health((int)(STANDARD_HEALTH * scaleAmount)));

            creep.Add(new Components.Damage(STANDARD_DAMAGE));
            creep.Add(new Components.CreepComponent(Components.TargetType.Ground));

            return creep;
        }

        public static Entity createSimpleFly(float x, float y, Vector2 goal, int level)
        {
            float scaleAmount = scale(level);
            var creep = new Entity();

            creep.Add(new Components.Drawable());
            creep.Add(new Components.AnimatedSprite(flyCreep, 3, 25, Color.White, Color.Black, rotatable: true));

            creep.Add(new Components.Position(x, y, .75f, .75f));
            creep.Add(new Components.Orientation(90));

            creep.Add(new Components.PathMovable(MOVE_AMOUNT * scaleAmount, goal));
            creep.Add(new Components.Cost((uint)(STANDARD_COST * scaleAmount)));
            creep.Add(new Components.Health((int)(STANDARD_HEALTH * scaleAmount)));

            creep.Add(new Components.Damage(STANDARD_DAMAGE));
            creep.Add(new Components.CreepComponent(Components.TargetType.Air));

            return creep;
        }
    }
}