using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Intance;
    

    private void Awake() {
        if (Intance == null)
        {
            Intance = this;
        }
    }

    [SerializeField] LayerMask mousePlaneLayerMask;
   

    public static Vector3 Getposition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, Intance.mousePlaneLayerMask);
        return hitInfo.point;
    }
}
