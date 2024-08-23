using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ParticleEffect.Scripts
{
    public class ProjectileMove : MonoBehaviour
    {
        public float speed;
    
        public float fireRate;
        public GameObject hitPrefab;
        public float hitLifeTime;
    
        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            if (speed != 0)
            {
                transform.position += transform.forward * (Time.deltaTime * speed);
            }
        }
    
        private void OnCollisionEnter(Collision col)
        {
            speed = 0;
            ContactPoint contact = col.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
    
            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, pos, rot);
                Destroy(hitVFX, hitLifeTime);
            }
            Destroy(gameObject);
        }
    }
}

