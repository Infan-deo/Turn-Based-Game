using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 3f;
        }

    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakingEnemyAiAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // all enemies taken there action
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;

        }

    }

    public void SetStateTakingTurn(bool s)
    {
        timer = 1f;
        state = State.TakingTurn;

    }

    public bool TryTakingEnemyAiAction(Action<bool> onEnemyAIActionComplete)
    {
       
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyList())
        {
            if (TryTakingEnemyAiAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }
        return false;
    }

    public bool TryTakingEnemyAiAction(Unit enemyUnit, Action<bool> onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                // Enemy cannot afford this action
                continue;
            }
            if (bestEnemyAIAction == null)
            {
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction testEnemyAiAction = baseAction.GetBestEnemyAIAction();
                if (testEnemyAiAction != null && testEnemyAiAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAiAction;
                    bestBaseAction = baseAction;
                }

            }

        }

        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
