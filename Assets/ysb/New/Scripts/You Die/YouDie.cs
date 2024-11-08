using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouDie : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        StageManager.instance.GameOver();
    }
}
