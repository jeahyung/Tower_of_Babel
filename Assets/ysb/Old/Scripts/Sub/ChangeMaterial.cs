using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public Material normal;     //�Ϲ�
    public Material luminous;   //�߱�
    public GameObject effect;

    private SpriteRenderer rend;
    private ParticleSystem particle;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.material = normal;

        particle = effect.GetComponent<ParticleSystem>();
    }

    public void OffLight()
    {
        rend.material = luminous;
        OnEffect();
    }

    public void OnLight()
    {
        rend.material = normal;
        OffEffect();
    }


    public void OnEffect()
    {
        effect.SetActive(true);
        particle.Play();
    }
    public void OffEffect()
    {
        particle.Stop();
        effect.SetActive(false);
    }
}
