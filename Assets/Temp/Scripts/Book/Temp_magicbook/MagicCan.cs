using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MagicCan : MonoBehaviour
{
    public int cIndex;
    public MagicCanMgr mg;
    private Image canImg;

    public WordData word;
    private void Awake()
    {
        canImg = GetComponent<Image>();
        mg = GetComponentInParent<MagicCanMgr>();
    }

    //이미지 교체
    public void ChangeWord(WordData wd)
    {
        if(wd == null) { return; }
        word = null;
        canImg.sprite = null;

        word = wd;
        canImg.sprite = wd.wordImg;

        mg.AddWord(cIndex, wd);
    }
}
