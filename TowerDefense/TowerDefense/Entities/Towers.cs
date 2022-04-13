﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Entities
{
    public class SimpleTower
    {
        private const int FIRE_INTERVAL = 150; // milliseconds
        private const int STANDARD_RANGE = 3; // position units
        private const int STANDARD_COST = 100;

        public static Entity create(Texture2D square, int x, int y)
        {
            var tower = new Entity();

            tower.Add(new Components.Sprite(square, Color.Red, Color.Black));

            tower.Add(new Components.Position(x, y));
            tower.Add(new Components.Orientation());
             
            tower.Add(new Components.Selectable());
            tower.Add(new Components.Cost(STANDARD_COST));
            tower.Add(new Components.TowerComponent(STANDARD_RANGE, FIRE_INTERVAL, Components.TargetType.Ground));


            return tower;
        }
    }
}