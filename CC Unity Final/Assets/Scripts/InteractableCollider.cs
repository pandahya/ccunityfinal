using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCollider : MonoBehaviour
{
    // enum ColliderType {mobile, immobile};
    [SerializeField] bool pickable;
    [SerializeField] Int32 number;

    [SerializeField] GameObject bindingObject; // the object bound to this checkbox is selected
    [SerializeField] String Tip;

    public event EventIntParam PointTriggeredEvent;
    
    // enable/disable the checkbox
    public void SetActive(bool val)
    {
        if(val)
        {
            int LayerPickable = LayerMask.NameToLayer("Pickable");
            gameObject.layer = LayerPickable;
        }
        else
        {
            gameObject.layer = 0; // when deactivated, set checkbox to default layer
        }
    }

    public bool Trigger()
    {
        // send out the trigger event
        if(PointTriggeredEvent != null) PointTriggeredEvent(number);
        // return if the bindingObject is pickable
        return pickable;
    }

    // toggle the highlight mode of the binding object
    public void ToggleHighlight(bool val)
    {
        if(bindingObject != null)
        {
            if(bindingObject?.GetComponent<Highlight>())
                bindingObject.GetComponent<Highlight>().ToggleHighlight(val);
            else if(bindingObject?.GetComponent<AppearanceController>())
                bindingObject.GetComponent<AppearanceController>().ToggleHighlight(val);
        }      
    }

    // check if pickable
    public bool IsPickable()
    {
        return pickable;
    }
    // get the bindingObejct
    public GameObject GetBindingObject()
    {
        return bindingObject ?? null;
    }
    // get tip text
    public String GetTip()
    {
        return Tip;
    }
}
