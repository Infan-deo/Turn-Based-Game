using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace TBGame
{
    public class SceneController : Singleton<SceneController>
    {

        private int currentSceneIndex;
        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public int GetcurrentSceneIndex()
        {
            return currentSceneIndex;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            currentSceneIndex = arg0.buildIndex;
            if (arg0.name == "GameScene")
                CircleFadeTransition.Instance.CircleFadeOut();
            if (arg0.buildIndex == 0)
                CircleFadeTransition.Instance.CircleFadeOut();
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(1);
        }
        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(0);
        }


    }
}
