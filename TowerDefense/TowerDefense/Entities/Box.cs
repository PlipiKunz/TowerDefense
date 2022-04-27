using Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Systems;

namespace Entities
{
    public class box
    {
        static Texture2D boxSprite;
        static Texture2D circle;
        public static void init(ContentManager content)
        {
            boxSprite = content.Load<Texture2D>("Sprites/SquareSprite");
            circle = content.Load<Texture2D>("Sprites/Circle");
        }
        public static Entity createSimple(float x, float y, bool goalPos)
        {
            var box = new Entity();

            Color c = goalPos ? Color.Red : Color.Green;

            box.Add(new Components.Drawable());
            box.Add(new Components.Sprite(boxSprite,c,Color.Black, .01f));
            box.Add(new Components.Position(x, y, 1,1));

            return box;
        }

        public static Entity createCirc(float x, float y, int w, int h)
        {
            var circ = new Entity();

            circ.Add(new Components.Drawable());
            circ.Add(new Components.Sprite(circle, Color.White, Color.White, .45f));

            circ.Add(new Components.Position(x, y, w, h));
            var pos = circ.GetComponent<Components.Position>();
            pos.CenterX = x;
            pos.CenterY = y;

            return circ;
        }


    }
}