using UnityEngine;
using UnityEngine.Assertions.Must;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void Setup(Transform originalRootBone,Vector3 enemyPos)
    {
        MatchAllChildTransform(originalRootBone, ragdollRootBone);
        float offSet = 0.5f;
        Vector3 explosionPosition = (((enemyPos - this.transform.position).normalized) * offSet) + this.transform.position;

        ApplyExplosionToRagdoll(ragdollRootBone, 500f, explosionPosition, 10f);
    }


    public void MatchAllChildTransform(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransform(child, cloneChild);
            }
        }
    }

    public void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }

    }

    

}
