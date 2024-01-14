using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    private SameWordPuzzleManager manager_Puzzle;

    [SerializeField]
    private WordData word;

    [SerializeField]
    private float moveSpeed;
    private float baseY;
    private float targetY;

    private void Awake()
    {
        manager_Puzzle = GetComponentInParent<SameWordPuzzleManager>();

        baseY = transform.position.y;
        targetY = transform.position.y - 0.2f;
    }

    private IEnumerator MoveDown()
    {
        float ypos = baseY;
        while(ypos > targetY)
        {
            ypos -= Time.deltaTime * moveSpeed;
            transform.position = new Vector3(transform.position.x, ypos, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        manager_Puzzle.TakeWord(word);
    }

    private IEnumerator MoveUp()
    {
        yield return new WaitForSeconds(0.5f);
        float ypos = targetY;
        while (ypos < baseY)
        {
            ypos += Time.deltaTime * moveSpeed;
            transform.position = new Vector3(transform.position.x, ypos, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, baseY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StopCoroutine(MoveUp());
            StartCoroutine(MoveDown());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(MoveDown());
            StartCoroutine(MoveUp());
        }
    }
}
