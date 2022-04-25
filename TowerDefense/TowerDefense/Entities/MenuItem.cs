using Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Entities
{
    public class MenuItem
    {
        static SpriteFont font;
        static Color color = Color.White;
        public static void init(ContentManager content)
        {
             font = content.Load<SpriteFont>("Fonts/menu");
        }

        public static Entity createSimpleMenuItem(float x, float y, string text)
        {
            var item = new Entity();
            item.Add(new Components.Drawable());
            item.Add(new Components.MenuComponent(font, text, color));


            var menuComp = item.GetComponent<MenuComponent>();
            var measurement = menuComp.measure();
            item.Add(new Components.Position(x, y, measurement.X, measurement.Y));
            return item;
        }

        public static Entity createActionMenuItem(float x, float y, string text, towerClass action)
        {
            var item = new Entity();
            item.Add(new Components.Drawable());
            item.Add(new Components.MenuComponent(font, text, color,  action));

            var menuComp = item.GetComponent<MenuComponent>();
            var measurement = menuComp.measure();

            item.Add(new Components.Position(x, y, measurement.X, measurement.Y));

            item.Add(new Components.Selectable());
            return item;
        }


    }
}