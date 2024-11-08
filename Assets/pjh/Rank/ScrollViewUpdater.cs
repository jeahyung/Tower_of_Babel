using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollViewUpdater : MonoBehaviour
{
    public Transform contentParent;  // ScrollView의 Content 객체를 할당 (TextMeshPro들이 이미 자식으로 존재)

    // string 리스트를 받아서 이미 생성된 TextMeshPro에 텍스트를 할당하는 함수
    public void UpdateScrollView(List<string> nameList, List<string> scoreList)
    {
        bool isScore = false;
        int j = 0;
        int k = 0;
        // Content 하위에 있는 TextMeshPro를 배열로 가져옴
        TextMeshProUGUI[] textComponents = contentParent.GetComponentsInChildren<TextMeshProUGUI>();

        // nameList의 원소 개수와 contentParent에 있는 TextMeshPro 개수 중 작은 값을 사용
        //int count = Mathf.Min(nameList.Count, textComponents.Length);        
        int count = nameList.Count * 2;
        // nameList의 데이터를 TextMeshPro에 할당
        for (int i = 0; i < count; i++)
        {                      
            if(isScore)
            {
                textComponents[i].text = scoreList[j];
                j++;
            }
            else
            {
                textComponents[i].text = ((i / 2) + 1).ToString() + ". " + nameList[k];  // nameList의 원소를 각 TextMeshPro에 할당 
                k++;
            }
            isScore = !isScore;
        }
    }
}
