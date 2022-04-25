using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

using System.Windows.Input;

namespace Systems
{
    public class MouseHandeler : System
    {
        public MouseHandeler()
            : base(typeof(Components.Mouse))
        {
        }

        private ButtonState prevState = ButtonState.Released;

        public bool mouseSet = false;

        /// <summary>
        /// Handel mouse system data
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Entity mouse = findMouse(m_entities);
            if (mouse != null)
            {
                MouseState mouseState = Mouse.GetState();
                if (!mouseSet)
                {
                    MouseCursor mouseCursor = MouseCursor.FromTexture2D(Renderer.blank, 0, 0);
                    Mouse.SetCursor(mouseCursor);
                    mouseSet = true;
                }

                updateMousePosition(gameTime,mouse, mouseState);
                updateMouseClick(gameTime,mouse, mouseState);
            }
        }

        private void updateMousePosition(GameTime gameTime, Entity mouse, MouseState mouseState) {
            int mousePixX = mouseState.X;
            int mousePixY = mouseState.Y;
            Vector2 mousePos = CoordinateSystem.convertPixToGame(mousePixX, mousePixY);

            var mouseEntityPos = mouse.GetComponent<Components.Position>();
            mouseEntityPos.x = mousePos.X;
            mouseEntityPos.y = mousePos.Y;
        }

        private void updateMouseClick(GameTime gameTime, Entity mouse, MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && prevState == ButtonState.Released)
            {
                SelectionSystem.Instance().click(mouse);
            }

            prevState = mouseState.LeftButton;
        }


        /// <summary>
        /// Returns a collection of all the mouse entities.
        /// </summary>
        private Entity findMouse(Dictionary<uint, Entity> entities)
        {

            foreach (var entity in m_entities.Values)
            {
                if (entity.ContainsComponent<Components.Mouse>() && entity.ContainsComponent<Components.Position>())
                {
                    return entity;
                }
            }

            return null;
        }
    }
}
