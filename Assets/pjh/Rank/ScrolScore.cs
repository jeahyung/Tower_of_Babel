using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrolScore : MonoBehaviour
{
    public Transform contentParent;  // ScrollView�� Content ��ü�� �Ҵ� (TextMeshPro���� �̹� �ڽ����� ����)

    // string ����Ʈ�� �޾Ƽ� �̹� ������ TextMeshPro�� �ؽ�Ʈ�� �Ҵ��ϴ� �Լ�
    public void UpdateScrollView(List<string> nameList)
    {
        // Content ������ �ִ� TextMeshPro�� �迭�� ������
        TextMeshProUGUI[] textComponents = contentParent.GetComponentsInChildren<TextMeshProUGUI>();

        // nameList�� ���� ������ contentParent�� �ִ� TextMeshPro ���� �� ���� ���� ���
        int count = Mathf.Min(nameList.Count, textComponents.Length);

        // nameList�� �����͸� TextMeshPro�� �Ҵ�
        for (int i = 0; i < count; i++)
        {
            textComponents[i].text = nameList[i];  // nameList�� ���Ҹ� �� TextMeshPro�� �Ҵ�
        }
    }
}
