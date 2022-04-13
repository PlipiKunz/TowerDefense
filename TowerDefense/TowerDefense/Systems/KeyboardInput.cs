﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Systems
{

    public enum KeyboardActions
    {
        Upgrade,
        Sell,
        Next
    }

    /// <summary>
    /// This system knows how to accept keyboard input and use that
    /// to move an entity, based on the entities 'KeyboardControlled'
    /// component settings.
    /// </summary>
    class KeyboardInput : System
    {

        public KeyboardInput()
            : base(typeof(Components.KeyboardControlled))
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                var movable = entity.GetComponent<Components.PathMovable>();
                var input = entity.GetComponent<Components.KeyboardControlled>();

                
            }
        }
    }
}
