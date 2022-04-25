using CS5410.Persistence;
using Microsoft.Xna.Framework;
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


        bool prevUpgrade = false;
        bool prevSell = false;
        bool prevNext = false;
        public override void Update(GameTime gameTime)
        {
            foreach (var entry in KeyboardPersistence.actionToKey)
            {
                var KeyPress = entry.Value;
                var KeyAction = entry.Key;

                bool press = Keyboard.GetState().IsKeyDown(KeyPress);

                if (KeyAction == KeyboardActions.Upgrade)
                {
                    if (press && !prevUpgrade)
                    {

                    }
                    prevUpgrade = press;
                }
                else if (KeyAction == KeyboardActions.Sell)
                {
                    if (press && !prevSell) { 
                    
                    }
                    prevSell = press;
                }
                else
                {
                    if (press && !prevNext && !LevelSystem.inLevel)
                    {
                        LevelSystem.Instance().nextLevel();
                    }
                    prevNext = press;
                }

            }
        }
    }
}
