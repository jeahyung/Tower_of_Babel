using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INab.WorldAlchemy
{
    public class BarriersDemo : MonoBehaviour
    {
        public float speed = 0.4f;

        public Mask sphere;
        public AnimationCurve scaleCurveSphere;

        public Vector3 minScaleSphere = new Vector3(0.3f, 0.3f, 0.3f);
        public Vector3 maxScaleSphere = new Vector3(1, 1, 1);

        private float elapsedTime;

        public Mask cube;

        public AnimationCurve scaleCurveCube;
        public Vector3 minScaleCube = new Vector3(0.3f, 0.3f, 0.3f);
        public Vector3 maxScaleCube = new Vector3(1, 1, 1);

        public List<Transform> rockSpawnPositions = new List<Transform>();
        public GameObject rockPrefab;
        private bool restarting = false;

        public GameObject projectilePrefab; // The projectile prefab to shoot
        public Transform shootPoint; // The point from which the projectile will be shot
        public float shootForce = 1000f; // The force applied to the projectile to propel it forward

        public LayerMask mask;

        // Start is called before the first frame update
        void Start()
        {
            sphere.transform.localScale = minScaleSphere;
            cube.transform.localScale = minScaleCube;
        }

        void ShootProjectile()
        {
            var myCamera = Camera.main;

            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 300, mask.value))
            {
                // Instantiate the projectile at the shootPoint's position and rotation
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);

                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    var hitDir = (hit.point - transform.position).normalized;
                    rb.AddForce(hitDir * shootForce);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Check for the fire button (left mouse button in this case)
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ShootProjectile();
            }

            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                int index = Random.Range(0, rockSpawnPositions.Count);

                GameObject rock = Instantiate(rockPrefab, rockSpawnPositions[index].position, Quaternion.identity);
            }   

            if (Input.GetKeyDown(KeyCode.R))
            {
                restarting = true;
            }

            if(restarting)
            {
                if (elapsedTime > 1) restarting = false;
                elapsedTime += Time.deltaTime * speed;

                float curveValue = scaleCurveSphere.Evaluate(elapsedTime);
                sphere.transform.localScale = Vector3.Lerp(minScaleSphere, maxScaleSphere, curveValue);

                float curveValueCube = scaleCurveCube.Evaluate(elapsedTime);
                cube.transform.localScale = Vector3.Lerp(minScaleCube, maxScaleCube, curveValueCube);
            }


        }
    }
}