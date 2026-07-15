using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public struct SceneEvent : IEvent
{
    public int currentSceneIndex;
    public string currentSceneName;
}
namespace TBGame
{
    public class SceneController : Singleton<SceneController>
    {


        private int currentSceneIndex;
        EventBinding<SceneEvent> sceneEventBinding;
        public Scene Sceneinfo;
        void OnEnable()
        {
            sceneEventBinding = new EventBinding<SceneEvent>(HandlePlayerEvent);
            EventBus<SceneEvent>.Register(sceneEventBinding);

            // Can Add or Remove Actions to/from the EventBinding
        }

        private void HandlePlayerEvent(SceneEvent @event)
        {
            print("CurrenSceneIndex :" + @event.currentSceneIndex + "  CurrentSceneName: " + @event.currentSceneName);
        }

        void OnDisable()
        {
            EventBus<SceneEvent>.Deregister(sceneEventBinding);
        }
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
            Sceneinfo = arg0;
            currentSceneIndex = arg0.buildIndex;
            if (arg0.name == "GameScene")
                CircleFadeTransition.Instance.CircleFadeOut();
            if (arg0.buildIndex == 0)
                CircleFadeTransition.Instance.CircleFadeOut();
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(1,LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(0);
        }
        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(0,LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(1);
        }




    }
}
