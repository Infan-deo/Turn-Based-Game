using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    protected Unit Unit;
    protected bool isActive;
    protected Action<bool> onActionComplete;

    protected virtual void Awake()
    {
        Unit = GetComponent<Unit>();
    }

}
