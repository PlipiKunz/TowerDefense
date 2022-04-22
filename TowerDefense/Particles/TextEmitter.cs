using System;
using System.Collections.Generic;
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Systems;

namespace CS5410.Particles
{
    public class TextEmitter: ParticleEmitter
    {
        private SpriteFont spriteFont;
        public const float PARTICLE_SPEED = .3f;
        public static TimeSpan SPAN = new TimeSpan(0, 0, 1);


        public TextEmitter(ContentManager content): base(content)
        {
            spriteFont = content.Load<SpriteFont>("Fonts/menu");
        }

        /// <summary>
        /// updates the state of existing ones and retires expired particles.
        /// </summary>
        public override void update(GameTime gameTime)
        {
            foreach (Particle p in m_particles.Values)
            {
                // Update its position and apply gravity
                p.position += ((p.direction + new Vector2((float)Math.Cos(p.rotation), 0)*15) * p.speed);
               
                // Have it rotate proportional to its speed
                p.rotation += p.speed / 2.5f;
            }


            remove(gameTime);
        }

        /// <summary>
        /// Renders the active particles
        /// </summary>
        public override void draw(SpriteBatch spriteBatch)
        {
            Vector2 r = new Vector2(0, 0);
            foreach (Particle p in m_particles.Values)
            {
                r.X = (int)p.position.X;
                r.Y = (int)p.position.Y;


                string text = p.text;
                Vector2 stringSize = spriteFont.MeasureString(text);

                r.X -= stringSize.X/2;
                r.Y -= stringSize.Y/2;

                Renderer.drawStrokedString(spriteFont, text, r, Color.Yellow, spriteBatch);
            }
        }

        public void addText(Entity e, String text) {
            var pos = e.GetComponent<Components.Position>();

            // Generate particle at the place
                var adjustedPos = CoordinateSystem.convertGameToPix(pos.CenterX, pos.CenterY,  pos.w, pos.h);
                Vector2 v = new Vector2(adjustedPos.X, adjustedPos.Y);
                Particle p = new Particle(
                    m_random.Next(),
                    v,
                    new Vector2(0, -1f),
                    PARTICLE_SPEED,
                    SPAN,
                    0,
                    text);

                if (!m_particles.ContainsKey(p.name))
                {
                    m_particles.Add(p.name, p);
                }
        }
    }
}
