using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Entities
{
    public class SimpleTower
    {
        private const int FIRE_INTERVAL = 2000; // millisecond interval between firing
        private const float TURN_SPEED = 15 / 1000f; // degrees per milliseconds
        private const int STANDARD_RANGE = 2; // position units
        private const int STANDARD_COST = 100;

        static Texture2D towerSprite;
        public static void init(ContentManager content)
        {
             towerSprite = content.Load<Texture2D>("Sprites/TowerSprite");
        }

        public static Entity create(int x, int y)
        {
            var tower = new Entity();

            tower.Add(new Components.Sprite(towerSprite, Color.White, Color.Black, rotatable:true));

            tower.Add(new Components.Position(x, y));
            tower.Add(new Components.Orientation(degreeTurnSpeed:TURN_SPEED));
             
            tower.Add(new Components.Selectable());
            tower.Add(new Components.Cost(STANDARD_COST));
            tower.Add(new Components.TowerComponent(STANDARD_RANGE, FIRE_INTERVAL, Components.TargetType.Ground));

            return tower;
        }
    }
}