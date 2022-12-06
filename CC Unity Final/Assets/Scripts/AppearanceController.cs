using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceController : MonoBehaviour
{
    private GameObject currentModel;

    void Awake()
    {
        // designate the current model
        currentModel = transform.GetChild(0).gameObject;
    }

    // change to another designated model
    public void ChangeLook(string name)
    {
        if(name != currentModel.name)
        {
            currentModel.SetActive(false); // deactivate the current model
            currentModel = transform.Find(name).gameObject; // show the designated model
            currentModel.SetActive(true);
        }  
    }
    // toggle the highlight mode of the current model
    public void ToggleHighlight(bool val)
    {
        if(currentModel.GetComponent<Highlight>())
            currentModel.GetComponent<Highlight>().ToggleHighlight(val);
    }
    public GameObject GetCurrentModel()
    {
        return currentModel;
    }
}
