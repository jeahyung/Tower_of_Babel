using UnityEngine;

namespace Amazing_VFX_Pack___Visual_Effects.Scripts.GroundSlash
{
    public class GroundSlashShooter : MonoBehaviour
    {
        public GameObject projectile;
        public Transform firePoint;
        public float fireRate = 4f;

        private Vector3 _destination;
        private float _timeToFire;
        private GroundSlash _groundSlashScript;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time >= _timeToFire)
                {
                    _timeToFire = Time.time + 1 / fireRate;
                    ShootProjectile();
                }
            }
        }

        private void ShootProjectile()
        {
            _destination = transform.forward;
            SpawnProjectile();
        }

        private void SpawnProjectile()
        {
            var projectile = Instantiate(this.projectile, firePoint.position, Quaternion.identity) as GameObject;

            _groundSlashScript = projectile.GetComponent<GroundSlash>();
            RotateToDestination(projectile, _destination, true);
            projectile.GetComponent<Rigidbody>().velocity = transform.forward * _groundSlashScript.speed;
        }

        private void RotateToDestination(GameObject obj, Vector3 destination, bool onlyY)
        {
            var direction = destination - obj.transform.position;
            var rotation = Quaternion.LookRotation(direction);
            if (onlyY)
            {
                rotation.x = 0;
                rotation.z = 0;
            }

            obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
        }
    }
}