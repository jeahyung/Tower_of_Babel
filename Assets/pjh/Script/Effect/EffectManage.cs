using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManage : MonoBehaviour
{
    public static EffectManage Instance { get; private set; }

    private Dictionary<string, ParticleSystem> effectDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 이펙트 딕셔너리 초기화
            effectDictionary = new Dictionary<string, ParticleSystem>();
        }
        {
            Destroy(gameObject);
            return;
        }

       
    }

    public void RegisterEffect(string effectName, ParticleSystem effect)
    {
        if (!effectDictionary.ContainsKey(effectName))
        {
            effectDictionary.Add(effectName, effect);
        }
    }

    // 이펙트를 재생하는 메서드
    public void PlayEffect(string effectName, Vector3 position)
    {
        if (effectDictionary.ContainsKey(effectName))
        {
            ParticleSystem effect = effectDictionary[effectName];
            ParticleSystem instance = Instantiate(effect, position, Quaternion.identity);
            instance.Play();
            Destroy(instance.gameObject, effect.main.duration);
        }
        else
        {
            Debug.LogWarning("EffectManager: Effect not found - " + effectName);
        }
    }
}
