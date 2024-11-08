using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public ParticleSystem[] effect;
   // public ParticleSystem landEffect;

    private void Awake()
    {
        EffectManage.Instance.RegisterEffect("Player_Step", effect[0]);
        EffectManage.Instance.RegisterEffect("Player_BigStep", effect[1]);
        EffectManage.Instance.RegisterEffect("Player_Hit", effect[2]);
        EffectManage.Instance.RegisterEffect("Player_Skill", effect[3]);
        EffectManage.Instance.RegisterEffect("Player_Itemget", effect[4]);
        EffectManage.Instance.RegisterEffect("Item_Use", effect[5]);
        EffectManage.Instance.RegisterEffect("Diamond_Create", effect[6]);
        EffectManage.Instance.RegisterEffect("Diamond_Destroy", effect[7]);
        EffectManage.Instance.RegisterEffect("Key_Effect", effect[8]);
        EffectManage.Instance.RegisterEffect("Rope_Effect", effect[9]);
        EffectManage.Instance.RegisterEffect("Player_Teleport", effect[10]);
        EffectManage.Instance.RegisterEffect("Door_Open", effect[11]);
        EffectManage.Instance.RegisterEffect("Item_Select", effect[12]);
        EffectManage.Instance.RegisterEffect("Select_Upgrade", effect[13]);
        EffectManage.Instance.RegisterEffect("UI_Popup", effect[14]);
        EffectManage.Instance.RegisterEffect("UI_Use_Skill", effect[15]);
        EffectManage.Instance.RegisterEffect("Monster_Move", effect[16]);
        EffectManage.Instance.RegisterEffect("Rope_Effect", effect[17]);

    }
}