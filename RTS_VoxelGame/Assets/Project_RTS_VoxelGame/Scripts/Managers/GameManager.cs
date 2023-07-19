using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NekraliusDevelopmentStudio
{
    public class GameManager : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern - 
        public static GameManager Instance;
        void Awake() => Instance = this;
        #endregion

        public GameObject pauseMenu;
        public Button saveButton;
        public bool gameIsPaused;

        private void Start()
        {
            saveButton.onClick.AddListener(delegate { SaveManager.Instance.FullGameDataSave(); });
        }
        public void LoadSceneAsync(int sceneIndex)
        {
            AsyncOperation load = SceneManager.LoadSceneAsync(sceneIndex);
        }
        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void PauseGame()
        {
            pauseMenu.SetActive(true);
            gameIsPaused = true;
        }
        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            gameIsPaused = false;
        }

        public void QuitToDesktop()
        {
            SettingsMenuManager.Instance.onSceneLoad.Invoke();
            SaveManager.Instance.FullGameDataSave();
            Application.Quit();
        }
        public void QuitToMenu()
        {
            SettingsMenuManager.Instance.onSceneLoad.Invoke();
            LoadSceneAsync(0);
        }
    }
}