using CS5410.TowerDefenseGame;
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

                    if ( (curDistance <= towerComponent.range) && ( (towerComponent.targetType == creepComponent.creepType) || towerComponent.targetType == Components.TargetType.Both) &&  (minCreep == null || minDistance > curDistance)) {
                        minCreep = creep;
                        minDistance = curDistance;
                    }
                }

                towerComponent.target = minCreep;
            }
        }

        private void towerRotateAndFire(GameTime gameTime, List<Entity> towers) {
            foreach (var tower in towers)
            {
                var position = tower.GetComponent<Components.Position>();
                var towerComponent = tower.GetComponent<Components.TowerComponent>();
                var orientation = tower.GetComponent<Components.Orientation>();

                //updates fire interval timer
                towerComponent.elapsedInterval += (uint)gameTime.ElapsedGameTime.Milliseconds;

                //if the target creep does exist
                Entity targetCreep = towerComponent.target;
                if (targetCreep != null)
                {
                    var creepPosition = targetCreep.GetComponent<Components.Position>();
                    orientation.radianGoal = CoordinateSystem.angle(new Vector2(position.CenterX, position.CenterY), new Vector2(creepPosition.CenterX, creepPosition.CenterY));

                    bool linedUp = orientation.rotateToGoal(gameTime);
                    if (linedUp)
                    {
                        fire(tower);
                        //resets fire interval timer
                        if (towerComponent.elapsedInterval > towerComponent.fireInterval)
                        {
                            towerComponent.elapsedInterval = 0;
                        }
                    }
                }
                else {
                    orientation.rotate(gameTime);
                }

                //stop elapsed interval from possibly overflowwing, while not disrupting fire timer
                if (towerComponent.elapsedInterval > towerComponent.fireInterval*2)
                {
                    towerComponent.elapsedInterval = (uint)(towerComponent.fireInterval * 1.5);
                }
            }
        }

        private void fire(Entity tower)
        {
            var towerComponent = tower.GetComponent<Components.TowerComponent>();
            if (towerComponent.elapsedInterval > towerComponent.fireInterval)
            {
                var towerPos = tower.GetComponent<Components.Position>();

                Entity targetCreep = towerComponent.target;
                if (targetCreep != null)
                {

                    Entity bullet;
                    if (towerComponent.bulletType == Components.bulletType.bomb)
                    {
                        bullet = Bullet.createBomb(towerPos.CenterX, towerPos.CenterY, targetCreep, towerComponent.level, towerComponent.targetType);
                    }
                    else if (towerComponent.bulletType == Components.bulletType.missle)
                    {
                        bullet = Bullet.createMissle(towerPos.CenterX, towerPos.CenterY, targetCreep, towerComponent.level, towerComponent.targetType);
                    }
                    else
                    {
                        bullet = Bullet.createProjectile(towerPos.CenterX, towerPos.CenterY, targetCreep, towerComponent.level);
                    }
                    GameModel.m_addThese.Add(bullet);
                }
            }
        }
        

    }
}
