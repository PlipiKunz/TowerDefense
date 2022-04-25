
namespace Components
{
    public enum TargetType
    {
        Ground,
        Air,
        Both
    }

    public enum towerClass { 
        projectile, 
        bomb, 
        missle,
        other
    }

    public class TowerComponent : Component
    {
        public uint range;
        public uint level;

        public bulletType bulletType;
        public TargetType targetType;
        public Entities.Entity target;
        public uint fireInterval { get; set; }
        public uint elapsedInterval;
        public towerClass tc;
        public TowerComponent(uint range,  uint fireInterval, TargetType type, bulletType bulletType, towerClass tc)
        {
            this.range = range;
            this.fireInterval = fireInterval;
            this.elapsedInterval = (uint)(fireInterval * 1.5);
            this.targetType = type;
            this.bulletType = bulletType;

            this.level = (uint)level;
            this.target = null;
            this.tc = tc;
        }
    }
}
