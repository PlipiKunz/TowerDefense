using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CS5410
{
    public class HighScoresView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "These are the high scores";

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.HighScores;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            float bottom = drawMenuItem(m_font,"High Scores", 200, Color.Yellow);

            int i = 1;
            foreach (int score in Persistence.ScorePersistence.scores)
            {
                bottom = drawMenuItem(m_font, "(" + i + "): " + score, bottom, Color.Yellow);
                i++;
            }

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
