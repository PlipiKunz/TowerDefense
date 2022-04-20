
using Entities;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Systems;

namespace Components
{
    public enum bulletType { 
        projectile,
        bomb
    }
    class BulletComponent : Component
    {
        public float moveAmount; //game units per milli-second
        public Entity target;
        public bulletType type;

        public BulletComponent(Entity target, float moveAmount, bulletType type = bulletType.projectile)
        {
            this.target = target;
            this.moveAmount = moveAmount;
            this.type = type;
        }
    }
}
