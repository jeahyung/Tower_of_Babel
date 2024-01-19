using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMovePage : MonoBehaviour
{
    [SerializeField]
    private Transform book;

    private void Awake()
    {
        
    }
    public void LoadingEnd_NextPage()
    {
        book.SendMessage("CheckAnimEnd"); 
    }
}
