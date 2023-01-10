using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSetup : MonoBehaviour
{
    [SerializeField] GameObject Player;
    GameObject CameraRoot;
    GameObject MainCamera;
    [SerializeField] Image BlackMask;
    [SerializeField] FloatingText BottomLine;
    [SerializeField] GameObject PostFX;

    // public event EventNoParam EnterSceneEvent;
    // public event EventIntParam SwitchSceneEvent;

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
        CameraRoot = Player.transform.Find("PlayerCameraRoot").gameObject;
        MainCamera = CameraRoot.transform.Find("Player Camera").gameObject;

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
        StartCoroutine(EnterScene());
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

    //----------------------------------------------------------------//}
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
                instruction.text = "Take medicine";
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
                _audioManager["knock"].Play();

                instruction.text = "Check the front door";
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[10]);
                // trigger activate
                triggerPoints[17].GetComponent<InteractableCollider>().SetActive(true);
                break;
            case 16: // try to catch water in the kitchen
                _audioManager["faucetsqueak"].Play();
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[12]);
                narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[13]);
                break;
            case 17: // look out of the front door
                _audioManager["knock"].Stop();
                _audioManager["getup"].Play();
                instruction.text = "";

                SetPlayerController(false); // enable player control
                // start coroutine
                StartCoroutine(EndScene());
                break;
        }
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
    // Scene Beginning animations
    IEnumerator EnterScene()
    {
        BlackMask.enabled = true;
        yield return new WaitForSeconds(2.0f);

        narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[0]);
        yield return new WaitForSeconds(1.0f);
        BlackMask.enabled = false;
        // animate - open eyes
        CameraRoot.GetComponent<Animator>().Play("Open eyes");
        while(CameraRoot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1) yield return 0;
        // wait for animator normalized time return to zero
        _audioManager["getup"].Play();
        while(CameraRoot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return 0;
        // wait for animation to finish

        narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[1]);
        narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[2]);
        _audioManager["clock"].Play();
        yield return new WaitForSeconds(1.5f);
        // start playing
        instruction.text = "Get the cup";
        triggerPoints[2].GetComponent<InteractableCollider>().SetActive(true); // trigger activate
        SetPlayerController(true); // enable player control
        // disable postFXs
        PostFX.GetComponent<PostFX_Manager>().EnableEffects(false);

        yield break;
    }

    // Scene Ending animations
    IEnumerator EndScene()
    {
        BlackMask.enabled = true; // enable the black mask
        // fade in
        BlackMask.color = new Color(0.001f, 0, 0, 0);
        while(BlackMask.color.a < 1 && BlackMask.color.r == 0.001f)
        {
            BlackMask.color = new Color(0.001f, 0, 0, BlackMask.color.a + Time.deltaTime);
            yield return 0;
        }
        BlackMask.color = new Color(0, 0, 0, 1);
        _audioManager["knock_loud"].Play();
        // enable postFXs
        PostFX.GetComponent<PostFX_Manager>().EnableEffects(true);
        // move camera outdoor
        MainCamera.transform.position = new Vector3(9.418f, 2.98f, -17.12f);
        MainCamera.transform.eulerAngles = new Vector3(0, 90, 0);
        // fade out
        while(BlackMask.color.a > 0 && BlackMask.color.r == 0)
        {
            BlackMask.color = new Color(0, 0, 0, BlackMask.color.a - Time.deltaTime);
            yield return 0;
        }
        BlackMask.enabled = false;
        narrator.GetComponent<TitleController>().AddDialogue(dialogueTexts[11]);

        // wait for 4 seconds
        yield return new WaitForSeconds(4.0f);

        BlackMask.enabled = true; // enable the black mask
        // fade in
        BlackMask.color = new Color(0.001f, 0, 0, 0);
        while(BlackMask.color.a < 1 && BlackMask.color.r == 0.001f)
        {
            BlackMask.color = new Color(0.001f, 0, 0, BlackMask.color.a + Time.deltaTime);
            yield return 0;
        }
        BlackMask.color = new Color(0, 0, 0, 1);
        _audioManager["knock_loud"].Stop();
        // disable postFXs
        PostFX.GetComponent<PostFX_Manager>().EnableEffects(false);
        // reset the camera
        MainCamera.transform.localPosition = Vector3.zero;
        MainCamera.transform.localEulerAngles = Vector3.zero;
        // fade out
        while(BlackMask.color.a > 0 && BlackMask.color.r == 0)
        {
            BlackMask.color = new Color(0, 0, 0, BlackMask.color.a - Time.deltaTime);
            yield return 0;
        }

        // move the man
        GameObject Man = GameObject.Find("Man");
        Man.transform.position = new Vector3(1.96f, 0.51f, -17.07f);
        Man.transform.eulerAngles = new Vector3(0, -90, 0);
        // animate - turn back
        CameraRoot.GetComponent<Animator>().Play("Turn back");
        while(CameraRoot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1) yield return 0;
        // wait for animator normalized time return to zero
        while(CameraRoot.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1) yield return 0;
        _audioManager["jumpscare"].Play();
        yield return new WaitForSeconds(0.5f);

        // black out
        BlackMask.color = new Color(0, 0, 0, 1);
        Man.SetActive(false);
        yield return new WaitForSeconds(2.2f);

        BlackMask.enabled = false;

        _audioManager["creepy"].Play();
        CameraRoot.GetComponent<CameraAnimation>().OnPassOut();
        // animate - close eyes
        CameraRoot.GetComponent<Animator>().Play("Close eyes");
        // CameraRoot.transform.localPosition = new Vector3(0.48f, -0.66f, 0f);
        // CameraRoot.transform.localEulerAngles = new Vector3(-20, 180, -48);
        while( _audioManager["creepy"].isPlaying) yield return 0;
        yield return new WaitForSeconds(2.2f);

        BottomLine.CreateFloatingText("To Be Continued ...");
        while(BottomLine.GetState() == "Dormant") yield return 0;
        while(BottomLine.GetState() != "Dormant") yield return 0;

        // scene end here
        Debug.Log("Scene end");
        SceneManager.LoadScene("Menu"); // back to menu scene
        yield break;
    }
}
