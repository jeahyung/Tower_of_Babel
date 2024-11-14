using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewUpdater : MonoBehaviour
{
    public Transform contentParent;  // ScrollView의 Content 객체를 할당 (TextMeshPro들이 이미 자식으로 존재)
    public ScrollRect scrollRect;
    public TextMeshPro[] textComponents;
    // string 리스트를 받아서 이미 생성된 TextMeshPro에 텍스트를 할당하는 함수
    private void Start()
    {
        textComponents = contentParent.GetComponentsInChildren<TextMeshPro>();
    }
    public void TopViewer()
    {
        scrollRect.verticalNormalizedPosition = 1f;
    }
    public void UpdateScrollView(List<string> nameList, List<string> scoreList)
    {

        TopViewer();
        //bool isScore = false;
        int j = 0;
        // int k = 0;
        // Content 하위에 있는 TextMeshPro를 배열로 가져옴


        // nameList의 원소 개수와 contentParent에 있는 TextMeshPro 개수 중 작은 값을 사용
        //int count = Mathf.Min(nameList.Count, textComponents.Length);        
        int count = Mathf.Min(nameList.Count * 2, textComponents.Length);
        // nameList의 데이터를 TextMeshPro에 할당
        for (int i = 0; i < count-1; i++)
        {                      
            //if(isScore)
            //{
            //    textComponents[i].text = scoreList[j];
            //    j++;
            //}
            //else
            //{
            //    textComponents[i].text = ((i / 2) + 1).ToString() + ". " + nameList[k];  // nameList의 원소를 각 TextMeshPro에 할당 
            //    k++;
            //}
            //isScore = !isScore;

            textComponents[i].text = ((i / 2) + 1).ToString() + ". " + nameList[j];
            textComponents[i+1].text = scoreList[j];
            j++;
            i++;
        }
    }
}
