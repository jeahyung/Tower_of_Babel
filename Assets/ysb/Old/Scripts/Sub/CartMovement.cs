using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovement : InteractionObject
{
    private bool isMoving = false;

    [SerializeField]
    private Transform[] points = new Transform[2];
    private int pointCount = 0;
    private Transform target;

    private float smoothTime = 0.3f;

    private void Start()
    {
        transform.position = points[0].position;
    }

    public override void ShowMessage(bool b)
    {
        if(isMoving == true) { return; }
        base.ShowMessage(b);
    }
    public override void InteractObject()
    {
        if(isMoving == true) { return; }
        manager_UI.ShowInteractMessage(false);

        //카트가 움직임
        pointCount = pointCount + 1 > 1 ? 0 : 1;
        target = points[pointCount];
        StartCoroutine(MoveCart());
    }

    private IEnumerator MoveCart()
    {
        isMoving = true;
        while(Vector3.Distance(transform.position, target.position) >= 0.1f)
        {
            Vector3 direction = target.position - transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref direction, smoothTime);
            yield return null;
        }
        isMoving = false;
    }
}
