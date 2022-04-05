
namespace Components
{

    public class Movable : Component
    {
        public uint speed;
        public uint moveInterval { get; private set; }
        public uint elapsedInterval = 0;

        public Movable(uint speed,  uint moveInterval)
        {
            this.speed = speed;
            this.moveInterval = moveInterval;
        }
    }
}
