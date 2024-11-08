using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ParticleEffect.Scripts
{
    public class IceWall : MonoBehaviour
    {
        public Camera cam;
        public float detectRange = 10;
        [Space][Header("Wall Base")] public GameObject wallBase;
        public GameObject wallGrowUp;
        private Vector3 destination;
        private Quaternion rotation;
        private bool wallBaseActive;

        void Start()
        {
            wallBase.SetActive(false);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!wallBuilding && wallBaseActive)
                {
                    wallBuilding = true;
                    wallBaseActive = false;
                    wallBase.SetActive(wallBaseActive);

                    GameObject wall = new GameObject();
                    wall.transform.position = destination;
                    wall.name = "Wall";

                    List<Material> cubeMaterials = new List<Material>();
                    for (int i = 0; i < wallAmount; i++)
                    {
                        var cube = Instantiate(wallPrefab, destination + new Vector3(i * wallDistance, 0, 0),
                                Quaternion.identity) as GameObject;
                        cube.transform.SetParent(wall.transform);
                        cubeMaterials.Add(cube.GetComponent<MeshRenderer>().material);
                    }


                    wall.transform.rotation = rotation;
                    wall.transform.Translate(new Vector3(-(int)(wallAmount / 2) * wallDistance + wallDistance / 2f, 0, 0), Space.Self);

                    var wallUp = Instantiate(wallGrowUp, destination, rotation);
                    Destroy(wallUp, 1f);

                    wallBuilding = false;

                    StartCoroutine(DestroyWall(wall, cubeMaterials));
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                wallBaseActive = false;
                wallBase.SetActive(wallBaseActive);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                wallBaseActive = true;
                wallBase.SetActive(wallBaseActive);
            }
            if (wallBaseActive)
            {
                UpdateWallBase();
            }
        }

        private void UpdateWallBase()
        {
            destination = Vector3.zero;
            rotation = Quaternion.LookRotation(transform.forward);
            wallBase.transform.position = destination;
            rotation = new Quaternion(wallBase.transform.rotation.x, rotation.y, wallBase.transform.rotation.z,
                wallBase.transform.rotation.w);
            wallBase.transform.rotation = rotation;
            wallBase.SetActive(true);
        }

        public GameObject wallPrefab;
        public int wallAmount = 4;
        public float wallDistance = 1.7f;
        private bool wallBuilding = false;

        public float wallDuration = 5f;
        public float updateRate = 0.05f;

        [Space][Header("Explosion")] public GameObject wallExplosionPrefab;
        public Vector3 wallExplosionOffset = new Vector3(0, 1.3f, 0);

        IEnumerator DestroyWall(GameObject wallToDestroy, List<Material> materials)
        {
            float duration = wallDuration;
            float crackAmount = 0;

            while (duration > 0)
            {
                duration -= updateRate;

                if (crackAmount < 1)
                {
                    crackAmount += 1 / ((wallDuration - 0.2f) / updateRate);
                    for (int i = 0; i < materials.Count; i++)
                    {
                        materials[i].SetFloat("_CrackAmount", crackAmount);
                    }
                }

                yield return new WaitForSeconds(updateRate);
                if (duration <= 0)
                {
                    for (int i = 0; i < wallToDestroy.transform.childCount; i++)
                    {
                        var explosion = Instantiate(wallExplosionPrefab,
                            wallToDestroy.transform.GetChild(i).position + wallExplosionOffset, Quaternion.identity);
                        Destroy(explosion, 1.5f);
                    }
                    Destroy(wallToDestroy);
                }
            }
        }
    }

}
