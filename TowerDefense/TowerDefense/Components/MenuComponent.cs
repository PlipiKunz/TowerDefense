
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Systems;

namespace Components
{
    class MenuComponent : Component
    {
        public SpriteFont font;
        public string text;
        public towerClass towerClass;
        public Color c;
        public MenuComponent(SpriteFont sf, string t, Color c, towerClass tc = towerClass.projectile)
        {
            font = sf;
            text = t;
            towerClass = tc;
            this.c = c;
        }

        public Vector2 measure() {
            var pixDimensions = font.MeasureString(text);
            return CoordinateSystem.convertPixToGameScalar((int)pixDimensions.X, (int)pixDimensions.Y);
        }

    }
}
