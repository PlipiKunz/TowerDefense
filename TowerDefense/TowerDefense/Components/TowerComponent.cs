
namespace Components
{
    public enum TargetType
    {
        Ground,
        Air,
        Both
    }

    public class TowerComponent : Component
    {
        public uint range;
        public uint level;

        public TargetType targetType;
        public Entities.Entity target;
        public uint fireInterval { get; set; }
        public uint elapsedInterval;

        public TowerComponent(uint range,  uint fireInterval, TargetType type)
        {
            this.range = range;
            this.fireInterval = fireInterval;
            this.elapsedInterval = (uint)(fireInterval * 1.5);
            this.targetType = type;
            this.level = 0;

            this.target = null;
        }
    }
}
