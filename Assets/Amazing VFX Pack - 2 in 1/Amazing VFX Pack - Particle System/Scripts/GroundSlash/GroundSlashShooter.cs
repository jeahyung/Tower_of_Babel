using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleEffect.Scripts
{
    public class GroundSlashShooter : MonoBehaviour
    {
        public float dad;
        public GameObject projectile;
        public Transform firePoint;
        public float fireRate = 4f;

        private Vector3 destination;
        private float timeToFire;
        private GroundSlash groundSlashScript;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time >= timeToFire)
                {
                    timeToFire = Time.time + 1 / fireRate;
                    SpawnProjectile();
                }
            }
        }

        private void SpawnProjectile()
        {
            var projectile = Instantiate(this.projectile, firePoint.position, Quaternion.identity) as GameObject;

            groundSlashScript = projectile.GetComponent<GroundSlash>();
            RotateToDestination(projectile, transform.forward, true);
            projectile.GetComponent<Rigidbody>().velocity = transform.forward * groundSlashScript.speed;
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