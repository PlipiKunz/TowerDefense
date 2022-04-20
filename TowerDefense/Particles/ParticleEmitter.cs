using System;
using System.Collections.Generic;
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Systems;

namespace CS5410.Particles
{
    public class ParticleEmitter
    {

        private Dictionary<int, Particle> m_particles = new Dictionary<int, Particle>();
        private Texture2D particle;
        private MyRandom m_random = new MyRandom();

        private int m_sarticleSize;
        private int m_speed;
        private TimeSpan m_lifetime;

        public Vector2 Gravity { get; set; }

        public ParticleEmitter(ContentManager content,int size, int speed, TimeSpan lifetime)
        {
            m_sarticleSize = size;
            m_speed = speed;
            m_lifetime = lifetime;

            particle = content.Load<Texture2D>("Sprites/SquareSprite");

            this.Gravity = new Vector2(0, 0);
        }

        private TimeSpan m_accumulated = TimeSpan.Zero;

        /// <summary>
        /// Generates new particles, updates the state of existing ones and retires expired particles.
        /// </summary>
        public void update(GameTime gameTime)
        {
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
                //
                // Update its position
                p.position += (p.direction * p.speed);
                //
                // Have it rotate proportional to its speed
                p.rotation += p.speed / 50.0f;
                //
                // Apply some gravity
                p.direction += this.Gravity;
            }

            //
            // Remove any expired particles
            foreach (int Key in removeMe)
            {
                m_particles.Remove(Key);
            }
        }

        /// <summary>
        /// Renders the active particles
        /// </summary>
        public void draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(0, 0, m_sarticleSize, m_sarticleSize);
            foreach (Particle p in m_particles.Values)
            {
                Texture2D texDraw = particle;

                r.X = (int)p.position.X;
                r.Y = (int)p.position.Y;
                spriteBatch.Draw(
                    texDraw,
                    r,
                    null,
                    Color.Gold,
                    p.rotation,
                    new Vector2(texDraw.Width / 2, texDraw.Height / 2),
                    SpriteEffects.None,
                    1f);
            }
        }

        public void add(Entity e) {
            var pos = e.GetComponent<Components.Position>();
            var normal_pos = CoordinateSystem.convertGameToPix(pos.x, pos.y, pos.w, pos.h);

            // Generate particles at the place
            for (int i = 0; i < 250; i++)
            {
                Vector2 v = new Vector2(normal_pos.X, normal_pos.Y);
                float r = m_random.nextRange(normal_pos.X, normal_pos.X + normal_pos.Width );
                float  r1 = m_random.nextRange(normal_pos.Y, normal_pos.Y + normal_pos.Height);

                v.X = r ;
                v.Y = r1 ;

                Particle p = new Particle(
                    m_random.Next(),
                    v,
                    m_random.nextCircleVector(),
                    (float)m_random.nextGaussian(m_speed, 1),
                    m_lifetime);

                if (!m_particles.ContainsKey(p.name))
                {
                    m_particles.Add(p.name, p);
                }
            }
        }
    }
}
