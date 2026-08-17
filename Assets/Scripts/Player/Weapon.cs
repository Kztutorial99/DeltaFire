using UnityEngine;
using DeltaFire.Player;

namespace DeltaFire.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Camera aimCamera;
        [SerializeField] private float damage = 25f;
        [SerializeField] private float range = 150f;
        [SerializeField] private float shotsPerSecond = 8f;
        [SerializeField] private int magazineSize = 30;
        [SerializeField] private float reloadTime = 1.6f;

        private int ammo;
        private float nextShot;
        private bool reloading;

        private void Awake()
        {
            ammo = magazineSize;
            if (!aimCamera) aimCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(Reload());
            if (Input.GetButton("Fire1")) Fire();
        }

        private void Fire()
        {
            if (reloading || ammo <= 0 || Time.time < nextShot) return;
            nextShot = Time.time + 1f / shotsPerSecond;
            ammo--;
            if (!aimCamera) return;

            Ray ray = aimCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                Health target = hit.collider.GetComponentInParent<Health>();
                if (target) target.Damage(damage);
            }
        }

        private System.Collections.IEnumerator Reload()
        {
            if (reloading || ammo == magazineSize) yield break;
            reloading = true;
            yield return new WaitForSeconds(reloadTime);
            ammo = magazineSize;
            reloading = false;
        }
    }
}
