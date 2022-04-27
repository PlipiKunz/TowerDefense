using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Systems;

namespace CS5410.Persistence
{
    public static class KeyboardPersistence
    {

        private static PersistControls p = new PersistControls();

        public static Dictionary<KeyboardActions, Keys> actionToKey = new Dictionary<KeyboardActions, Keys>() {
            {KeyboardActions.Upgrade, Keys.U },
            {KeyboardActions.Sell, Keys.S },
            {KeyboardActions.Next, Keys.G },
            {KeyboardActions.Music, Keys.M },
        };


        public static Boolean loaded = false;

        public static void getPersistedActionToKey() {
            p.loadControls();
            while (!loaded)
            {
                if (PersistControls.m_loadedControls != null)
                {
                    actionToKey = PersistControls.m_loadedControls;
                    loaded = true;
                }
                else if (PersistControls.m_loadedControls == null && PersistControls.controlsExists == false)
                {
                    loaded = true;
                }
            }
        }

        public static void persistActionToKey()
        {
            p.saveControls();
        }

        public static void bind(KeyboardActions ka, Keys k)
        {
            actionToKey[ka] = k;
        }
    }
}
