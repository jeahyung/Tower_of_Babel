using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mural : InteractionObject
{
    [SerializeField]
    private ChangeMaterial[] answers;
    private ChangeMaterial[] words;

    private bool isObserve = false;

    public UnityEvent onObserve;
    public UnityEvent endObserve;

    private void Start()
    {
        words = GetComponentsInChildren<ChangeMaterial>();
    }

    public override void InteractObject()
    {
        isObserve = !isObserve;
        if(isObserve == true)
        {
            onObserve.Invoke();
            OffEffect();
        }
        else
        {
            endObserve.Invoke();
            OnEffect();
        }
    }

    public void OffLight()
    {
        foreach(var a in answers)
        {
            a.OffLight();
        }
    }
    public void OnLight()
    {
        foreach (var a in answers)
        {
            a.OnLight();
        }
    }

    public void OnEffect()
    {
        foreach (var a in answers)
        {
            a.OnEffect();
        }
    }
    public void OffEffect()
    {
        foreach (var a in answers)
        {
            a.OffEffect();
        }
    }
}
