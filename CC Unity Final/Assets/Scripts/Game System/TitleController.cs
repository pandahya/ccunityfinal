using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [SerializeField] float FadingSpeed = 0.02f;
    [SerializeField] float StayDuration = 0.06f; // stay duration for each char(sec)

    private Text dialogContent; // dialogue text component
    private List<String> dialogQueue; // pending strings to be displayed
    private String currentDialog = ""; // the dialogue being displayed now
    private String state = "stop"; // stop; fadeIn; stay; fadeOut
    private float counter = 0;
    void Awake()
    {
        dialogContent = this.GetComponent<Text>();
        // initialize the queue
        dialogQueue = new List<String>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case "fadeIn":
                counter += FadingSpeed;
                dialogContent.color = new Color(255, 255, 255, counter);
                if(counter >= 1) {
                    counter = 1 + currentDialog.Length * StayDuration;
                    dialogContent.color = Color.white;
                    state = "stay";
                }
                break;
            case "stay":
                counter -= Time.deltaTime;
                if(counter <= 0) {
                    counter = 1;
                    state = "fadeOut";
                }
                break;
            case "fadeOut":
                counter -= FadingSpeed;
                dialogContent.color = new Color(255, 255, 255, counter);
                if(counter <= 0) {
                    currentDialog = "";
                    dialogQueue.RemoveAt(0); // remove the 1st dialogue from the queue
                    // if there is still dialogue ramaining in the list, display it
                    if(dialogQueue.Count > 0) ShowDialogue();
                    else state = "stop";
                }
                break;
            default:
                if(dialogQueue.Count > 0) ShowDialogue();
                return;
        }
    }

    public void AddDialogue(String dialog)
    {
        dialogQueue.Add(dialog); // add new dialogue to the end of the queue
    }

    // show the 1st dialogue in the queue
    void ShowDialogue()
    {
        currentDialog = dialogQueue[0]; // set the 1st dialogue as the current dialogue
        dialogContent.text = currentDialog; // assign the new text
        counter = 0;
        state = "fadeIn"; // start fading in
    }
}
