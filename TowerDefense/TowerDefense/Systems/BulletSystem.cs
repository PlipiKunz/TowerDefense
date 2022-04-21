
using CS5410.TowerDefenseGame;
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
    class BulletSystem : System
    {
        public BulletSystem() : base(typeof(Components.BulletComponent))
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var bullet in m_entities.Values)
            {
                bool hit = moveBullet(bullet, gameTime);
                if (hit)
                {
                    bulletHit(bullet);
                }
            }
        }

        /// <summary>
        /// moves bullet, returns true if goal has been reached by the bullet
        /// </summary>
        public bool moveBullet(Entity bullet, GameTime gameTime) {

            var bulletComponent = bullet.GetComponent<Components.BulletComponent>();
            var bulletPos = bullet.GetComponent<Components.Position>();
            var bulletOrientation = bullet.GetComponent<Components.Orientation>();

            //bombs dont have creep goals
            if (bulletComponent.type != Components.bulletType.bomb)
            {
                //set goal angle to that of the creep if there is one
                var creep = bulletComponent.target;
                var creepPos = creep.GetComponent<Components.Position>();
                bulletComponent.goalPos =  new Vector2(creepPos.CenterX, creepPos.CenterY);
            }

            bulletOrientation.radianGoal = CoordinateSystem.angle(new Vector2(bulletPos.CenterX, bulletPos.CenterY), bulletComponent.goalPos);
            bulletOrientation.rotateToGoal(gameTime);

            var prevPos = new Vector2(bulletPos.CenterX, bulletPos.CenterY);

            //bullet movement
            float curMovmement = bulletComponent.moveAmount * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            bulletPos.move(new Vector2((float)Math.Cos(bulletOrientation.radianGoal), (float)Math.Sin(bulletOrientation.radianGoal)), curMovmement);

            if (bulletComponent.type != Components.bulletType.bomb)
            {
                var creep = bulletComponent.target;
                return CoordinateSystem.collides(bullet, creep);
            }
            return between(prevPos, bulletComponent.goalPos, new Vector2(bulletPos.CenterX, bulletPos.CenterY));
        }

        /// <summary>
        /// handles bullet hits
        /// </summary>
        public void bulletHit(Entity bullet) {
            var bulletComponent = bullet.GetComponent<Components.BulletComponent>();
            var bulletPos = bullet.GetComponent<Components.Position>();

            var damage = bullet.GetComponent<Components.Damage>().damage;
            if (bulletComponent.type != Components.bulletType.bomb)
            {
                var creep = bulletComponent.target;
                hitCreep(creep, damage);
            }
            else { 
                var bombComponent = bullet.GetComponent<Components.Bomb>();

                foreach (var creep in CoordinateSystem.Instance().findCreeps())
                {
                    var creepPos = creep.GetComponent<Components.Position>();
                    var creepComp = creep.GetComponent<Components.CreepComponent>();
                    if (creepComp.creepType == bombComponent.targetType) { 
                        if (CoordinateSystem.distance(new Vector2(bulletPos.CenterX, bulletPos.CenterY), new Vector2(creepPos.CenterX, creepPos.CenterY)) <= bombComponent.range) {
                            hitCreep(creep, damage);
                        }
                    }
                }
            }

            GameModel.m_removeThese.Add(bullet);
        }


        public void hitCreep(Entity creep, uint damage)
        {
            var creepHealth = creep.GetComponent<Components.Health>();
            if (creepHealth.health > 0) { 
                creepHealth.health -= (int)damage;

                if (creepHealth.health <= 0)
                {
                    var creepScore = creep.GetComponent<Components.Cost>();

                    GameModel.score += (int)creepScore.cost;
                    GameModel.funds += (int)creepScore.cost;

                    GameModel.m_removeThese.Add(creep);
                }
                
            }

        }

        /// <summary>
        /// if point b is between a and c
        /// </summary>
        public bool between(Vector2 a, Vector2 b, Vector2 c)
        {
            if (a.X < b.X && c.X < b.X)
            {
                return false;
            }
            else if (a.X > b.X && c.X > b.X)
            {
                return false;
            }
            else if (a.Y < b.Y && c.Y < b.Y)
            {
                return false;
            }
            else if (a.Y > b.Y && c.Y > b.Y)
            {
                return false;
            }
            return true;
        }
    }
}
