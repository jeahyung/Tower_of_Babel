using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INab.WorldAlchemy
{
    public class DestroyProjectile : MonoBehaviour
    {
        private bool isSticking = false;
        private float timer = 0f;
        public float duration = 0.3f;

        void Update()
        {
            if (isSticking)
            {
                timer += Time.deltaTime;

                if (timer >= duration)
                {
                    timer = 0f;
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isSticking)
            {
                isSticking = true;
            }
        }
    }
}