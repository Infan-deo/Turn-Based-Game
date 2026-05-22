using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        if (UnitManager.Instance.GetFriendlyList().Count <= 0)
        {
            print("Won");
        }
        else if (UnitManager.Instance.GetEnemyList().Count <= 0)
        {
            print("Lost");
        }
        else
            OnTurnChanged.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
