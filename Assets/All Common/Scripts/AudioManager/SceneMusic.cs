using UnityEngine;

public class SceneMusic : MonoBehaviour
{

    [SerializeField] private BgmData bgm;
    [SerializeField] private bool playBgm;

    private void Start()
    {
        if (playBgm)
        {
            MusicManager.Instance.Play(bgm);
        }
    }
}

