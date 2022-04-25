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

        static LevelSystem instance;
        private static object lockObj = new object();
        public static LevelSystem Instance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = new LevelSystem();
                }
            }

            return instance;
        }
        public static void reset()
        {
            lock (lockObj)
            {
                instance = new LevelSystem();
            }
        }

        public static int level;
        public static bool inLevel;

        public bool inWave;
        public int curWaveCount;
        public uint WaveInterval = 5000;
        public uint elapsedWaveInterval;


        public const uint SpawnInterval = 500;
        public uint elapsedSpawnInterval;

        public uint creepsToSpawn;

        public List<Vector2> entrancesAndExits = new List<Vector2>();

        protected MyRandom m_random = new MyRandom();

        protected LevelSystem() : base(typeof(Components.CreepComponent))
        {
            inLevel = false;
            level = 0;


            entrancesAndExits = new List<Vector2>()
            {
                new Vector2(0, CoordinateSystem.GRID_SIZE/2),
                new Vector2(CoordinateSystem.GRID_SIZE-1, CoordinateSystem.GRID_SIZE/2),
                new Vector2(CoordinateSystem.GRID_SIZE/2, 0),
                new Vector2(CoordinateSystem.GRID_SIZE/2, CoordinateSystem.GRID_SIZE-1),

            };
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
                if (creepsToSpawn > 0)
                {
                    creepsToSpawn--;

                    var endpoints = getEntrancesAndExits(level);
                    Vector2 startPos = endpoints[0];
                    Vector2 endPos = endpoints[1];

                    var creep = Creeps.createSimpleGround(startPos.X, startPos.Y, endPos, level);

                    GameModel.m_addThese.Add(creep);
                    CreepMovement.Instance().upToDate = false;
                }
                elapsedSpawnInterval = 0;
            }
        }

        public List<Vector2> getEntrancesAndExits(int level)
        {
            List<Vector2> result = new List<Vector2>();

            if (level == 1)
            {
                result.Add(entrancesAndExits[0]);
                result.Add(entrancesAndExits[1]);
            }
            else if (level == 2)
            {
                result.Add(entrancesAndExits[2]);
                result.Add(entrancesAndExits[3]);
            }
            else {
                return getEntrancesAndExits((curWaveCount % 2) + 1);
            }

            return result;
        }

        public void nextLevel() { 
            inLevel = true;
            level++;

            WaveInterval -= 250;
            if(WaveInterval < 500) WaveInterval = 500; 

            elapsedWaveInterval = 0;

            curWaveCount = 0;

            creepsToSpawn = (uint)(Math.Pow(2, level) + curWaveCount);
            inWave = true;

        }
    }
        
}
