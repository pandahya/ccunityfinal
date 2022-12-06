using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    CharacterController controller;
    float InputX, InputY;
    public float normalSpd = 4;
    public float runSpd = 10;

    public bool isGrounded;

    // Footstep sound
    bool isMuted = false;
    float timePassed, voicePlayInterval;
    [SerializeField] GameObject FootStepSFX;
    AudioSource audioData;

    void Awake()
    {
        // get the controller
        controller = this.GetComponent<CharacterController> ();
        // get the audio data
        if(FootStepSFX != null)
        {
            audioData = FootStepSFX.GetComponent<AudioSource> ();
            timePassed = 0;
            voicePlayInterval = 0.5f;
        }
        else isMuted = true;
    }

    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");

        // check if is running
        float spd;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            spd = runSpd; // when running, change to the running speed
            voicePlayInterval = 0.3f; // change the voicePlayInterval
        }
        else
        {
            spd = normalSpd;
            voicePlayInterval = 0.5f; // reset the voicePlayInterval
        }
        
        // play the footstep SFX
        if(!isMuted && (InputX != 0 || InputY != 0))
        {
            timePassed += Time.deltaTime;
            if(timePassed > voicePlayInterval)
            {
                timePassed = 0; // reset passed time
                // audioData.Play(); // play the audio
            }
        }

        // set the moving direction
        Vector3 direction = transform.forward * InputY + transform.right * InputX;
        // reset Y speed (if not grounded, add gravity)
        isGrounded = controller.isGrounded;
        float speedY = isGrounded ? 0f : -0.5f;
        // move the character
        controller.Move(new Vector3(direction.x, speedY, direction.z) * spd * Time.deltaTime);

        // quit game
        if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
}
