using Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Entities
{
    public class Tower
    {
        private const int FIRE_INTERVAL = 1000; // millisecond interval between firing
        private const float TURN_SPEED = 15 / 1000f; // degrees per milliseconds
        public static uint STANDARD_RANGE = 2; // position units
        public static uint STANDARD_COST = 25;
        public static uint COMPLEX_COST = 100;
        static Texture2D towerSprite;
        public static void init(ContentManager content)
        {
             towerSprite = content.Load<Texture2D>("Sprites/TowerSprite");
        }

        public static Entity createSimpleTower(int x, int y, int level=0)
        {
            var tower = new Entity();

            tower.Add(new Components.Sprite(towerSprite, Color.White, Color.Black, rotatable:true));
            tower.Add(new Components.Drawable());
            tower.Add(new Components.Position(x, y));
            tower.Add(new Components.Orientation(degreeTurnSpeed:TURN_SPEED));
             
            tower.Add(new Components.Selectable());
            tower.Add(new Components.Cost(STANDARD_COST));
            tower.Add(new Components.TowerComponent(STANDARD_RANGE, FIRE_INTERVAL/2, Components.TargetType.Ground, bulletType.projectile, towerClass.Projectile, level));

            return tower;
        }

        public static Entity createBombTower(int x, int y, int level=0)
        {
            var tower = new Entity();

            tower.Add(new Components.Drawable());
            tower.Add(new Components.Sprite(towerSprite, Color.Red, Color.Black, rotatable: true));

            tower.Add(new Components.Position(x, y));
            tower.Add(new Components.Orientation(degreeTurnSpeed: TURN_SPEED));

            tower.Add(new Components.Selectable());
            tower.Add(new Components.Cost(COMPLEX_COST));
            tower.Add(new Components.TowerComponent(STANDARD_RANGE+1, FIRE_INTERVAL, Components.TargetType.Ground, bulletType.bomb, towerClass.Bomb, level));

            return tower;
        }

        public static Entity createMissleTower(int x, int y, int level=0)
        {
            var tower = new Entity();
            tower.Add(new Components.Drawable());
            tower.Add(new Components.Sprite(towerSprite, Color.Blue, Color.Black, rotatable: true));

            tower.Add(new Components.Position(x, y));
            tower.Add(new Components.Orientation(degreeTurnSpeed: TURN_SPEED));

            tower.Add(new Components.Selectable());
            tower.Add(new Components.Cost(COMPLEX_COST));  
            tower.Add(new Components.TowerComponent(STANDARD_RANGE+1, FIRE_INTERVAL, Components.TargetType.Air, bulletType.missle, towerClass.Missle, level));

            return tower;
        }

        public static Entity createHomingTower(int x, int y, int level = 0)
        {
            var tower = new Entity();

            tower.Add(new Components.Sprite(towerSprite, Color.Yellow, Color.Black, rotatable: true));
            tower.Add(new Components.Drawable());
            tower.Add(new Components.Position(x, y));
            tower.Add(new Components.Orientation(degreeTurnSpeed: TURN_SPEED));

            tower.Add(new Components.Selectable());
            tower.Add(new Components.Cost(COMPLEX_COST));
            tower.Add(new Components.TowerComponent(STANDARD_RANGE, FIRE_INTERVAL / 4, Components.TargetType.Both, bulletType.projectile, towerClass.Other, level));

            return tower;
        }
    }
}