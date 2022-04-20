
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
                var bulletComponent = bullet.GetComponent<Components.BulletComponent>();
                var creep = bulletComponent.target;
                if (creep != null)
                {
                    var bulletPos = bullet.GetComponent<Components.Position>();
                    var bulletOrientation = bullet.GetComponent<Components.Orientation>();

                    var creepPos = creep.GetComponent<Components.Position>();

                    bulletOrientation.radianGoal = CoordinateSystem.angle(new Vector2(bulletPos.CenterX, bulletPos.CenterY), new Vector2(creepPos.CenterX, creepPos.CenterY));
                    bulletOrientation.rotateToGoal(gameTime);

                    //bullet movement
                    float curMovmement = bulletComponent.moveAmount * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    bulletPos.move(new Vector2((float)Math.Cos(bulletOrientation.radianGoal), (float)Math.Sin(bulletOrientation.radianGoal)), curMovmement);

                    var creepHealth = creep.GetComponent<Components.Health>();
                    if (creepHealth.health > 0)
                    {
                        //bullet creep collision
                        if (CoordinateSystem.collides(bullet, creep))
                        {
                            var bulletDamage = bullet.GetComponent<Components.Damage>();

                            creepHealth.health -= (int)bulletDamage.damage;

                            if (creepHealth.health <= 0)
                            {
                                var creepScore = creep.GetComponent<Components.Cost>();

                                GameModel.score += (int)creepScore.cost;
                                GameModel.funds += (int)creepScore.cost;

                                GameModel.m_removeThese.Add(creep);
                            }

                            GameModel.m_removeThese.Add(bullet);
                        }
                    }
                    else
                    {
                        GameModel.m_removeThese.Add(bullet);
                    }
                }

            }
        }
    }
}
