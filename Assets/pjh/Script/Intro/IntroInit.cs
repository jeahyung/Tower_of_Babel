using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroInit : MonoBehaviour
{
    public ParticleSystem effect;
    // Start is called before the first frame update
    void Awake()
    {
        EffectManage.Instance.RegisterEffect("Door_Open_intro", effect);
    }
}
