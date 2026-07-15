using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace ZakhanSpellsPack
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        public GameObject ExplosionPrefab;
        public float DestroyExplosion = 4.0f;
        public float DestroyChildren = 2.0f;
        public Vector2 Velocity;

        private Transform spellProjectilePrefab;


        public Action<Unit> OnProjectileDestroyed;

        Rigidbody rb;
        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            // rb.linearVelocity = Velocity;

        }

        public void SetVelocity(Vector2 velocity, float moveDuration)
        {
            Vector3 targetPos = transform.position + (Vector3)velocity * moveDuration;
            transform.DOMove(targetPos, moveDuration).SetSpeedBased(false);
        }

        void OnCollisionEnter(Collision collider)
        {
            if (collider.gameObject.name == gameObject.name)
            {
                return;
            }
            print("Projectile collided with " + collider.gameObject.name);
            PoolManager.Instance.Return(spellProjectilePrefab, transform);
            var exp = Instantiate(ExplosionPrefab, transform.position, ExplosionPrefab.transform.rotation);
            Destroy(exp, DestroyExplosion);
            Transform child;
            child = transform.GetChild(0);
            // transform.DetachChildren();
            // Destroy(child.gameObject, DestroyChildren);
            StartCoroutine(Setfalseafterdelay(DestroyChildren, child.gameObject));
            if (collider.transform.TryGetComponent(out Unit targetUnit))
            {
print("Projectile collided with adsfadf" + collider.gameObject.name);
                OnProjectileDestroyed?.Invoke(targetUnit);

            }
        }

        public IEnumerator Setfalseafterdelay(float delay, GameObject gameObject)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }

        public void OnSpawn()
        {
            transform.DOKill();
        }

        public void OnDespawn()
        {
            transform.DOKill();
            // OnProjectileDestroyed = null;
        }

        public void SetSpellProjectilePrefab(Transform spellProjectilePrefab)
        {
            this.spellProjectilePrefab = spellProjectilePrefab;
        }
    }
}
