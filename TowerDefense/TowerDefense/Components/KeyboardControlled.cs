
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Systems;

namespace Components
{
    class KeyboardControlled : Component
    {
        public Dictionary<Keys, KeyboardActions> keys;

        public KeyboardControlled(Dictionary<Keys, KeyboardActions> keys)
        {
            this.keys = keys;
        }
    }
}
