using System;
using DG.Tweening;
using UnityEngine;

namespace ParticleEffect.Scripts
{
    public class DeployShieldV2 : MonoBehaviour
    {
        [SerializeField] private GameObject shieldPrefab;
        [SerializeField] private Transform spawnPoint;

        private GameObject shieldVfx;
        private bool shieldActive;
        private Vector3 destination;

        private GameObject currentShield;
       
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!shieldActive)
                {
                    if (shieldPrefab != null)
                    {
                        ActivateShield();
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (shieldActive)
                {
                    if (shieldPrefab != null)
                    {
                        DisableShield();
                    }
                }
            }
        }
        private void ActivateShield()
        {
            shieldActive = true;
            currentShield = Instantiate(shieldPrefab, spawnPoint.position, spawnPoint.rotation);
            currentShield.transform.parent = spawnPoint;
            DOTween.Kill(currentShield.transform);
            currentShield.transform.DOScale(Vector3.one, 0.3f).From(Vector3.zero).SetEase(Ease.OutBack);
        }

        private void DisableShield()
        {
            shieldActive = false;
            Destroy(currentShield);
        }

    }
}
