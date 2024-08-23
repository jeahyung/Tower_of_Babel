using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace ParticleEffect.Scripts
{
    public class PoisonWall : MonoBehaviour
    {
        [SerializeField] private Transform bone1;
        [SerializeField] private Transform bone2;
        [SerializeField] private float growUpDuration;
        private Vector3 _startPos;
        private bool _isGrowUp = false;
    
        public bool IsGrowUp()
        {
            return this._isGrowUp;
        }
        public async Task GrowUpWallFirstTime(float distance, Vector3 startPos)
        {
    
            this._startPos = startPos;
            
            bone1.position = _startPos;
    
            bone1.DOScaleY(3f, 0.4167f).From(0);
    
            bone2.DOScaleY(0, growUpDuration / 2).From(0).OnComplete(() =>
            {
                bone2.DOScaleY(3, growUpDuration / 2).SetEase(Ease.OutQuad);
            });
            
            bone2.DOMove(_startPos + (bone2.forward * distance), growUpDuration).From(_startPos).SetEase(Ease.OutQuad);
            
            this._isGrowUp = true;
    
            TimeSpan time = TimeSpan.FromMilliseconds(growUpDuration * 1000);
            await Task.Delay(time);
        }
        public async Task GrowUpWall()
        {
            if (this._isGrowUp)
            {
                bone1.DOScaleY(0f, 0.6f).SetEase(Ease.InQuad);
                bone2.DOScaleY(0f, 1f).SetEase(Ease.InQuad);
                this._isGrowUp = false;
                await Task.Delay(1000);
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
    
                bone1.DOScaleY(3f, 0.6f).From(0).SetEase(Ease.OutQuad);
    
                bone2.DOScaleY(3, 1).SetEase(Ease.OutQuad);
                this._isGrowUp = true;
            }
    
            await Task.Delay(2000);
        }
    }

}
