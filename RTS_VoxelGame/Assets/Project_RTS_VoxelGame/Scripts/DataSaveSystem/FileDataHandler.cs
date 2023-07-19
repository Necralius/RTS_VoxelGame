using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace NekraliusDevelopmentStudio
{
    public class FileDataHandler
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        public string dataPath = "";

        public string dataFileName = "";

        public FileDataHandler(string dataPath, string dataFileName)
        {
            this.dataPath = dataPath;
            this.dataFileName = dataFileName;
        }

        public GameStateData Load()
        {
            string fullPath = Path.Combine(dataPath, dataFileName);
            GameStateData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using(StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();    
                        }
                    }
                    loadedData = JsonUtility.FromJson<GameStateData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath);
                    Debug.LogError("Exception -> " + e);
                }
            }
            return loadedData;
        }
        public void Save(GameStateData data)
        {
            string fullPath = Path.Combine(dataPath, dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string DataToStore = JsonUtility.ToJson(data, true);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create)) 
                {
                    using (StreamWriter writer = new StreamWriter(stream)) writer.Write(DataToStore);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath);
                Debug.LogError("Expection -> " + e);
            }
        }
    }
}