using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrolScore : MonoBehaviour
{
    public Transform contentParent;  // ScrollView의 Content 객체를 할당 (TextMeshPro들이 이미 자식으로 존재)

    // string 리스트를 받아서 이미 생성된 TextMeshPro에 텍스트를 할당하는 함수
    public void UpdateScrollView(List<string> nameList)
    {
        // Content 하위에 있는 TextMeshPro를 배열로 가져옴
        TextMeshProUGUI[] textComponents = contentParent.GetComponentsInChildren<TextMeshProUGUI>();

        // nameList의 원소 개수와 contentParent에 있는 TextMeshPro 개수 중 작은 값을 사용
        int count = Mathf.Min(nameList.Count, textComponents.Length);

        // nameList의 데이터를 TextMeshPro에 할당
        for (int i = 0; i < count; i++)
        {
            textComponents[i].text = nameList[i];  // nameList의 원소를 각 TextMeshPro에 할당
        }
    }
}
