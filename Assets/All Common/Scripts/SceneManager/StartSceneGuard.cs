using UnityEngine;
using UnityEngine.SceneManagement;

public static class StartSceneGuard
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureStartScene()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "BootStraped Scene")
        {
            SceneManager.LoadScene("BootStraped Scene");
            SceneManager.LoadScene(currentScene,LoadSceneMode.Additive);
        }
    }
}
