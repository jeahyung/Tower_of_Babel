using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HideObject : MonoBehaviour
{
    //hide������Ʈ�� Ÿ���� ��ü�� �������ֱ� ����
    private HideObject hideObg;
    //���� ������Ʈ ����
    private static Dictionary<Collider, HideObject> hideMap = new Dictionary<Collider, HideObject>();
    //static��ü �������� ����

    [SerializeField]
    public GameObject Renderers;

    public Collider Collider = null;

    void Start()
    {
        InitHideObject();
    }

    public static void InitHideObject()
    {
        //���� ������ ������ ������Ʈ ǥ�����ְ� �ʱ�ȭ
        foreach (var obj in hideMap.Values)
        {
            if (obj != null && obj.Collider != null)
            {
                obj.SetVisible(true);
                obj.hideObg = null;
            }
        }
        //�����ֱ�
        hideMap.Clear();

        //�ݶ��̴� �ٽ� �־��ֱ�
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