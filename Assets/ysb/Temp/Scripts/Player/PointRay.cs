using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRay : MonoBehaviour
{
    public bool canInput = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canInput == false) { return; }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                PuzzleManager manager_Puzzle = hit.collider.GetComponent<PuzzleManager>();
                if(manager_Puzzle != null)
                {
                    manager_Puzzle.StartPuzzleSolving();
                    canInput = false;   //ÆÛÁñÀ» Çª´Â µ¿¾È¿£ ¸·±â
                }
            }
        }
    }
}
