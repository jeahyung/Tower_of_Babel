using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
namespace ParticleEffect.Scripts
{
    public class ShieldV2 : MonoBehaviour
    {
        [SerializeField] private float shieldLifeTime = 5f;
        [SerializeField] private float crackTime = 3f;
        [ColorUsage(true, true)]
        [SerializeField] private Color dyingColor;

        [SerializeField] private MeshRenderer shieldMeshRenderer;

        [SerializeField] private GameObject fracturedShield;

        [SerializeField] private float explosionPower = 200;
        [SerializeField] private float explosionRadius = 50;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(DestroyShield());
        }

        IEnumerator DestroyShield()
        {
            yield return new WaitForSeconds(shieldLifeTime);
            shieldMeshRenderer.material.DOColor(dyingColor, "_Color", crackTime);
            float crackAmount = 0f;
            DOTween.To(
                () =>
                {
                    if (shieldMeshRenderer != null)
                    {
                        return shieldMeshRenderer.material.GetFloat("_CrackAmount");
                    }
                    return 0;
                },
                (val) =>
                {
                    if (shieldMeshRenderer != null)
                    {
                        shieldMeshRenderer.material.SetFloat("_CrackAmount", val);
                        crackAmount = val;
                    }

                },
                1,
                crackTime).From(0).OnComplete(() =>
                {
                    var shieldVfx = Instantiate(fracturedShield, this.transform.position, this.transform.rotation);
                    foreach (Rigidbody rb in shieldVfx.GetComponentsInChildren(typeof(Rigidbody)))
                    {
                        if (rb != null)
                        {
                            rb.AddExplosionForce(explosionPower, this.transform.position, explosionRadius, 0, ForceMode.Impulse);
                        }
                    }
                    Destroy(shieldVfx, 2f);
                    Destroy(this.gameObject);
                });
        }

       
    }

}
