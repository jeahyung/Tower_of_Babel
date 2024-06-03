using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private IntroDoor door;
    public IntroCam cam;
    public float delayTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        door = GetComponentInParent<IntroDoor>();
    }


    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ Ʈ���ſ� ������ MoveDownY �Լ��� ȣ���մϴ�.
        if (other.CompareTag("Player"))
        {
            TriggerFunctions();
        }
    }

    public void TriggerFunctions()
    {
        StartCoroutine(TriggerFunctionsWithDelay());
    }

    private IEnumerator TriggerFunctionsWithDelay()
    {
        cam.MoveCam(1);
        yield return new WaitForSeconds(delayTime);
        door.OpenTheDoor();
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        door.MoveUpY();
    //    }
    //}


}
