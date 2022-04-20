using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CS5410.Persistence;
using CS5410.TowerDefenseGame;

namespace CS5410
{
    public class GamePlayView : GameStateView
    {

        ContentManager m_content;
        public static GameModel m_gameModel;

        public override void initializeSession()
        {
            m_gameModel = new GameModel(m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight);
            m_gameModel.initialize(m_content, m_spriteBatch, m_graphics.GraphicsDevice);
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_content = contentManager;
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //ScorePersistence.score = m_gameModel.score;
                return GameStateEnum.EndGameConfirm;
            }

            return checkIfDone();
        }

        public override void render(GameTime gameTime)
        {
            m_gameModel.render(gameTime);
        }

        public override void update(GameTime gameTime)
        {
            m_gameModel.update(gameTime);
            ScorePersistence.score = GameModel.score;
        }


        private GameStateEnum checkIfDone() {
            if (m_gameModel.isDone)
            {
                done();
                return GameStateEnum.GameFinished;
            }

            return GameStateEnum.GamePlay;
        }

        public static void done()
        {
            ScorePersistence.addScore(GameModel.score);
        }
    }
}
