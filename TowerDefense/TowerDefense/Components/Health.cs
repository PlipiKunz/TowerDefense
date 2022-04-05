
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Systems;

namespace Components
{
    class Health : Component
    {
        public uint health;

        public Health(uint health)
        {
            this.health = health;
        }
    }
}
