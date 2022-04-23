using CS5410.Particles;
using CS5410.TowerDefenseGame;
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Systems
{
    class Renderer : System
    {
        private readonly SpriteBatch m_spriteBatch;

        private readonly Texture2D m_texBackground;
        private readonly SpriteFont spriteFont;
        public static Texture2D blank;

        public static ExplosionEmitter m_explosion_emitter;
        public static TrailEmitter m_trail_emitter;
        public static TextEmitter m_text_emitter;

        public Renderer(ContentManager content, SpriteBatch spriteBatch, GraphicsDevice gd) :
            base(typeof(Components.Sprite), typeof(Components.Position))
        {
            m_spriteBatch = spriteBatch;
            spriteFont = content.Load<SpriteFont>("Fonts/menu");
            m_texBackground = content.Load<Texture2D>("Sprites/SquareSprite");
            blank = new Texture2D(gd, 1, 1);

            m_explosion_emitter = new ExplosionEmitter(content);
            m_trail_emitter = new TrailEmitter(content);
            m_text_emitter = new TextEmitter(content);
        }

        public override void Update(GameTime gameTime)
        {
            m_explosion_emitter.update(gameTime);
            m_trail_emitter.update(gameTime);   
            m_text_emitter.update(gameTime);

            m_spriteBatch.Begin(SpriteSortMode.FrontToBack);

            Rectangle background = CoordinateSystem.convertGameToPix(0, 0, CoordinateSystem.GRID_SIZE, CoordinateSystem.GRID_SIZE);
            m_spriteBatch.Draw(m_texBackground, background, Color.Blue);

            //render all the entities
            foreach (var entity in m_entities.Values)
            {
                renderEntity(entity);
            }

            m_explosion_emitter.draw(m_spriteBatch);
            m_trail_emitter.draw(m_spriteBatch);
            m_text_emitter.draw(m_spriteBatch);

            drawUI();

            m_spriteBatch.End();
        }

        public void drawUI() {
            string text = "Score: " + GameModel.score;
            Vector2 stringSize = spriteFont.MeasureString(text);
            Renderer.drawStrokedString(spriteFont, text, new Vector2(), Color.Yellow, m_spriteBatch);
            text = "Level: " + LevelSystem.level;
            stringSize = spriteFont.MeasureString(text);
            Renderer.drawStrokedString(spriteFont, text, new Vector2(0, stringSize.Y), Color.Yellow, m_spriteBatch);

            text = "Health: " + GameModel.health;
            stringSize = spriteFont.MeasureString(text);
            Renderer.drawStrokedString(spriteFont, text,new Vector2(CoordinateSystem.SWIDTH - CoordinateSystem.OFFSET_X, 0),Color.Yellow, m_spriteBatch);
            text = "Funds: " + GameModel.funds;
            stringSize = spriteFont.MeasureString(text);
            Renderer.drawStrokedString(spriteFont, text, new Vector2(CoordinateSystem.SWIDTH - CoordinateSystem.OFFSET_X, stringSize.Y), Color.Yellow, m_spriteBatch);
        }

        public static void drawStrokedString(SpriteFont font, string text, Vector2 pos, Color c, SpriteBatch sb) {
            for (int i = -1; i <= 1; i += 2) {
                Vector2 offsetPos = pos;
                offsetPos.X += i*1;
                offsetPos.Y += i*1;
                Renderer.drawString(font, text, offsetPos, Color.Black, .99f, sb);
            }
            Renderer.drawString(font, text, pos, c, 1, sb);
        }
        public static void drawString(SpriteFont font, string text, Vector2 pos, Color c, float layerDepth, SpriteBatch sb)
        {
            sb.DrawString(
                font,
                text,
                pos,
                c, 0,new Vector2(), 1, SpriteEffects.None, layerDepth);
        }
        private void renderEntity(Entity entity)
        {
            var appearance = entity.GetComponent<Components.Sprite>();
            var position = entity.GetComponent<Components.Position>();

            float rotation = 0;
            if (appearance.rotatable)
            {
                rotation = entity.GetComponent<Components.Orientation>().radians;
            }

            //drawing the stoked outline
            Rectangle area = CoordinateSystem.convertGameToPix(position.x, position.y, position.w, position.h);
            draw(appearance.image, area, appearance.stroke, appearance.priority - .01f, rotation);

            //drawing the actual image, ontop of the stroked item
            int offsetAmount = 2;
            area.X += offsetAmount;
            area.Y += offsetAmount;
            area.Width -= offsetAmount * 2;
            area.Height -= offsetAmount * 2;
            draw(appearance.image, area, appearance.fill, appearance.priority, rotation);

            //if the entity has a health component
            if (entity.ContainsComponent<Components.Health>()) {
                var health = entity.GetComponent<Components.Health>();

                if (health.health != health.max_health) { 
                    Rectangle healthBar = new Rectangle();
                    healthBar.Width = (int)(area.Width * 1.25);
                    healthBar.Height = CoordinateSystem.convertGameToPix(0, 0, 0, .1f).Height;

                    healthBar.X = area.Center.X - healthBar.Width / 2;
                    healthBar.Y = area.Y - healthBar.Height - offsetAmount*2;
                    draw(m_texBackground, healthBar, Color.Red, appearance.priority+.01f);

                    healthBar.Width = (int)(area.Width * 1.25 * ((float)health.health / health.max_health));
                    draw(m_texBackground, healthBar, Color.LightGreen, appearance.priority+.02f);
                }
            }
        }
        public void draw(Texture2D image, Rectangle destRect, Color stroke, float layerDepth = 0, float rotation = 0, Rectangle? sourceRect = null)
        {
            destRect.X += destRect.Width / 2;
            destRect.Y += destRect.Height / 2;

            Vector2 origin = new Vector2(image.Width / 2, image.Height / 2);
            if (sourceRect != null)
            {
                origin.X += sourceRect.Value.X;
                origin.Y += sourceRect.Value.Y;
            }

            m_spriteBatch.Draw(image, destRect, sourceRect, stroke, rotation, origin, SpriteEffects.None, layerDepth);
        }

    }

}