using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ParticleEffect.Scripts
{
    public class Shield : MonoBehaviour
    {
        public GameObject ripplesVFX;
    
        private Material mat;
    
        // private void OnCollisionEnter(Collision collision)
        // {
        //     if (collision.gameObject.tag == "Bullet")
        //     {
        //         var ripple = Instantiate(ripplesVFX, transform.position, transform.rotation) as GameObject;
        //         var psr = ripple.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
        //         mat = psr.material;
        //         mat.SetVector("_SphereCenter", collision.contacts[0].point);
        //         Destroy(ripple, 2);
        //         Debug.Log("Collision");
        //     }
        // }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                var ripple = Instantiate(ripplesVFX, transform.position, transform.rotation) as GameObject;
                var psr = ripple.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
                mat = psr.material;
                mat.SetVector("_SphereCenter", other.ClosestPoint(this.transform.position));
                Destroy(ripple, 2);
                Debug.Log("Collision");
            }
        }

}
}
