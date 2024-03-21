using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private RandomPattern randomPattern;

    [SerializeField] private int num = 0;
    [SerializeField] private GameObject[] map;
    [SerializeField] private GameObject restPos;
    private int cnt = 0;

    // Start is called before the first frame update
    void Awake()
    {
        randomPattern = GetComponent<RandomPattern>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MapChange();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            RemoveMap();
        }
    }


    void MapChange()
    {
   
        cnt++;
        if(cnt == 5)
        {
            randomPattern.ClearArrPattern();
            cnt = 0;
            num = randomPattern.Number();
        }
        else
        {
            num = randomPattern.Number();
        }
        
        map[num].transform.position = map[0].transform.position;
    }

    void RemoveMap()
    {
        map[num].transform.position = restPos.transform.position;
    }
}
