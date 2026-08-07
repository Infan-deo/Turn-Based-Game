using System;
using DG.Tweening;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Material mat;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void SheildOn()
    {
        

        mat.SetFloat("_Progress", -1.6f);

        DOTween.To(
            () => mat.GetFloat("_Progress"),
            x => mat.SetFloat("_Progress", x),
            0.5f,
            0.5f
        ).SetEase(Ease.OutCubic);
        
        
    }

    public void SheildOff()
    {
        mat.SetFloat("_Progress", 0.5f);

        DOTween.To(
            () => mat.GetFloat("_Progress"),
            x => mat.SetFloat("_Progress", x),
            -1.6f,
            0.5f
        ).SetEase(Ease.OutCubic);
    }

  
}
