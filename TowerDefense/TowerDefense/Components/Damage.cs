
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Systems;

namespace Components
{
    class Damage : Component
    {
        public uint damage;

        public Damage(uint damage)
        {
            this.damage = damage;
        }
    }
}
