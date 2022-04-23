using CS5410;
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
    class LevelSystem : System
    {
        public static int level;
        public static bool inLevel;

        public bool inWave;
        public int curWaveCount;
        public const uint WaveInterval = 5000;
        public uint elapsedWaveInterval;


        public const uint SpawnInterval = 333;
        public uint elapsedSpawnInterval;

        public uint creepsToSpawn;

        public List<Vector2> entrancesAndExits = new List<Vector2>();

        protected MyRandom m_random = new MyRandom();

        public LevelSystem() : base(typeof(Components.CreepComponent))
        {
            inLevel = false;
            level = 0;

            nextLevel();
        }

        public override void Update(GameTime gameTime)
        {
            if (inLevel) {
                elapsedWaveInterval += (uint)gameTime.ElapsedGameTime.Milliseconds;
                elapsedSpawnInterval += (uint)gameTime.ElapsedGameTime.Milliseconds;

                if (inWave)
                {
                    addCreep();
                }

                //if the current wave is done
                if (!inWave &&  (elapsedWaveInterval >= WaveInterval*2 || m_entities.Count == 0))
                {
                    elapsedWaveInterval = 0;
                    curWaveCount++;
                    creepsToSpawn = (uint)(Math.Pow(2, level) + curWaveCount);
                    inWave = true;
                }
                else if(inWave && (elapsedWaveInterval >= WaveInterval || creepsToSpawn <= 0 ))
                {
                    elapsedWaveInterval = 0;
                    inWave = false;
                }

                //if all waves are done
                if (curWaveCount >= Math.Pow(2, level))
                {
                    inLevel = false;
                }
            }

        }

        public void addCreep() {
            if (elapsedSpawnInterval >= SpawnInterval)
            {
                if (m_random.NextDouble() >= .66f)
                {

                    if (creepsToSpawn > 0)
                    {
                        creepsToSpawn--;

                        Vector2 startPos = entrancesAndExits[0];
                        Vector2 endPos = entrancesAndExits[1];

                        var creep = Creeps.createSimpleGround(startPos.X, startPos.Y, endPos);
                        GameModel.m_addThese.Add(creep);
                        CreepMovement.Instance().upToDate = false;
                    }
                }
                elapsedSpawnInterval = 0;
            }
        }

        public void nextLevel() { 
            inLevel = true;
            level++;

            elapsedWaveInterval = 0;

            curWaveCount = 0;

            creepsToSpawn = (uint)(Math.Pow(2, level) + curWaveCount);
            inWave = true;

            entrancesAndExits = new List<Vector2>()
            {
                new Vector2(0,0),
                new Vector2(CoordinateSystem.GRID_SIZE-1, CoordinateSystem.GRID_SIZE-1),
            };
        }
    }
        
}
