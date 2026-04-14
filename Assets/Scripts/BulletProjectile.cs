using Unity.VisualScripting;
using UnityEngine;
using UnityUtils;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 targetPostion;

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletSparksPrefab;



    public void Setup(Vector3 targetPosition)
    {
        this.targetPostion = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDir = (targetPostion - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPostion);

        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPostion);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPostion;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletSparksPrefab, targetPostion, Quaternion.identity);

        }

    }
}
