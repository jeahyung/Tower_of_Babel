using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollViewUpdater : MonoBehaviour
{
    public Transform contentParent;  // ScrollView�� Content ��ü�� �Ҵ� (TextMeshPro���� �̹� �ڽ����� ����)

    // string ����Ʈ�� �޾Ƽ� �̹� ������ TextMeshPro�� �ؽ�Ʈ�� �Ҵ��ϴ� �Լ�
    public void UpdateScrollView(List<string> nameList, List<string> scoreList)
    {
        bool isScore = false;
        int j = 0;
        int k = 0;
        // Content ������ �ִ� TextMeshPro�� �迭�� ������
        TextMeshProUGUI[] textComponents = contentParent.GetComponentsInChildren<TextMeshProUGUI>();

        // nameList�� ���� ������ contentParent�� �ִ� TextMeshPro ���� �� ���� ���� ���
        //int count = Mathf.Min(nameList.Count, textComponents.Length);        
        int count = nameList.Count * 2;
        // nameList�� �����͸� TextMeshPro�� �Ҵ�
        for (int i = 0; i < count; i++)
        {                      
            if(isScore)
            {
                textComponents[i].text = scoreList[j];
                j++;
            }
            else
            {
                textComponents[i].text = ((i / 2) + 1).ToString() + ". " + nameList[k];  // nameList�� ���Ҹ� �� TextMeshPro�� �Ҵ� 
                k++;
            }
            isScore = !isScore;
        }
    }
}
