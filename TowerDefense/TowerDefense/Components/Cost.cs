
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Systems;

namespace Components
{
    class Cost : Component
    {
        public uint cost;

        public Cost(uint cost)
        {
            this.cost = cost;
        }
    }
}
