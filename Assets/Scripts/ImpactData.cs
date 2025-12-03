using UnityEngine;

public struct ImpactData
{
    public Vector3 position;
    public Vector3 direction;

    public ImpactData(Vector3 pos, Vector3 dir)
    {
        position = pos;
        direction = dir.normalized;
    }
}
