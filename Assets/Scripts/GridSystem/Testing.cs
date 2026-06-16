using System.Collections.Generic;
using UnityEngine;
using Ami.BroAudio;
using TBGame;

public class Testing : MonoBehaviour
{
    [SerializeField] Unit unit;
    [SerializeField] SoundID _sfx = default;

    public List<SpellInfo> spellInfos;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            EventBus<SceneEvent>.Raise(new SceneEvent
            {
                currentSceneIndex = SceneController.Instance.GetcurrentSceneIndex(),
                currentSceneName = SceneController.Instance.Sceneinfo.name
            });

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            EventBus<SpellUIEvent>.Raise(new SpellUIEvent
            {
                spellInfos1 = spellInfos
            });

        }
    }


}
