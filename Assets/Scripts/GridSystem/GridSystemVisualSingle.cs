using Unity.VisualScripting;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;




    public void Show()
    {
        meshRenderer.enabled = true;
    }
    public void Show(Color32 color, float glow)
    {
        meshRenderer.enabled = true;

        meshRenderer.material.color = new Color32(color.r, color.g, color.b, color.a);
        meshRenderer.material.SetColor("_EmissionColor", meshRenderer.material.color * Mathf.LinearToGammaSpace(glow));
    }
    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}
