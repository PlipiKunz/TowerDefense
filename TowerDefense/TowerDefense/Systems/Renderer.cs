using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{
    class Renderer : System
    {

        private readonly SpriteBatch m_spriteBatch;
        private readonly Texture2D m_texBackground;

        public Renderer(SpriteBatch spriteBatch, Texture2D texBackGround) :
            base(typeof(Components.Sprite), typeof(Components.Position))
        {
            m_spriteBatch = spriteBatch;
            m_texBackground = texBackGround;
        }

        public override void Update(GameTime gameTime)
        {
            m_spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);

            // Draw a blue background
            Rectangle background = CoordinateSystem.convertGameToPix(0,0,CoordinateSystem.GRID_SIZE, CoordinateSystem.GRID_SIZE);
            m_spriteBatch.Draw(m_texBackground, background, Color.Blue);

            //render all the entities
            foreach (var entity in m_entities.Values)
            {
                renderEntity(entity);
            }

            m_spriteBatch.End();
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
            draw(appearance.image, area, appearance.stroke, appearance.priority, rotation);

            //drawing the actual image, ontop of the stroked item
            area.X += 1;
            area.Y += 1;
            area.Width  -= 2;
            area.Height  -= 2;
            draw(appearance.image, area, appearance.fill, appearance.priority, rotation);
        }

        private void draw(Texture2D image, Rectangle destRect, Color stroke, float layerDepth = 0, float rotation = 0, Rectangle? sourceRect = null) {
            destRect.X += destRect.Width / 2;
            destRect.Y += destRect.Height / 2;

            Vector2 origin = new Vector2(image.Width / 2, image.Height / 2);
            if (sourceRect != null) {
                origin.X += sourceRect.Value.X;
                origin.Y += sourceRect.Value.Y;
            }

            m_spriteBatch.Draw(image, destRect, sourceRect, stroke, rotation, origin, SpriteEffects.None, layerDepth);
        }

    }

}
