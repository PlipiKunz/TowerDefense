using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Entities
{
    public class MouseEntity
    {
        static Texture2D mouseSprite;
        public static void init(ContentManager content) {
            mouseSprite = content.Load<Texture2D>("Sprites/SquareSprite");
        }
        public static Entity create(int x, int y)
        {
            var mouse = new Entity();

            mouse.Add(new Components.Drawable());
            mouse.Add(new Components.Sprite(mouseSprite, Color.White, Color.Black, 1f));
            mouse.Add(new Components.Position(x, y, .15f, .2f));
            mouse.Add(new Components.Mouse());

            return mouse;
        }
    }
}