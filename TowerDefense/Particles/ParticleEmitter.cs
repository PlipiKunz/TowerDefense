using System;
using System.Collections.Generic;
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Systems;

namespace CS5410.Particles
{
    public abstract class ParticleEmitter
    {
        protected Dictionary<int, Particle> m_particles = new Dictionary<int, Particle>();
        protected MyRandom m_random = new MyRandom();
        private TimeSpan m_accumulated = TimeSpan.Zero;
        public ParticleEmitter(ContentManager content)
        {

        }

        public void remove(GameTime gameTime) {
            //
            // For any existing particles, update them, if we find ones that have expired, add them
            // to the remove list.
            List<int> removeMe = new List<int>();
            foreach (Particle p in m_particles.Values)
            {
                p.lifetime -= gameTime.ElapsedGameTime;
                if (p.lifetime < TimeSpan.Zero)
                {
                    //
                    // Add to the remove list
                    removeMe.Add(p.name);
                }
            }

            //
            // Remove any expired particles
            foreach (int Key in removeMe)
            {
                m_particles.Remove(Key);
            }
        }

        /// <summary>
        /// updates the state of existing ones and retires expired particles.
        /// </summary>
        public abstract void update(GameTime gameTime);
        /// <summary>
        /// Renders the active particles
        /// </summary>
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
