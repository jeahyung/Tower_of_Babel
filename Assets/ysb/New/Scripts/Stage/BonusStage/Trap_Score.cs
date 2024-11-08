using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Score : MonoBehaviour
{
    [SerializeField] int delScore = 200;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { ScoreManager.instance.DecreaseScore(delScore); }
    }
}
