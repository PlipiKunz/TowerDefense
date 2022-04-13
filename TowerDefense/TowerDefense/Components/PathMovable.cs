
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Components
{

    public class PathMovable : Component
    {
        public float moveAmount; //game units per milli-second
        public Vector2 goal;

        public Vector2 nextPointToGoal;
        public List<Vector2> path;

        public PathMovable(float moveAmount ,Vector2 goal)
        {
            this.moveAmount = moveAmount;
            this.goal = goal;
            path = null;
            nextPointToGoal = new Vector2(-1,-1);
        }
    }
}
