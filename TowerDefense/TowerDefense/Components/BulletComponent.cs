
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Systems;

namespace Components
{
    public enum bulletType { 
        projectile,
        bomb,
        missle
    }
    class BulletComponent : Component
    {
        public float moveAmount; //game units per milli-second
        public Entity target;
        public bulletType type;
        public Vector2 goalPos;

        public BulletComponent(Entity target, float moveAmount, bulletType type = bulletType.projectile, Vector2 goalPos = new Vector2())
        {
            this.target = target;
            this.moveAmount = moveAmount;
            this.type = type;
            this.goalPos = goalPos;
        }
    }
}
