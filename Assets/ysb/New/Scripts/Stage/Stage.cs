using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private bool isFind = false;
    [SerializeField] List<GameObject> objs = new List<GameObject>(); //맵 오브젝트들

    private void Start()
    {
        Transform item = transform.GetChild(1);
        for (int i = 0; i < item.childCount; ++i)
        {
            objs.Add(item.GetChild(i).gameObject);
        }

        Transform mob = transform.GetChild(0);
        for(int i = 0; i < mob.childCount; ++i)
        {
            objs.Add(mob.GetChild(i).gameObject);
        }

        isFind = true;
    }

    private void OnEnable()
    {
        if(isFind == false) { return; }
        for(int i = 0; i < objs.Count; ++i)
        {
            objs[i].SetActive(true);
        }
    }
}
