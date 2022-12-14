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

    //-----------------------Scene Variables-------------------------//
    Dictionary<string, int> _sceneVariables;
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
        _dialogueResource = Resources.Load("Text/Dialogue00") as TextAsset;
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

        // set in-scene variables
        _sceneVariables = new Dictionary<string, int>();

        _sceneVariables.Add("NumOfCollectedClothes", 0);
    }

    void Start()
    {
        SceneInitialize();
    }

    //------------------------Events handlers-------------------------//
    void PointTriggeredHandler(Int32 num)
    {
        // Debug.Log("Point" + num + " triggered");
        triggerPoints[num].GetComponent<InteractableCollider>().SetActive(false);
        PointEventHappen(num);
    }

    void PositionTriggeredHandler(Int32 num)
    {
        // Debug.Log("Position" + num + "triggered");
        triggerPositions[num].SetActive(false);
        PositionEventHappen(num);
    }
    //----------------------------------------------------------------//
    public void SceneInitialize()
    {
        _audioManager["getup"].Play();
        narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[2]);
        instruction.text = "Get the cup";
        // trigger activate
        triggerPoints[2].GetComponent<InteractableCollider>().SetActive(true);
    }
    public void PointEventHappen(Int32 num)
    {
        switch(num)
        {
            // case 0:
            //     narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[0]);
            //     // change the instruction
            //     instruction.text = "Go to the red sphere";
            //     // trigger activate
            //     triggerPoints[2].GetComponent<InteractableCollider>().SetActive(true);
            //     triggerPositions[0].SetActive(true);
            //     break;
            // case 1:
            //     narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[1]);
            //     _dynamicObjects["Object2"].GetComponent<Renderer>().material.color = Color.green;
            //     break;
            // case 2:
            //     // drop down the picked item
            //     _dynamicObjects["Object1"].transform.SetParent(_dynamicObjects["Object1"].GetComponent<PickupAnim>().GetOriginParent());
            //     _dynamicObjects["Object1"].GetComponent<PickupAnim>().DropDown(0);
            //     // change the instruction
            //     instruction.text = "";
            //     // trigger activate
            //     triggerPoints[1].GetComponent<InteractableCollider>().SetActive(true);
            //     break;
            case 0: // check alarm clock
                instruction.text = "";
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[9]);
                // trigger activate
                triggerPoints[1].GetComponent<InteractableCollider>().SetActive(true);
                _audioManager["alarm"].Stop();
                break;
            case 1: // put down alarm clock
                // trigger activate
                triggerPoints[15].GetComponent<InteractableCollider>().SetActive(true);
                // drop down the alarm clock
                GameObject clock = _dynamicObjects["AlarmClock"];
                clock.transform.SetParent(clock.GetComponent<PickupAnim>().GetOriginParent());
                clock.GetComponent<PickupAnim>().DropDown(0);
                break;
            case 2: // pick up the cup
                instruction.text = "Catch some water";
                // trigger activate
                triggerPoints[3].GetComponent<InteractableCollider>().SetActive(true);
                triggerPoints[16].GetComponent<InteractableCollider>().SetActive(true);
                break;
            case 3: // catch some water
                _audioManager["waterfill"].Play();

                instruction.text = "Go to the kitchen to make coffee";
                // change appearance of the cup
                _dynamicObjects["Cup"].GetComponent<AppearanceController>().ChangeLook("Cup_water");
                // trigger activate
                triggerPoints[16].GetComponent<InteractableCollider>().SetActive(false);
                triggerPoints[4].GetComponent<InteractableCollider>().SetActive(true);
                triggerPositions[0].SetActive(true);
                break;
            case 4: // make coffee
                // play SFXs
                _audioManager["coffeemachine_launch"].Play();
                _audioManager["coffeemachine_run"].Play();
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[4]);
                // change the instruction
                instruction.text = "Pick up clothes on the ground in the bedroom";
                // drop down the cup
                GameObject cup = _dynamicObjects["Cup"];
                cup.transform.SetParent(cup.GetComponent<PickupAnim>().GetOriginParent());
                cup.GetComponent<PickupAnim>().DropDown(1);
                // trigger activate
                for(int i = 5; i <= 9; i++)
                {
                    triggerPoints[i].GetComponent<InteractableCollider>().SetActive(true);
                }
                break;
            case 5: // Pick up clothe A
            case 6: // Pick up clothe B
            case 7: // Pick up clothe C
            case 8: // Pick up clothe D
            case 9: // Pick up clothe E
                _audioManager["pickupclothe"].Play();
                // add 1 to collected clothes
                _sceneVariables["NumOfCollectedClothes"] ++;
                // check if all the clothes have been collected
                if(_sceneVariables["NumOfCollectedClothes"] >= 5)
                {
                    instruction.text = "Put the clothes into the washing machine";
                    // trigger activate
                    triggerPoints[10].GetComponent<InteractableCollider>().SetActive(true);
                }
                break;
            case 10: // start washing
                _audioManager["washmachine_launch"].Play();
                _audioManager["washmachine"].Play();

                instruction.text = "";
                // trigger activate
                triggerPoints[11].GetComponent<InteractableCollider>().SetActive(true);
                triggerPositions[1].SetActive(true);
                break;
            case 11: // pick up watering can
                // trigger activate
                triggerPoints[12].GetComponent<InteractableCollider>().SetActive(true);
                break;
            case 12: // water the flower
                _audioManager["watering"].Play();

                instruction.text = "";
                // trigger activate
                triggerPoints[13].GetComponent<InteractableCollider>().SetActive(true);
                break;
            case 13: // put down watering can
                _audioManager["coffeemachine_run"].Stop();
                _audioManager["coffeemachine_finish"].Play();

                instruction.text = "Drink coffee";
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[7]);
                // drop down the watering can
                GameObject pot = _dynamicObjects["WateringCan"];
                pot.transform.SetParent(pot.GetComponent<PickupAnim>().GetOriginParent());
                pot.GetComponent<PickupAnim>().DropDown(0);
                // change appearance of the cup
                _dynamicObjects["Cup"].GetComponent<AppearanceController>().ChangeLook("Cup_coffee");
                // trigger activate
                triggerPoints[14].GetComponent<InteractableCollider>().SetActive(true);
                break;
            case 14: // drink coffee
                _audioManager["coffeemachine_finish"].Stop();
                _audioManager["sip"].Play();

                instruction.text = "check the alarm clock";
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[8]);
                // delete the cup
                Destroy(_dynamicObjects["Cup"]);
                // trigger activate
                triggerPoints[0].GetComponent<InteractableCollider>().SetActive(true);
                _audioManager["alarm"].Play(); // play alarm
                break;
            case 15: // take the medicine
                _audioManager["takemedicine"].Play();

                break;
            case 16: // try to catch water in the kitchen
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[12]);
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[13]);
                break;
        }
    }

    void PlayAudio(string name)
    {

    }
    public void PositionEventHappen(Int32 num)
    {
        switch(num)
        {
            case 0: // leave bathroom
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[3]);
                break;
            case 1: // leave bathroom 2nd
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[5]);
                // trigger activate
                triggerPositions[2].SetActive(true);
                break;
            case 2: // leave bedroom
                instruction.text = "Water the plant in the living room";
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[6]);
                // trigger activate
                triggerPoints[11].GetComponent<InteractableCollider>().SetActive(true);
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
