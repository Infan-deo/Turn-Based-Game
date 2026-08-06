using System;
using DG.Tweening;
using UnityEngine;

public class ShieldSpawn : MonoBehaviour
{
    Material mat;

    void OnEnable()
    {
        mat = GetComponent<Renderer>().material;

        mat.SetFloat("_Progress", -1.6f);

        DOTween.To(
            () => mat.GetFloat("_Progress"),
            x => mat.SetFloat("_Progress", x),
            0.5f,
            0.5f
        ).SetEase(Ease.OutCubic);
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }
}
