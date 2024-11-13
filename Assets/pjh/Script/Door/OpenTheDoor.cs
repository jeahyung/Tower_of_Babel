using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class OpenTheDoor : MonoBehaviour
{
    public GameObject[] door;
    private bool open;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾ Ʈ���ſ� ������ MoveDownY �Լ��� ȣ���մϴ�.
        if (other.CompareTag("Player"))
        {
            TriggerFunctions();
        }
    }

    private void TriggerFunctions()
    {
        if (open)
        {
            Vector3 rightDoor = new Vector3(door[0].transform.localEulerAngles.x, door[0].transform.localEulerAngles.y, -90);

            door[0].transform.DOLocalRotate(rightDoor, 1f).SetEase(Ease.Linear);
            Vector3 pos1 = new(door[0].transform.position.x, door[0].transform.position.y + 4f, door[0].transform.position.z);
            // EffectManage.Instance.PlayEffect("Door_Open_intro", pos1);

            Vector3 leftDoor = new Vector3(door[1].transform.localEulerAngles.x, door[1].transform.localEulerAngles.y, 90);

            door[1].transform.DOLocalRotate(leftDoor, 1f).SetEase(Ease.Linear);
            Vector3 pos2 = new(door[1].transform.position.x, door[1].transform.position.y + 4f, door[1].transform.position.z);
            // EffectManage.Instance.PlayEffect("Door_Open_intro", pos2);

            GetComponent<AudioSource>().Play();

            open = false;
        }
    }
}
