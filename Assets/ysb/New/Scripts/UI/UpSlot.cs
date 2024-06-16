using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpSlot : MonoBehaviour
{
    public Upgrade slotUp = null;
    public Image img;

    private void Start()
    {
        if (img == null) { transform.Find("Image").GetComponent<Image>(); }
        if(slotUp == null) { gameObject.SetActive(false); }
    }


    public bool AddUpgrade(Upgrade up)
    {
        if (slotUp != null) { return false; }
        slotUp = up;
        img.sprite = Resources.Load<Sprite>("Data/icon/" + up.id.ToString());
        return true;
    }
}
