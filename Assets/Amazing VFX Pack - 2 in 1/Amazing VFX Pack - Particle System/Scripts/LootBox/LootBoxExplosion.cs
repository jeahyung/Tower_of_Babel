using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
namespace ParticleEffect.Scripts
{
    public class LootBoxExplosion : MonoBehaviour
    {
        public float minForce;
        public float maxForce;
        public float radius;
    
        private void Start()
        {
            Explosion();
        }
    
        private void Explosion()
        {
            foreach (Transform t in this.transform)
            {
                var rig = t.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.AddExplosionForce(Random.Range(minForce,maxForce), this.transform.position, radius);
                }
            }
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }
}

