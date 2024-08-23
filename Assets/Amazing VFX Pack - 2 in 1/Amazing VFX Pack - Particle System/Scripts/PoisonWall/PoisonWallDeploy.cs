using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
namespace ParticleEffect.Scripts
{
    public class PoisonWallDeploy : MonoBehaviour
    {
        [SerializeField] private GameObject _viperWallPrefab;
        [SerializeField] private List<Transform> baseSmokes;
        [SerializeField] private List<ParticleSystem> smokeEffects;
        [SerializeField] private List<ParticleSystem> smokeLoopEffects;
        [SerializeField] private bool hasDeploy = true;
        [SerializeField] private float distance;
        [SerializeField] private bool skillHasActive = true;

        private bool _isPlayFirstTime = false;
        private Vector3 startPos;
        private Quaternion currentRotate;


        private void Start()
        {
            foreach (var ps in baseSmokes)
            {
                smokeEffects.Add(ps.GetChild(0).GetComponent<ParticleSystem>());
                smokeLoopEffects.Add(ps.GetChild(2).GetComponent<ParticleSystem>());
            }
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!skillHasActive) return;

                if (hasDeploy)
                {
                    DeploySmoke();
                    this.skillHasActive = false;
                }
                else
                {
                    if (!_isPlayFirstTime)
                    {
                        PlayWallFirstTime();
                        this._isPlayFirstTime = true;
                        this.skillHasActive = false;
                    }
                    else
                    {
                        PlayWall();
                        this.skillHasActive = false;
                    }
                }
            }
        }

        private async void DeploySmoke()
        {
            Vector3 currentPos = transform.forward;
            currentRotate = transform.rotation;
            startPos = new Vector3(transform.position.x, 0, transform.position.z);
            for (int i = 0; i < baseSmokes.Count; i++)
            {
                float dis2Point = (distance / baseSmokes.Count) * i; // Distance between to point
                Vector3 destination = (currentPos * (dis2Point + 1.25f)) + startPos;
                Transform baseSmoke = baseSmokes[i];
                baseSmoke.transform.rotation = currentRotate;
                Vector3 posBegin = destination + new Vector3(0, 10f, 0);

                baseSmoke.gameObject.SetActive(true);
                baseSmoke.DOLocalMove(destination, 0.5f).From(posBegin);

                baseSmoke.GetChild(1).GetComponent<TrailRenderer>().Clear();
                await Task.Delay(100);
            }

            await Task.Delay(400);
            await PlaySmokeEffect();

            this.hasDeploy = false;
            this.skillHasActive = true;
        }

        private PoisonWall _currentViperWall;

        private async void PlayWallFirstTime()
        {
            await PlaySmokeEffect(true);

            GameObject wall = Instantiate(_viperWallPrefab.gameObject, startPos, currentRotate);
            PoisonWall viperWall = wall.GetComponent<PoisonWall>();
            this._currentViperWall = viperWall;
            await this._currentViperWall.GrowUpWallFirstTime(distance, startPos);
            await PlaySmokeLoopEffect();
            this.skillHasActive = true;
        }

        private async void PlayWall()
        {
            if (!_currentViperWall.IsGrowUp())
            {
                await PlaySmokeEffect(true);
                await Task.Delay(200);
                await PlaySmokeLoopEffect();
            }
            else
            {
                await PlaySmokeLoopEffect(false);
            }

            await _currentViperWall.GrowUpWall();
            this.skillHasActive = true;
        }

        private async Task PlaySmokeLoopEffect(bool growUp = true)
        {
            if (growUp)
            {
                foreach (var ps in smokeLoopEffects)
                {
                    ps.Play();
                    await Task.Delay(100);
                }
            }
            else
            {
                foreach (var ps in smokeLoopEffects)
                {
                    ps.Stop();
                }
            }
        }

        private async Task PlaySmokeEffect(bool deplay = false)
        {
            if (!deplay)
            {
                foreach (var ps in smokeEffects)
                {
                    ps.Play();
                }
            }
            else
            {
                foreach (var ps in smokeEffects)
                {
                    ps.Play();
                    await Task.Delay(100);
                }
            }

            await Task.Yield();
        }
    }
}