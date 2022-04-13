
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
        public TargetType type;
        public uint fireInterval { get; set; }
        public uint elapsedInterval = 0;

        public TowerComponent(uint range,  uint moveInterval, TargetType type)
        {
            this.range = range;
            this.fireInterval = moveInterval;
            this.type = type;
            this.level = 0;
        }
    }
}
