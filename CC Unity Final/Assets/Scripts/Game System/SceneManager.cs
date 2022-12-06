using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] GameObject Player;

    public event EventNoParam EnterSceneEvent;
    public event EventIntParam SwitchSceneEvent;

    // Dynamic objects
    Dictionary<string, GameObject> _dynamicObjects;

    // Audio assets
    Dictionary<string, AudioSource> _audioManager;

    // Dialogue text assets
    [SerializeField] GameObject narrator;
    TextAsset _dialogueResource;
    String[] dialogueTexts;

    // instruction
    [SerializeField] Text instruction;

    //----------------------Experient Function------------------------//
    // all the trigger points under this parent
    [SerializeField] Transform triggerPointsParent;
    // 以<List>存储trigger points
    private List<GameObject> triggerPoints;

    // all the trigger positions under this parent
    [SerializeField] Transform triggerPositionsParent;
    // 以<List>存储trigger positions
    private List<GameObject> triggerPositions;
    //----------------------------------------------------------------//
    void Awake()
    {
        // load all the dynamic objects
        GameObject[] dynamicObjs = GameObject.FindGameObjectsWithTag("Dynamic");
        if(dynamicObjs.Length > 0)
        {
            _dynamicObjects = new Dictionary<string, GameObject>();
            foreach(GameObject dynamicObj in dynamicObjs)
            {
                _dynamicObjects.Add(dynamicObj.name, dynamicObj);
            }
        }

        // load audio resources
        GameObject[] audioObjs = GameObject.FindGameObjectsWithTag("Audio");
        if(audioObjs.Length > 0)
        {
            _audioManager = new Dictionary<string, AudioSource>();
            // load all the audio assets
            foreach(GameObject audioObj in audioObjs)
            {
                _audioManager.Add(audioObj.name, audioObj.GetComponent<AudioSource>());
            }
        }

        // load dialogue resources
        _dialogueResource = Resources.Load("Dialogue01") as TextAsset;
        // split the dialogue texts and save single sentences as an array
        dialogueTexts = _dialogueResource.text.Split('\n');


        // get all the trigger points
        if(triggerPointsParent.childCount > 0)
        {
            triggerPoints = new List<GameObject>();
            foreach(Transform triggerPt in triggerPointsParent)
            {
                triggerPoints.Add(triggerPt.gameObject);
                // listen to each trigger event
                triggerPt.GetComponent<InteractableCollider>().PointTriggeredEvent += PointTriggeredHandler;
            }
        }
        // get all the trigger positions
        if(triggerPositionsParent.childCount > 0)
        {
            triggerPositions = new List<GameObject>();
            foreach(Transform triggerPos in triggerPositionsParent)
            {
                triggerPositions.Add(triggerPos.gameObject);
                // listen to each trigger event
                triggerPos.GetComponent<TriggerCollider>().PositionTriggeredEvent += PositionTriggeredHandler;
                // deactive the trigger position initially
                triggerPos.gameObject.SetActive(false);
            }
        }
    }

    //------------------------Events handlers-------------------------//
    void PointTriggeredHandler(Int32 num)
    {
        Debug.Log("Point" + num + " triggered");
        triggerPoints[num].GetComponent<InteractableCollider>().SetActive(false);
        PointEventHappen(num);
    }

    void PositionTriggeredHandler(Int32 num)
    {
        Debug.Log("Position" + num + "triggered");
        triggerPositions[num].SetActive(false);
        PositionEventHappen(num);
    }
    //----------------------------------------------------------------//
    public void PointEventHappen(Int32 num)
    {
        switch(num)
        {
            case 0:
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[0]);
                // change the instruction
                instruction.text = "Go to the red sphere";
                // trigger activate
                triggerPoints[2].GetComponent<InteractableCollider>().SetActive(true);
                triggerPositions[0].SetActive(true);
                break;
            case 1:
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[1]);
                _dynamicObjects["Object2"].GetComponent<Renderer>().material.color = Color.green;
                break;
            case 2:
                // drop down the picked item
                _dynamicObjects["Object1"].transform.SetParent(_dynamicObjects["Object1"].GetComponent<PickupAnim>().GetOriginParent());
                _dynamicObjects["Object1"].GetComponent<PickupAnim>().DropDown(0);
                // change the instruction
                instruction.text = "";
                // trigger activate
                triggerPoints[1].GetComponent<InteractableCollider>().SetActive(true);
                break;
        }
    }
    public void PositionEventHappen(Int32 num)
    {
        switch(num)
        {
            case 0:
                _dynamicObjects["Object1"].GetComponent<AppearanceController>().ChangeLook("Sphere");
                // change the instruction
                instruction.text = "Drop the cube at the transparent sphere";
                break;
            case 1:
                _dynamicObjects["Object1"].GetComponent<AppearanceController>().ChangeLook("Cube");
                break;
        }
    }

    // play designated SFX
    void PlaySFX(String key)
    {

    }

    // enable/disable the player control
    public void SetPlayerController(bool val)
    {
        if(val)
        {
            Player.GetComponent<Movement>().enabled = true;
            Player.GetComponent<MouseLook>().enabled = true;
        }
        else
        {
            Player.GetComponent<Movement>().enabled = false;
            Player.GetComponent<MouseLook>().enabled = false;
        }
    }
}
