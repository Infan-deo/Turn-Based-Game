using System.Collections.Generic;
using UnityEngine;
using Ami.BroAudio;

public class Testing : MonoBehaviour
{
    [SerializeField] Unit unit;
    [SerializeField] SoundID _sfx = default;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BroAudio.Play(_sfx).AsDominator();

        }
    }


}
