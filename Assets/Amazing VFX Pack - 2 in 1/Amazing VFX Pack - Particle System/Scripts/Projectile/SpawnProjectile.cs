using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ParticleEffect.Scripts
{
    public class SpawnProjectile : MonoBehaviour
    {
        public GameObject firePoint;
        public GameObject muzzlePrefab;
        public int projectileIndex;
        public List<GameObject> vfx = new List<GameObject>();
        private GameObject effectToSpawn;
        public RotateToMouse rotateToMouse;
    
        private void Start()
        {
            effectToSpawn = vfx[projectileIndex];
        }
    
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnMuzzle();
                SpawnVFX();
            }
        }
        private void SpawnMuzzle()
        {
            GameObject muzzleVFX;
            if (firePoint != null)
            {
                muzzleVFX = Instantiate(muzzlePrefab, firePoint.transform.position, Quaternion.identity);
                muzzleVFX.transform.localRotation = rotateToMouse.rotation;
                Destroy(muzzleVFX, 1f);
            }
            else
            {
                Debug.Log("No Muzzle");
            }
        }
        private void SpawnVFX()
        {
            GameObject vfx;
            if (firePoint != null)
            {
                vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
                vfx.transform.localRotation = rotateToMouse.rotation;
                Destroy(vfx, 10);
            }
            else
            {
                Debug.Log("No Fire Point");
            }
        }
    }

}
