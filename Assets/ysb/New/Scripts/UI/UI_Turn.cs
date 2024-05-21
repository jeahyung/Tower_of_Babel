using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Turn : MonoBehaviour
{
    //public Transform turnObj = null;

    public Image turnImg;
    public Sprite[] img = new Sprite[2];
    private int imgIndex = 0;


    public TMP_Text turn;
    public TMP_Text stage;

    public float moveSpeed = 1f;

    private void Awake()
    {
        //if(turnObj == null)
        //{
        //    turnObj = GameObject.Find("Turn_Img_Object").transform;
        //}
        stage = transform.GetChild(1).GetComponent<TMP_Text>();
    }
    public void RotateObj(int i)
    {
        turn.text = "";

        //�̹��� ��ü
        imgIndex++;
        if(imgIndex > 1) { imgIndex = 0; }
        turnImg.sprite = img[imgIndex];

        turn.text = i.ToString();
        //StartCoroutine(StartRotate(i));
    }

    //IEnumerator StartRotate(int i)
    //{
    //    Vector3 cur = transform.forward;
    //    Vector3 target = Quaternion.Euler(new Vector3(0, 180, 0)) * transform.forward;
    //    float t = 0;
    //    while (t < 1)
    //    {
    //        turnObj.forward = Vector3.Slerp(cur, target, t);
    //        yield return null;
    //        t += Time.deltaTime * moveSpeed;
    //    }
    //    turnObj.forward = target;
    //    turn.text = i.ToString();
    //}

    public void SetStageInfo(int c, int s)
    {
        stage.text = "";
        stage.text = c.ToString() + " - " + s.ToString();
    }

}