using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HideObject : MonoBehaviour
{
    //hide오브젝트의 타입의 객체를 참조해주기 위함
    private HideObject hideObg;
    //숨길 오브젝트 저장
    private static Dictionary<Collider, HideObject> hideMap = new Dictionary<Collider, HideObject>();
    //static객체 생성없이 참조

    [SerializeField]
    public GameObject Renderers;

    public Collider Collider = null;

    void Start()
    {
        InitHideObject();
    }

    public static void InitHideObject()
    {
        //이전 정보가 있으면 오브젝트 표시해주고 초기화
        foreach (var obj in hideMap.Values)
        {
            if (obj != null && obj.Collider != null)
            {
                obj.SetVisible(true);
                obj.hideObg = null;
            }
        }
        //지워주기
        hideMap.Clear();

        //콜라이더 다시 넣어주기
        foreach (var obj in FindObjectsOfType<HideObject>())
        {
            if (obj.Collider != null)
            {
                hideMap[obj.Collider] = obj;
            }
        }
    }

    public static HideObject GetRootHideByCollider(Collider collider)
    {
        HideObject obj;

        if (hideMap.TryGetValue(collider, out obj))
            return GetRoot(obj);
        else
            return null;
    }

    private static HideObject GetRoot(HideObject obj)
    {
        if (obj.hideObg == null)
            return obj;
        else
            return GetRoot(obj.hideObg);
    }

    public void SetVisible(bool visi)
    {
        Renderer rend = Renderers.GetComponent<Renderer>();

        if (rend != null && rend.gameObject.activeInHierarchy && hideMap.ContainsKey(rend.GetComponent<Collider>()))
        {
            rend.shadowCastingMode = visi ? ShadowCastingMode.On : ShadowCastingMode.ShadowsOnly;
        }
    }

}