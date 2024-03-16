using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    protected GameObject player;
    protected UIManager manager_UI;

    private void Awake()
    {
        manager_UI = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (player == null) { return; }
        ShowMessage(true);
        if (Input.GetKeyDown(KeyCode.F))
        {
            InteractObject();
        }
    }

    public virtual void ShowMessage(bool b)
    {
        manager_UI.ShowInteractMessage(b);
    }
    public virtual void InteractObject()
    {
        Debug.Log(gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            player = other.gameObject;
            ShowMessage(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        manager_UI.ShowInteractMessage(false);
        player = null;
    }
}
