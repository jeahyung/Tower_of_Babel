using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTmanager : MonoBehaviour
{
    [SerializeField]
    protected UIManager manager_UI;
    private bool canF = false;
    [SerializeField]
  
    public WordData wordData;

    // Start is called before the first frame update
    void Start()
    {
        manager_UI = FindObjectOfType<UIManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            manager_UI.ShowInteractMessage(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
       
        manager_UI.ShowInteractMessage(false);
       
    }
    // Update is called once per frame
    void Update()
    {
        if(canF == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {

            }
        }      
    }
}
