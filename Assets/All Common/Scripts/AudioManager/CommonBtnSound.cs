using Ami.BroAudio;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CommonBtnSound : MonoBehaviour
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(() =>
        {
            PlayAudio();
        });
    }

    void PlayAudio()
    {
        if (MusicManager.Instance != null)
        {
            BroAudio.Play(MusicManager.Instance.CommonUIBtnSound);
        }
    }


}
