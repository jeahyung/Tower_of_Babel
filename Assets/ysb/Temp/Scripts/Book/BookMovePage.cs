using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookMovePage : MonoBehaviour
{
    private Transform book;

    private void Awake()
    {
        book = transform.parent;
    }
    public void LoadingEnd_NextPage()
    {
        book.SendMessage("CheckAnimEnd");
    }
}
