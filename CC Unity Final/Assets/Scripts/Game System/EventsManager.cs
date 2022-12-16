using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

// declare delegate types
public delegate void EventNoParam();
public delegate void EventBoolParam(bool result);
public delegate void EventIntParam(int num);
public class EventsManager : MonoBehaviour
{
    // all the trigger points under this parent
    [SerializeField] Transform triggerPointsParent;
    // 以<List>存储trigger points
    private List<Transform> triggerPoints;

    // all the trigger positions under this parent
    [SerializeField] Transform triggerPositionsParent;
    // 以<List>存储trigger positions
    private List<Transform> triggerPositions;

    void Awake()
    {
        if(triggerPointsParent.childCount > 0)
        {
            triggerPoints = new List<Transform>();
            // get all the trigger points
            foreach(Transform triggerPt in triggerPointsParent)
            {
                triggerPoints.Add(triggerPt);
                // listen to each trigger event
                triggerPt.GetComponent<InteractableCollider>().PointTriggeredEvent += PointTriggeredHandler;
            }
        }

        if(triggerPositionsParent.childCount > 0)
        {
            triggerPositions = new List<Transform>();
            // get all the trigger positions
            foreach(Transform triggerPos in triggerPositionsParent)
            {
                triggerPositions.Add(triggerPos);
                // listen to each trigger event
                triggerPos.GetComponent<TriggerCollider>().PositionTriggeredEvent += PositionTriggeredHandler;
            }
        }
    }

    void PointTriggeredHandler(Int32 num)
    {
        Debug.Log("Point" + num + " triggered");
        triggerPoints[num].GetComponent<InteractableCollider>().SetActive(false);
        this.GetComponent<SceneSetup>().PointEventHappen(num);
    }

    void PositionTriggeredHandler(Int32 num)
    {
        Debug.Log("Position" + num + "triggered");
        triggerPositions[num].gameObject.SetActive(false);
    }
}
