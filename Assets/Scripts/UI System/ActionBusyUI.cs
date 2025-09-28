using System;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyStateChanged += UnitActionSystem_OnBusyStateChanged;
        gameObject.SetActive(false);
    }

    private void UnitActionSystem_OnBusyStateChanged(object sender, bool e)
    {
        gameObject.SetActive(e);
    }
}
