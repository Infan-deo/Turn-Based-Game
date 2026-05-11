using UnityEngine;


public class GameSceneManager : MonoBehaviour
{
    public void ToggleSettingsPage()
    {
        CommonGameManager.Instance.ToggleSettingsPage();
    }
}
