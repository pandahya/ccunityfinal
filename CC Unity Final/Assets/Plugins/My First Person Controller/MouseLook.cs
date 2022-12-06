using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivityX = 8f;
    public float sensitivityY = 8f;
    // private float minimumX = -85f;
	// private float maximumX = 85f;
 
	// private float minimumY = -360f;
	// private float maximumY = 360f;

    [SerializeField]
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        // lock the mouse in the center
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Transform player = this.transform;

        // calculate result angles
        float targetAngleX = mainCamera.transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivityX * Time.timeScale;
        float targetAngleY = player.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityY * Time.timeScale;
        // constrain the angles
        // float finRotationX = ClampAngle(targetAngleX, minimumX, maximumX, false);
        // float finRotationY = ClampAngle(targetAngleY, minimumY, maximumY, false);
        float finRotationX = targetAngleX;
        float finRotationY = targetAngleY;

        // apply rotations (unroll Z-axis angle)
        mainCamera.transform.localEulerAngles = new Vector3(finRotationX, 0f, 0f);
        player.localEulerAngles = new Vector3(0f, finRotationY, 0f);
    }

    // dynamically set new sensitivity during the game
    public void SetSensitivity(float s)
	{
		sensitivityX = s;
		sensitivityY = s;
	}

    public static float ClampAngle(float angle, float min, float max, bool mode)
	{
        // mode: true - loop; false - constrained
        if(mode)
        {
            if(angle < min) angle += 360f;
            if(angle > max) angle -= 360f;
		    return angle;
        }
        else
        {
            return Mathf.Clamp(angle, min, max);
        }
	}
}
