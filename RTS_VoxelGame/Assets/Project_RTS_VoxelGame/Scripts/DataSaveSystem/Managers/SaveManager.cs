using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NekraliusDevelopmentStudio
{
    public class SaveManager : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //SaveManager - (0.1 Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern -
        public static SaveManager Instance;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
            else Destroy(gameObject);
        }
        #endregion

        public GameStateData gameData;
        private FileDataHandler fileDataHandler;

        public string fileName;

        public List<ILoadableData> loadableDatas = new List<ILoadableData>();

        private void Start()
        {
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

            LoadGameData();            
        }
        private List<ILoadableData> GetAllLoadables()
        {
            IEnumerable<ILoadableData> dataLoadables = FindObjectsOfType<MonoBehaviour>().OfType<ILoadableData>();
            return new List<ILoadableData>(dataLoadables);
        }

        public void LoadAllData()
        {
            loadableDatas = GetAllLoadables();
            foreach (var data in loadableDatas) data.Load(gameData);
        }

        private void OnLevelWasLoaded(int level)
        {
            LoadAllData();
        }

        public GameStateData LoadGameData()
        {
            gameData = fileDataHandler.Load();
            return gameData;
        }
        public void FullGameDataSave()
        {
            gameData = GetGameSave(SaveType.FullSave);
            fileDataHandler.Save(gameData);
        }
        public void MenuGameSave()
        {
            gameData = GetGameSave(SaveType.Settings);
            fileDataHandler.Save(gameData);
        }
        public void SaveStructure(GameObject structureToSave)
        {
            gameData.structureData.Add(new StructureData(structureToSave));
        }
        public GameStateData GetGameSave(SaveType saveType)
        {
            if (saveType.Equals(SaveType.FullSave))
            {
                if (gameData.Equals(null))
                {
                    GameStateData gameStateData = new GameStateData();
                    gameStateData.SaveGame(Camera.main, ResourceManager.Instance, GridWorldGenerator.Instance);
                    return gameStateData;
                }
                gameData.SaveGame(Camera.main, ResourceManager.Instance, GridWorldGenerator.Instance);
                return gameData;
            }
            else if (saveType.Equals(SaveType.Settings))
            {
                if (gameData.Equals(null))
                {
                    GameStateData gameStateData = new GameStateData();
                    SettingsData data = new SettingsData();
                    data.SaveData();
                    gameStateData.SaveGame(data);
                    return gameStateData;
                }

                SettingsData dataSave = new SettingsData();
                dataSave.SaveData();
                gameData.SaveGame(dataSave);
            }
            else if (saveType.Equals(SaveType.StructureSave))
            {


                return gameData;
            }
            return gameData;
        }
        private void OnApplicationQuit()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0) MenuGameSave();
            else FullGameDataSave();
        }
    }
}