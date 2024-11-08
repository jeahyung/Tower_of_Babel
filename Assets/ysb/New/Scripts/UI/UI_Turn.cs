using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Turn : MonoBehaviour
{
    public Transform turnObj = null;

    public Image turnImg;
    public Sprite[] img = new Sprite[2];
    private int imgIndex = 0;


    public TMP_Text turn;
    public TMP_Text stage;

    public float moveSpeed = 1f;

    public GameObject[] TurnShowImg = new GameObject[2];

    int r = 1;
    private void Awake()
    {
        if (turnObj == null)
        {
            turnObj = GameObject.Find("Turn_Img_Object").transform;
        }
        stage = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void ResetObj()
    {
        turnObj.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        turn.text = "1";
    }
    public void RotateObj(int i)
    {
        turn.text = "";

        r = r + 1 > 1 ? 0 : 1;

        //이미지 교체
        imgIndex++;
        //if(imgIndex > 1) { imgIndex = 0; }
        //turnImg.sprite = img[imgIndex];

        //turn.text = i.ToString();
        StartCoroutine(StartRotate(i));
    }

    IEnumerator StartRotate(int i)
    {
        //Vector3 cur = transform.forward;
        //Vector3 target = Quaternion.Euler(new Vector3(0, 180, 0)) * transform.forward;
        //Debug.Log(cur + "/" + target);
        Quaternion cur;
        Quaternion target;

        if (r == 0)
        {
            cur = Quaternion.Euler(new Vector3(0, 0, 0));
            target = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            cur = Quaternion.Euler(new Vector3(0, 180, 0));
            target = Quaternion.Euler(new Vector3(0, 360, 0));
        }
        
        float t = 0;
        while (t < 1)
        {
            turnObj.rotation = Quaternion.Slerp(cur, target, t);
            //turnObj.forward = Vector3.Slerp(cur, target, t);
            yield return null;
            t += Time.deltaTime * moveSpeed;
        }
        turnObj.rotation = target;
        turn.text = i.ToString();
    }

    public void SetStageInfo(int c, int s)
    {
        stage.text = "";
        stage.text = c.ToString() + " - " + s.ToString();
    }

    public void ResetUI()
    {
        imgIndex = 0;
        turnImg.sprite = img[imgIndex];
        turn.text = "0";
    }

    public void ShowImg(int i)
    {
        if (TurnShowImg[i].activeSelf) { return; }

        if (StageManager.instance.isBonusStage) { i += 2; }
        
        TurnShowImg[i].SetActive(true);
        Debug.Log("ghcnf");
    }
    public void HideImg(int i)
    {
        TurnShowImg[i].SetActive(false);
    }
}
