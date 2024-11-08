using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace ParticleEffect.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class FireWallProjectile : MonoBehaviour
    {
        public float speed = 50;
        public float destroyDelay = 3.5f;
        public GameObject stayPrefabs;
        public List<GameObject> detachables;
    
        private Rigidbody rb;
        private ParticleSystem.EmitParams emitParam;
        private bool stop;
    
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(Stop());
            stayPrefabs.transform.parent = null;
            Destroy(stayPrefabs, 6f);
        }
    
        private void FixedUpdate()
        {
            if (speed != 0 && rb != null)
            {
                rb.position += (transform.forward) * (speed * Time.deltaTime);
            }
        }
    
        private IEnumerator Stop()
        {
            yield return new WaitForSeconds(destroyDelay);
            for (int i = 0; i < detachables.Count; i++)
            {
                if (detachables[i] != null)
                {
                    detachables[i].transform.parent = null;
                    Destroy(detachables[i], destroyDelay + 5f);
                }
            }
            Destroy(this.gameObject);
        }
    }
}
