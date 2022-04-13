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

            //
            // Draw a blue background
            Rectangle background = CoordinateSystem.convertGameToPix(0,0,CoordinateSystem.GRID_SIZE, CoordinateSystem.GRID_SIZE);
            m_spriteBatch.Draw(m_texBackground, background, Color.Blue);

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
            Rectangle area = new Rectangle();

            //drawing the stoked outline
            area = CoordinateSystem.convertGameToPix(position.x, position.y, position.w, position.h);
            draw(appearance.image, area, appearance.stroke, appearance.priority);

            //drawing the actual image, ontop of the stroked item
            area.X += 1;
            area.Y += 1;
            area.Width  -= 2;
            area.Height  -= 2;
            draw(appearance.image, area, appearance.fill, appearance.priority);
        }

        private void draw(Texture2D image, Rectangle r, Color stroke, float layerDepth) {
            m_spriteBatch.Draw(image, r, null, stroke, 0, new Vector2(), SpriteEffects.None, layerDepth);
        }

    }

}
