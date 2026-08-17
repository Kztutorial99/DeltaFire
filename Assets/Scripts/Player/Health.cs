using UnityEngine;

namespace DeltaFire.Player
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        public float Current { get; private set; }
        public bool IsDead => Current <= 0f;

        private void Awake() => Current = maxHealth;

        public void Damage(float amount)
        {
            if (IsDead) return;
            Current = Mathf.Max(0f, Current - Mathf.Max(0f, amount));
            if (IsDead) gameObject.SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
        }

        public void Heal(float amount) => Current = Mathf.Min(maxHealth, Current + Mathf.Max(0f, amount));
    }
}
