using System;
using UnityEngine;

public class PathfinderUpdater : MonoBehaviour
{
    private void Start()
    {
        DestructableCrate.OnAnyDestroyed += DestructableCrate_OnAnyDestroyed;
    }

    private void DestructableCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructableCrate destructableCrate = sender as DestructableCrate;
        GridPosition destructableCratePosition = destructableCrate.GetGridPosition();
        Pathfinding.Instance.SetisWalkableGridPostion(destructableCratePosition, true);
    }
}
