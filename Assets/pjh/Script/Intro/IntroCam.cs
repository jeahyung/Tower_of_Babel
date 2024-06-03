using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCam : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        BackMainCam();  //0번이 메인 캠
        //mainCam.MoveToTopOfPrioritySubqueue();  //메인 캠, 우선순위 최고로
        maxIndex = cams.Count - 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            camIndex++;
            if (camIndex > maxIndex) { camIndex = 0; }
            MoveCam(camIndex);
        }
    }

    //카메라 이동
    public void MoveCam(int id)
    {
        cams[id].MoveToTopOfPrioritySubqueue();
    }
    public void BackMainCam()
    {
        camIndex = 0;
        cams[0].MoveToTopOfPrioritySubqueue();
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
