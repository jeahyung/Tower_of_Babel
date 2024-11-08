using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankUpdate : MonoBehaviour
{
    public TMP_Text textMeshPro;

    [SerializeField]private string[] names;

    void Start()
    {
        
    }

   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            UpdateText(names);
        }        
    }

    public void UpdateName(string[] who)
    {
        name = who[0];
        
        Debug.Log("name show : "+name);        
    }


    public void UpdateText(string[] name)
    {
        Debug.Log("�۾�����");
        // textMeshPro�� �Ҵ�Ǿ����� Ȯ���� �� �ؽ�Ʈ ����
        if (textMeshPro != null && name != null)
        {
            textMeshPro.text = name[0];   
            Debug.Log(name);
        }
        else
        {
            Debug.LogWarning("TextMeshPro component is not assigned.");
        }
        
    }
   

}
