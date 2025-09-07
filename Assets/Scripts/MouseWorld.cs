using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Intance;
    [SerializeField] LayerMask mousePlaneLayerMask;
  
    

    private void Awake() {
        if (Intance == null)
        {
            Intance = this;
        }
    }

   

    public static Vector3 Getposition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, Intance.mousePlaneLayerMask);
        return hitInfo.point;
    }
    
}
