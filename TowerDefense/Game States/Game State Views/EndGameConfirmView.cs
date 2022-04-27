using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using CS5410.Persistence;

namespace CS5410
{
    public class EndGameConfirmView: MenuView
    {
        new public enum MenuState
        {
            Yes,
            No,
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
                if (!m_waitForKeyRelease && Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    //make sure this gets set so that menu navigation doesnt wig out
                    m_waitForKeyRelease = true;

                    if (m_currentSelection == (int)MenuState.Yes)
                    {

                    GamePlayView.done();
                        return GameStateEnum.MainMenu;
                    }
                    else if (m_currentSelection == (int)MenuState.No)
                    {
                        return GameStateEnum.GamePlay;
                    }
                }
            return GameStateEnum.EndGameConfirm;
        }
        public override void update(GameTime gameTime)
        {
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

                // I split the first one's parameters on separate lines to help you see them better

                float bottom = drawMenuItem(m_fontMenu, "End the current game?", 200, Color.Red);
                bottom = drawMenuItem(m_fontMenu, "Current Score: " + ScorePersistence.score, bottom, Color.Red);
                bottom += 25;

                bottom = drawSelectedMenuItem("Yes: End Game",  bottom, m_currentSelection == (int)MenuState.Yes);
                bottom = drawSelectedMenuItem("No: Dont End Game",  bottom, m_currentSelection == (int)MenuState.No);
                

            m_spriteBatch.End();
        }

        protected override int getMinMenuOption()
        {
            return (int)MenuState.Yes;
        }

        protected override int getMaxMenuOption()
        {
            return (int)MenuState.No;
        }

    }
}
