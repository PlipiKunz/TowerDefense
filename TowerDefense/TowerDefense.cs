using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using CS5410.Persistence;

namespace CS5410
{
    public class TowerDefense : Game
    {
        private GraphicsDeviceManager m_graphics;
        private IGameState m_currentState;
        private GameStateEnum m_nextStateEnum = GameStateEnum.MainMenu;
        private Dictionary<GameStateEnum, IGameState> m_states;

        public TowerDefense()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            m_graphics.PreferredBackBufferWidth = 1920;
            m_graphics.PreferredBackBufferHeight = 1080;

            m_graphics.ApplyChanges();

            // Create all the game states here
            m_states = new Dictionary<GameStateEnum, IGameState>();
            m_states.Add(GameStateEnum.MainMenu, new MainMenuView());
            m_states.Add(GameStateEnum.GamePlay, new GamePlayView());
            m_states.Add(GameStateEnum.HighScores, new HighScoresView());
            m_states.Add(GameStateEnum.Controls, new ControlsView());
            m_states.Add(GameStateEnum.Credits, new CreditsView());
            m_states.Add(GameStateEnum.EndGameConfirm, new EndGameConfirmView());
            m_states.Add(GameStateEnum.GameFinished, new GameFinishedView());

            // We are starting with the main menu
            m_currentState = m_states[GameStateEnum.MainMenu];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            KeyboardPersistence.getPersistedActionToKey();
            ScorePersistence.getPersistedScores();

            // Give all game states a chance to load their content
            foreach (var item in m_states)
            {
                item.Value.loadContent(this.Content);
                item.Value.initialize(this.GraphicsDevice, m_graphics);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            m_nextStateEnum = m_currentState.processInput(gameTime);
            // Special case for exiting the game
            if (m_nextStateEnum == GameStateEnum.Exit)
            {
                Exit();
            }

            m_currentState.update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_currentState.render(gameTime);

            if (m_currentState != m_states[m_nextStateEnum])
            {
                //if were resuming from pause menu dont reset
                if (!(m_currentState == m_states[GameStateEnum.EndGameConfirm] && m_nextStateEnum == GameStateEnum.GamePlay))
                {
                    m_currentState = m_states[m_nextStateEnum];
                    m_currentState.initializeSession();
                }
                else {

                    m_currentState = m_states[m_nextStateEnum];
                }
            }

            base.Draw(gameTime);
        }

        protected override void EndRun()
        {
            base.EndRun();

            //save the control bindings
            KeyboardPersistence.persistActionToKey();
            ScorePersistence.persistScores();
        }
    }
}
