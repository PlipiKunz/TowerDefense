using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CS5410
{
    public class MainMenuView : MenuView
    {

        new public enum MenuState
        {
            NewGame,
            HighScores,
            Credits,
            Controls,
            Quit
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_Color = Color.Blue;
            m_selectedColor = Color.Yellow;
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            base.processInput(gameTime);
            // This is the technique I'm using to ensure one keypress makes one menu navigation move
            // If enter is pressed, return the appropriate new state
            if (!m_waitForKeyRelease && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                //make sure this gets set so that menu navigation doesnt wig out
                m_waitForKeyRelease = true;

                if (m_currentSelection == (int)MenuState.NewGame)
                {
                    return GameStateEnum.GamePlay;
                }
                else if (m_currentSelection == (int)MenuState.HighScores)
                {
                    return GameStateEnum.HighScores;
                }
                else if (m_currentSelection == (int)MenuState.Controls)
                {
                    return GameStateEnum.Controls;
                }
                else if (m_currentSelection == (int)MenuState.Credits)
                {
                    return GameStateEnum.Credits;
                }
                else if (m_currentSelection == (int)MenuState.Quit)
                {
                    return GameStateEnum.Exit;
                }
            }
            return GameStateEnum.MainMenu;
        }
        public override void update(GameTime gameTime)
        {
        }
        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // I split the first one's parameters on separate lines to help you see them better
            float bottom = drawSelectedMenuItem("New Game", 200,m_currentSelection == (int)MenuState.NewGame);
            bottom = drawSelectedMenuItem("High Scores", bottom,  m_currentSelection == (int)MenuState.HighScores);
            bottom = drawSelectedMenuItem("Credits", bottom,  m_currentSelection == (int)MenuState.Credits);
            bottom = drawSelectedMenuItem("Controls", bottom,  m_currentSelection == (int)MenuState.Controls);
            bottom = drawSelectedMenuItem("Quit", bottom,  m_currentSelection == (int)MenuState.Quit);

            m_spriteBatch.End();
        }

        protected override int getMinMenuOption() {
            return (int)MenuState.NewGame;
        }

        protected override int getMaxMenuOption()
        {
            return (int)MenuState.Quit;
        }
    }
}