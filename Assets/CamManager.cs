using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    //[SerializeField]
    //private CinemachineVirtualCamera mainCam;
    [SerializeField]
    private List<CinemachineVirtualCamera> cams;
    private int camIndex = 0;
    private int maxIndex;

    ICinemachineCamera currentCam;
    Vector3 camPos;
    public float shakeAmount = 1.0f;
    public float shakeTime = 1.0f;

    [SerializeField]UI_Cam ui_cam = null;

    // Start is called before the first frame update
    void Start()
    {
        if(ui_cam == null) { FindObjectOfType<UI_Cam>(); }
        BackMainCam();  //0���� ���� ķ
        //mainCam.MoveToTopOfPrioritySubqueue();  //���� ķ, �켱���� �ְ��
        maxIndex = cams.Count - 1;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            camIndex++;
            if(camIndex > maxIndex) { camIndex = 0; }
            MoveCam(camIndex);
        }
    }

    //ī�޶� �̵�
    public void MoveCam(int id)
    {
        cams[id].MoveToTopOfPrioritySubqueue();
        ui_cam.ChangeCam();
    }
    public void BackMainCam()
    {
        camIndex = 0;
        cams[0].MoveToTopOfPrioritySubqueue();
        ui_cam.BackMainCam();
    }

    public void CamShake()
    {
        currentCam = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
        camPos = currentCam.VirtualCameraGameObject.transform.position;

        StartCoroutine(Shake(shakeAmount, shakeTime));

    }
    IEnumerator Shake(float ShakeAmount, float ShakeTime)
    {
        float timer = 0;
        while (timer <= ShakeTime)
        {
            currentCam.VirtualCameraGameObject.transform.position = 
                camPos + Random.insideUnitSphere * ShakeAmount;
            timer += Time.deltaTime;
            yield return null;
        }
        currentCam.VirtualCameraGameObject.transform.position = camPos;
    }
}
