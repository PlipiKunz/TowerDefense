using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{

    class Renderer : System
    {
        private readonly int GRID_SIZE;
        private readonly int CELL_SIZE;
        private readonly int OFFSET_X;
        private readonly int OFFSET_Y;
        private readonly SpriteBatch m_spriteBatch;
        private readonly Texture2D m_texBackground;

        public Renderer(SpriteBatch spriteBatch, Texture2D texBackGround, int width, int height, int gridSize) :
            base(typeof(Components.Sprite), typeof(Components.Position))
        {
            GRID_SIZE = gridSize;
            CELL_SIZE = height / gridSize;
            OFFSET_X = (width - gridSize * CELL_SIZE) / 2;
            OFFSET_Y = (height - gridSize * CELL_SIZE) / 2;
            m_spriteBatch = spriteBatch;
            m_texBackground = texBackGround;
        }

        public override void Update(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            //
            // Draw a blue background
            Rectangle background = new Rectangle(OFFSET_X, OFFSET_Y, GRID_SIZE * CELL_SIZE, GRID_SIZE * CELL_SIZE);
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
            area.X = OFFSET_X + position.x * CELL_SIZE;
            area.Y = OFFSET_Y + position.y * CELL_SIZE;
            area.Width = position.w * CELL_SIZE;
            area.Height = position.h * CELL_SIZE;
            m_spriteBatch.Draw(appearance.image, area, appearance.stroke);
                
            //drawing the actual image, ontop of the stroked item
            area.X = OFFSET_X + (position.x * CELL_SIZE) + 1;
            area.Y = OFFSET_Y + (position.y * CELL_SIZE) + 1;
            area.Width = (position.w * CELL_SIZE) - 2;
            area.Height = (position.h * CELL_SIZE) - 2;
            m_spriteBatch.Draw(appearance.image, area, appearance.fill);
        }

        private float lerp(float a, float b, float f)
        {
            return a + f * (b - a);
        }

    }
}
