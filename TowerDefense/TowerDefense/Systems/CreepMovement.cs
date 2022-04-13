using Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the movement of any
    /// entity with a movable & position components.
    /// </summary>
    class CreepMovement : System
    {
        static CreepMovement instance;
        private static object lockObj = new object();
        public static CreepMovement Instance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = new CreepMovement();
                }
            }

            return instance;
        }
        public static void reset() {
            lock (lockObj)
            {
                instance = null;
            }
        }


        private int[,] gridMap = null;
        public bool upToDate = false;

        public CreepMovement() : base()
        {
            gridMap = new int[CoordinateSystem.GRID_SIZE, CoordinateSystem.GRID_SIZE];
            mapSetup();
        }

        public override void Update(GameTime gameTime)
        {
            if (!upToDate) {
                mapSetup();
                upToDate = true;
            }

            var creeps = CoordinateSystem.findCreeps(m_entities);
            orientationSet(gameTime, creeps);
            move(gameTime, creeps);
        }

        /// <summary>
        /// sets next location in creep pathfinding
        /// </summary>
        private void orientationSet(GameTime gameTime, List<Entity> creeps)
        {
            foreach (var entity in creeps)
            {
                var movable = entity.GetComponent<Components.PathMovable>();
                var orientation = entity.GetComponent<Components.Orientation>();
                var position = entity.GetComponent<Components.Position>();
 
                
                if (movable.path.Count > 0)
                {
                    Vector2 nextPoint = movable.path[0];

                    if (movable.path.Count > 1)
                    {
                        if (nextPoint.X == Math.Floor(position.x) && nextPoint.Y == Math.Floor(position.y))
                        {
                            movable.path.RemoveAt(0);
                            nextPoint = movable.path[0];
                            orientation.radians = (float)Math.Atan2(nextPoint.Y + .5 - (position.h/2)  - position.y, nextPoint.X +.5 - (position.w / 2) - position.x);
                        }

                    }
                }

            }
        }

        /// <summary>
        /// move all creeps
        /// </summary>
        private void move(GameTime gameTime, List<Entity> creeps)
        {
            foreach (var entity in creeps)
            {
                moveEntity(entity, gameTime);
            }
        }

        /// <summary>
        /// moves an entity according to its orientation and movable speed
        /// </summary>
        private void moveEntity(Entity entity, GameTime gameTime)
        {
            var movable = entity.GetComponent<Components.PathMovable>();
            var orientation = entity.GetComponent<Components.Orientation>();
            var position = entity.GetComponent<Components.Position>();

            float curMovmement = movable.moveAmount * (uint)gameTime.ElapsedGameTime.TotalMilliseconds;

            Vector2 movementVector = new Vector2((float)Math.Cos(orientation.radians), (float)Math.Sin(orientation.radians));
            movementVector.Normalize();

            position.x += movementVector.X * curMovmement;
            position.y += movementVector.Y * curMovmement;
        }

        /// <summary>
        /// complete map setup
        /// </summary>
        private void mapSetup()
        {
            clearMap();

            var towers = CoordinateSystem.findTowers(m_entities);
            mapAddTowers(towers);
            creepPathSet();
        }

        /// <summary>
        /// checks if a proposed  tower causes pathfinding to fail
        /// returns true if added tower would work, else false
        /// </summary>
        public bool validTowerLocation(Entity proposed)
        {
            int[,] before = gridMap;

            var towers = CoordinateSystem.findTowers(m_entities);
            towers.Add(proposed);

            clearMap();
            mapAddTowers(towers);

            bool valid = creepPathSet();
            if (!valid) {
                gridMap = before;
                mapSetup();
            }
            return valid;
        }

        /// <summary>
        /// clears the map
        /// </summary>
        private void clearMap()
        {
            for (int i = 0; i < gridMap.GetLength(0); i++)
            {
                for (int j = 0; j < gridMap.GetLength(1); j++)
                {
                    gridMap[i, j] = 1;
                }
            }
            return;
        }

        /// <summary>
        /// adds tower posiotions to grid map
        /// </summary>
        private void mapAddTowers(List<Entity> towers)
        {
            foreach (var entity in towers)
            {
                var position = entity.GetComponent<Components.Position>();
                int xPos = (int)Math.Floor(position.x);
                int yPos = (int)Math.Floor(position.y);
                gridMap[xPos, yPos] = -1;
            }
        }

        /// <summary>
        /// set gridpath for each creep, returns true if all creeps have a valid path, else false
        /// also makes sure entrances and exits can connect
        /// </summary>
        private bool creepPathSet()
        {
            //creep path checking
            var creeps = CoordinateSystem.findCreeps(m_entities);
            foreach (var entity in creeps) {
                if (!setEntityPath(entity)) {
                    return false;
                }
            }

            //makes sure all entrances and exits can connnect
            List<Vector2> entrancesAndExits = new List<Vector2>();
            foreach (var entrance in entrancesAndExits) {
                foreach (var exit in entrancesAndExits) {
                    //makes sure entrance and exit arent the same
                    if (entrance != exit) {
                        //if the path between the entrance and exit is null, then return
                        if (findPath(entrance, exit) == null) { 
                            return false;
                        }
                    }

                }
            }

            return true;
        }

        private bool setEntityPath(Entity creep)
        {
            var movable = creep.GetComponent<Components.PathMovable>();
            var position = creep.GetComponent<Components.Position>();

            PathNode endNode = findPath(new Vector2((int)Math.Floor(position.x), (int)Math.Floor(position.y)), movable.goal);
            List<Vector2> path = tracePath(endNode);

            movable.path = path;
            return movable.path == null;
        }

        private PathNode findPath(Vector2 curLoc, Vector2 goal) {
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closedList = new List<PathNode>();

            PathNode endNode = null;

            PathNode curNode;
            PathNode neighbor;

            //add starting node
            openList.Add(new PathNode(curLoc, 0, 0, null));
            //while there are still nodes to check and the end hasnt been found
            while ((openList.Count > 0) && (endNode == null))
            {
                //gets min cost node
                curNode = openList[PathNode.minNodeIndex(openList)];
                openList.RemoveAt(PathNode.minNodeIndex(openList));

                //cycles through neighbors
                foreach (Vector2 neighborLoc in neighbors(curNode))
                {
                    if (gridMap[(int)neighborLoc.X, (int)neighborLoc.Y] > 0)
                    {
                        //sets up the nodes
                        if (gridMap[(int)neighborLoc.X, (int)neighborLoc.Y] < 0) {
                            continue;
                        }
                        if (curNode.completeCost() < 0) {
                            continue;
                        }
                        
                        neighbor = new PathNode(neighborLoc, curNode.movementCost + gridMap[(int)neighborLoc.X, (int)neighborLoc.Y], heuristic(neighborLoc, goal), curNode);

                        //if the neighbor is the goal
                        if (neighbor.pos.X == goal.X && neighbor.pos.Y == goal.Y)
                        {
                            endNode = neighbor;
                            break;
                        }

                        //if there isnt a better verion of the neighbor already on the stack
                        if (PathNode.noLowerNodeSamePosition(openList, neighbor) && PathNode.noLowerNodeSamePosition(closedList, neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }

                // movves to closed list
                closedList.Add(curNode);
            }

            return endNode;
        }

        /// <summary>
        /// Various a star algorithm pieces
        /// </summary>
        private List<Vector2> tracePath(PathNode endNode)
        {
            if (endNode == null) return null;

            List<Vector2> trace = new List<Vector2>();

            PathNode curNode = endNode;
            while (curNode != null) {
                trace.Add(curNode.pos);
                curNode = curNode.parent;
            }

            trace.Reverse();
            return trace;
        }

        /// <summary>
        /// Various a star algorithm pieces
        /// </summary>
        private List<Vector2> neighbors(PathNode node) {
            List<Vector2> neighbors = new List<Vector2>();

            //neighbors are only to the sides, not diagonal
            List<Vector2> offsets = new List<Vector2>() { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(0, 1), };

            foreach (Vector2 offset in offsets) {
                int xOffset = (int)offset.X;
                int yOffset = (int)offset.Y;

                int xPos = (int)node.pos.X + xOffset;
                int yPos = (int)node.pos.Y + yOffset;

                if ((xPos >= 0 && yPos >= 0) && (xPos < CoordinateSystem.GRID_SIZE && yPos < CoordinateSystem.GRID_SIZE))
                {
                    neighbors.Add(new Vector2(xPos, yPos));
                }
            }

            return neighbors;
        }
        /// <summary>
        /// Various a star algorithm pieces
        /// </summary>
        private int heuristic(Vector2 loc, Vector2 goal) {
            return (int)Math.Sqrt(Math.Pow((loc.X - goal.X), 2) + Math.Pow((loc.Y - goal.Y), 2));
        }
    }

    public class PathNode{
        public Vector2 pos;
        public int movementCost;
        public int heurisitcCost;
        public PathNode parent;
        public PathNode(Vector2 pos, int movementCost, int heurisitcCost, PathNode parent)
        {
            this.pos = pos;
            this.movementCost = movementCost;
            this.heurisitcCost = heurisitcCost;
            this.parent = parent;
        }
        public int completeCost()
        {
            return movementCost + heurisitcCost;
        }


        public static int minNodeIndex(List<PathNode> nodes) { 
            if(nodes.Count <= 0) return -1;

            int minVal = nodes[0].completeCost();
            int minIndex = 0;

            PathNode curNode;
            int completeCost;
            for (int i = 0; i < nodes.Count; i++) {
                curNode = nodes[i];
                completeCost = curNode.completeCost();

                if (completeCost < minVal) { 
                    minIndex = i;
                    minVal = completeCost;
                }
            }

            return minIndex;
        }
        public static bool noLowerNodeSamePosition(List<PathNode> nodes, PathNode node)
        {
            if (nodes.Count <= 0) return true;

            Vector2 nodePos = node.pos;
            int nodeCost = node.completeCost();


            foreach(PathNode curNode in nodes)
            {

                if ((int)nodePos.X == (int)curNode.pos.X && (int)nodePos.Y == (int)curNode.pos.Y)
                {
                    if (curNode.completeCost() <= nodeCost)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }
}
