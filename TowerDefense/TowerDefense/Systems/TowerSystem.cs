using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Systems
{
    /// <summary>
    /// This system knows how to rotate towers and fire at creeps
    /// </summary>
    class TowerSystem : System
    {

        public TowerSystem()
            : base(typeof(Components.TowerComponent))
        {
        }

        public override void Update(GameTime gameTime)
        {
            List<Entity> towers = CoordinateSystem.Instance().findTowers();

            towerCreepTarget(gameTime, towers);
            towerRotateAndFire(gameTime, towers);
        }

        private void towerCreepTarget(GameTime gameTime, List<Entity> towers) {
            foreach (var tower in towers) {
                var towerPosition = tower.GetComponent<Components.Position>();
                var towerComponent = tower.GetComponent<Components.TowerComponent>();

                float minDistance = -1;
                Entity minCreep = null;
                foreach (var creep in CoordinateSystem.Instance().findCreeps())
                {
                    var creepPosition = creep.GetComponent<Components.Position>();
                    var creepComponent = creep.GetComponent<Components.CreepComponent>();
                    float curDistance = CoordinateSystem.distance(new Vector2(towerPosition.x, towerPosition.y), new Vector2(creepPosition.x, creepPosition.y));

                    if ( (curDistance <= towerComponent.range) && (towerComponent.targetType == creepComponent.creepType) &&  (minCreep == null || minDistance > curDistance)) {
                        minCreep = creep;
                        minDistance = curDistance;
                    }
                }

                towerComponent.target = minCreep;
            }
        }


        private const float CLOSE_ENOUGH_ANGLE = 5f;
        private void towerRotateAndFire(GameTime gameTime, List<Entity> towers) {
            foreach (var tower in towers)
            {
                var position = tower.GetComponent<Components.Position>();
                var towerComponent = tower.GetComponent<Components.TowerComponent>();
                var orientation = tower.GetComponent<Components.Orientation>();

                Entity targetCreep = towerComponent.target;
                if (targetCreep != null)
                {
                    var creepPosition = targetCreep.GetComponent<Components.Position>();
                    float goalAngle = CoordinateSystem.angle(new Vector2(position.CenterX, position.centerY), new Vector2(creepPosition.CenterX, creepPosition.centerY));

                    float prevAngle = orientation.radians;
                    float curAngle = orientation.radians;


                    if (Math.Abs(curAngle - goalAngle) > (CLOSE_ENOUGH_ANGLE * (float)(Math.PI / 180)))
                    {
                        float degreesToMove = orientation.degreeTurnSpeed * gameTime.ElapsedGameTime.Milliseconds;

                        if (crossProduct(curAngle, goalAngle) < 0)
                        {
                            degreesToMove *= -1;
                        }

                        curAngle += degreesToMove;

                        if ((crossProduct(curAngle, goalAngle) < 0) != (crossProduct(prevAngle, goalAngle) < 0))
                        {
                            curAngle = goalAngle;
                        }

                        orientation.radians = curAngle;
                    }
                    else {
                        fire();
                    }
                }

            }
        }

        private void fire() { 
        
        }
        private float crossProduct(float angleA, float angleB)
        {
            Vector2 vA = new Vector2((float)Math.Cos(angleA), (float)Math.Sin(angleA));
            Vector2 vB = new Vector2((float)Math.Cos(angleB), (float)Math.Sin(angleB));

            return (float)((vA.X * vB.Y) - (vA.Y * vB.X));

        }

    }
}
