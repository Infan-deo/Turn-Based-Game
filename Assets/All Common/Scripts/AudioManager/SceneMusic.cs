using UnityEngine;

public class SceneMusic : MonoBehaviour
{

    [SerializeField] private BgmData bgm;

    private void Start()
    {
        MusicManager.Instance.Play(bgm);
    }
}

