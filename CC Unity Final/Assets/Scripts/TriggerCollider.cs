using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    [SerializeField] Int32 number;

    public event EventIntParam PositionTriggeredEvent;

    private void OnTriggerEnter(Collider player)
    {
        // send out the trigger event
        if(PositionTriggeredEvent != null) PositionTriggeredEvent(number);
    }    
}
