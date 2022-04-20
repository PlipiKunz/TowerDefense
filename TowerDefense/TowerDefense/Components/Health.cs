
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Systems;

namespace Components
{
    class Health : Component
    {
        public int max_health;
        public int health;

        public Health(int health)
        {
            this.health = health;
            this.max_health = health;
        }
    }
}
