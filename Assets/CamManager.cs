using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera mainCam;
    [SerializeField]
    private List<CinemachineVirtualCamera> subCams;

    ICinemachineCamera currentCam;
    Vector3 camPos;
    public float shakeAmount = 1.0f;
    public float shakeTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainCam.MoveToTopOfPrioritySubqueue();  //메인 캠, 우선순위 최고로
    }

    //카메라 이동
    public void MoveCam(int id)
    {
        subCams[id].MoveToTopOfPrioritySubqueue();
    }
    public void BackMainCam()
    {
        mainCam.MoveToTopOfPrioritySubqueue();
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
