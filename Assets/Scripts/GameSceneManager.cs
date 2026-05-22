using UnityEngine;


public class GameSceneManager : MonoBehaviour
{
    public void ToggleSettingsPage()
    {
        if (CommonGameManager.Instance != null)
        {
            CommonGameManager.Instance.ToggleSettingsPage();
        }
    }
}
