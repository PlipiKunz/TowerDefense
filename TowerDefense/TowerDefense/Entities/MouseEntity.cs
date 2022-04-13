using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Entities
{
    public class MouseEntity
    {

        public static Entity create(Texture2D square, int x, int y)
        {
            var mouse = new Entity();

            mouse.Add(new Components.Sprite(square, Color.White, Color.Black, 1f));
            mouse.Add(new Components.Position(x, y, .15f, .2f));
            mouse.Add(new Components.Mouse());

            return mouse;
        }
    }
}