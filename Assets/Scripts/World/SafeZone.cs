using UnityEngine;
using DeltaFire.Player;

namespace DeltaFire.World
{
    public class SafeZone : MonoBehaviour
    {
        [SerializeField] private float startRadius = 120f;
        [SerializeField] private float endRadius = 8f;
        [SerializeField] private float shrinkDuration = 300f;
        [SerializeField] private float damagePerSecond = 5f;

        private float elapsed;
        public float Radius { get; private set; }

        private void Start() => Radius = startRadius;

        private void Update()
        {
            elapsed += Time.deltaTime;
            Radius = Mathf.Lerp(startRadius, endRadius, Mathf.Clamp01(elapsed / shrinkDuration));

            foreach (Health health in FindObjectsOfType<Health>())
            {
                Vector3 delta = health.transform.position - transform.position;
                delta.y = 0f;
                if (delta.magnitude > Radius) health.Damage(damagePerSecond * Time.deltaTime);
            }

            transform.localScale = new Vector3(Radius * 2f, 0.05f, Radius * 2f);
        }
    }
}
