using DG.Tweening;
using UnityEngine;

public class PlayerUnitMovement : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private float stopanimationOffset;


    private void Awake()
    {
        targetPosition = transform.position;
    }
    public void MovePlayer(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            unitAnimator.SetBool("IsWalking", true);

        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }


    }
}
