using System;
using System.Collections.Generic;
using System.Text;
//
// Added to support serialization
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

using Microsoft.Xna.Framework.Input;
using Systems;

namespace CS5410.Persistence
{
    public class PersistControls
    {
        static private bool saving = false;
        static private bool loading = false;

        static private string controlsFileName = "Controls2.xml";
        static public bool? controlsExists = null;
        static public Dictionary<KeyboardActions, Keys> m_loadedControls = null;

        public void saveControls()
        {
            lock (this)
            {
                if (!saving)
                {
                    saving = true;
                    finalizeSaveControlsAsync(JsonConvert.SerializeObject(KeyboardPersistence.actionToKey));
                }
            }
        }
        private async void finalizeSaveControlsAsync(String dict)
        {

            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile(controlsFileName, FileMode.OpenOrCreate))
                        {
                            if (fs != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(String));
                                mySerializer.Serialize(fs, dict);
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                saving = false;
            });
        }

        public void loadControls()
        {
            lock (this)
            {
                if (!loading)
                {
                    loading = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    finalizeLoadControlsAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }
        private async Task finalizeLoadControlsAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists(controlsFileName))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile(controlsFileName, FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(String));

                                    m_loadedControls = JsonConvert.DeserializeObject<Dictionary<KeyboardActions, Keys>>((String)mySerializer.Deserialize(fs));

                                    controlsExists = true;
                                }
                                else
                                {
                                    controlsExists = false;
                                }
                            }
                        }
                        else
                        {
                            controlsExists = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        controlsExists = false;
                    }
                }

                loading = false;
            });
        }
    }

    public class PersistScore {
        static private bool saving = false;
        static private bool loading = false;
        static private string scoresFileName = "Scores2.xml";
        static public bool? scoresExists = null;
        static public List<int> m_loadedScores = null;

        public void saveScores()
        {
            lock (this)
            {
                if (!PersistScore.saving)
                {
                    PersistScore.saving = true;
                    finalizeSaveScoresAsync(JsonConvert.SerializeObject(ScorePersistence.scores));
                }
            }
        }
        private async void finalizeSaveScoresAsync(String dict)
        {

            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile(PersistScore.scoresFileName, FileMode.OpenOrCreate))
                        {
                            if (fs != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(String));
                                mySerializer.Serialize(fs, dict);
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                PersistScore.saving = false;
            });
        }

        public void loadScores()
        {
            lock (this)
            {
                if (!PersistScore.loading)
                {
                    PersistScore.loading = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    finalizeLoadScoresAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }
        private async Task finalizeLoadScoresAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists(PersistScore.scoresFileName))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile(PersistScore.scoresFileName, FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(String));

                                    m_loadedScores = JsonConvert.DeserializeObject<List<int>>((String)mySerializer.Deserialize(fs));

                                    scoresExists = true;
                                }
                                else
                                {
                                    scoresExists = false;
                                }
                            }
                        }
                        else
                        {
                            scoresExists = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        scoresExists = false;
                    }
                }

                PersistScore.loading = false;
            });
        }
    }
}
