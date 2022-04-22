using System;
using System.Collections.Generic;
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Systems;

namespace CS5410.Particles
{
    public class ExplosionEmitter: ParticleEmitter
    {
        private Texture2D particle;
        public const int EXPLOSION_PARTICLE_SPEED = 1;
        public const int EXPLOSION_PARTICLE_SIZE = 5;
        public static TimeSpan EXPLOSION_SPAN = new TimeSpan(0, 0, 0, 0, 500);


        public ExplosionEmitter(ContentManager content): base(content)
        {
            particle = content.Load<Texture2D>("Sprites/SquareSprite");

        }

        /// <summary>
        /// updates the state of existing ones and retires expired particles.
        /// </summary>
        public override void update(GameTime gameTime)
        {
            foreach (Particle p in m_particles.Values)
            {
                //
                // Update its position
                p.position += (p.direction * p.speed);
                //
                // Have it rotate proportional to its speed
                p.rotation += p.speed / 50.0f;
                //
                // Apply some gravity
                //p.direction += this.Gravity;
            }
            remove(gameTime);
        }

        /// <summary>
        /// Renders the active particles
        /// </summary>
        public override void draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(0, 0, 0, 0);
            foreach (Particle p in m_particles.Values)
            {
                Texture2D texDraw = particle;

                r.X = (int)p.position.X;
                r.Y = (int)p.position.Y;
                r.Width = p.size;
                r.Height = p.size;

                spriteBatch.Draw(
                    texDraw,
                    r,
                    null,
                    Color.Red,
                    p.rotation,
                    new Vector2(texDraw.Width / 2, texDraw.Height / 2),
                    SpriteEffects.None,
                    .1f);
            }
        }
        public void addExplosion(Entity e) {
            var pos = e.GetComponent<Components.Position>();

            var range = e.GetComponent<Components.Bomb>();
            // Generate particles at the place
            for (int i = 0; i < 100*range.range; i++)
            {

                float r = m_random.nextRange(0, range.range) ;
                float angle = m_random.nextRange(0, 360) * (float)(Math.PI / 180);
                var adjustedPos = CoordinateSystem.convertGameToPix(pos.x + (float)(Math.Cos(angle) * r), pos.y + (float)(Math.Sin(angle) * r), pos.w, pos.h);

                Vector2 v = new Vector2(adjustedPos.X, adjustedPos.Y);
                Particle p = new Particle(
                    m_random.Next(),
                    v,
                    m_random.nextCircleVector(),
                    (float)m_random.nextGaussian(EXPLOSION_PARTICLE_SPEED, 1),
                    EXPLOSION_SPAN,
                    EXPLOSION_PARTICLE_SIZE);

                if (!m_particles.ContainsKey(p.name))
                {
                    m_particles.Add(p.name, p);
                }
            }
        }
    }
}
