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
        public static void init(ContentManager content)
        {
            boxSprite = content.Load<Texture2D>("Sprites/SquareSprite");
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

        
    }
}