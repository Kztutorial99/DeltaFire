using UnityEngine;
using DeltaFire.Player;
using DeltaFire.Combat;

namespace DeltaFire.AI
{
    [RequireComponent(typeof(Health))]
    public class BotController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2.8f;
        [SerializeField] private float attackRange = 35f;
        [SerializeField] private float attackInterval = 1.2f;
        [SerializeField] private float damage = 12f;

        private Transform target;
        private float nextAttack;
        private Health health;

        private void Awake() => health = GetComponent<Health>();

        private void Update()
        {
            if (health.IsDead) return;
            if (!target)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player) target = player.transform;
                return;
            }

            Vector3 flat = target.position - transform.position;
            flat.y = 0f;
            float distance = flat.magnitude;
            if (distance > attackRange)
            {
                transform.position += flat.normalized * moveSpeed * Time.deltaTime;
                if (flat.sqrMagnitude > .01f) transform.rotation = Quaternion.LookRotation(flat);
            }
            else if (Time.time >= nextAttack)
            {
                nextAttack = Time.time + attackInterval;
                Health playerHealth = target.GetComponent<Health>();
                if (playerHealth) playerHealth.Damage(damage);
            }
        }
    }
}
