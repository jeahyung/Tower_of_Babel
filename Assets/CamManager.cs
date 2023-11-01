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
}
