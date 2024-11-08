using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace ParticleEffect.Scripts
{
    public class FireWall : MonoBehaviour
    {
        public GameObject projectile;
        public GameObject firePoint;
        public GameObject wall;
        public float wallDelay = 0.5f;
        public float destroyDelay;
    
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(SpawnProjectile());
            }
        }
    
        IEnumerator SpawnProjectile()
        {
            var position = firePoint.transform.position;
            var projectileVFX = Instantiate(projectile, new Vector3(position.x, 0, position.z), Quaternion.identity);
            RotateToMouse(projectileVFX, Vector3.forward, true);
    
            yield return new WaitForSeconds(wallDelay);
            var wallVFX = Instantiate(wall, new Vector3(position.x, 0, position.z), Quaternion.identity);
            RotateToMouse(wallVFX, Vector3.forward, true);
            var wallAnim = wallVFX.transform.GetComponent<Animator>();
            yield return new WaitForSeconds(destroyDelay - 1.5f);
            wallAnim.SetBool("Close", true);
            yield return new WaitForSeconds(2f);
            Destroy(wallVFX);
        }
    
        void RotateToMouse(GameObject obj, Vector3 destination, bool lockY = false)
        {
            var direction = destination - obj.transform.position;
            var rotation = Quaternion.LookRotation(direction);
    
            if (lockY)
            {
                rotation.z = 0;
                rotation.x = 0;
            }
    
            obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
        }
    }
}
