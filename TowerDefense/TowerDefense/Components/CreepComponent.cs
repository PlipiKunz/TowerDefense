
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Systems;

namespace Components
{
    class CreepComponent : Component
    {
        public TargetType creepType;
        public CreepComponent(TargetType type)
        {
            this.creepType = type;
        }
    }
}
