
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Systems;

namespace Components
{
    class Bomb : Component
    {
        public int range;
        public TargetType targetType;

        public Bomb(int range, TargetType targetType)
        {
            this.range = range;
            this.targetType = targetType;
        }
    }
}
