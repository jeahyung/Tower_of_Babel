using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMove : MonoBehaviour
{
    private CamMovement cam;

    private void Awake()
    {
        cam = FindObjectOfType<CamMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        cam.SetMove(false);
    }
}
