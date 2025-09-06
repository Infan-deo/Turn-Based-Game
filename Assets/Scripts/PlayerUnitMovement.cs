using DG.Tweening;
using UnityEngine;

public class PlayerUnitMovement : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float moveSpeed;
    


    public void MovePlayer(Vector3 targetPosition)
    {
        transform.DOMove(targetPosition, moveSpeed);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MovePlayer(MouseWorld.Getposition());
        }
    }
}
