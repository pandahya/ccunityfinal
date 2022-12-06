using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectCollider : MonoBehaviour
{
    // detection parameters
    [SerializeField]
    private LayerMask pickableLayerMask;

    [SerializeField]
    private Transform playerCameraTransform;

    [SerializeField]
    private GameObject pickupUI;

    [SerializeField]
    [Min(1)]
    private float hitRange = 2;

    // pick-up parameters
    [SerializeField]
    private Transform pickupParent;
    [SerializeField]
    private GameObject pickedItem;

    private RaycastHit hit;

    private void Start()
    {

    }
    void Update()
    {
        // keep detecting which object is selected currently
        if(hit.collider != null) // reset selected object in each frame
        {
            hit.collider.GetComponent<InteractableCollider>()?.ToggleHighlight(false); // disable highlight state
            pickupUI.SetActive(false); // disable the UI
        }
        if(Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out hit, hitRange, pickableLayerMask))
        {
            hit.collider.GetComponent<InteractableCollider>()?.ToggleHighlight(true); // enable highlight state
            pickupUI.SetActive(true); // enable the UI
            pickupUI.GetComponent<Text>().text = hit.collider.GetComponent<InteractableCollider>()?.GetTip(); // set the corresponding tip
        }

        // trigger interacting when the interact button is pressed
        if(Input.GetKeyDown(KeyCode.E) && hit.collider != null)
        {
            // Debug.Log(hit.collider.name);
            Interact();
        }
    }

    // interact with the selected object
    private void Interact()
    {
        // trigger the point
        bool isPickable = hit.collider.GetComponent<InteractableCollider>().Trigger();
        // if there is a pickable object
        if(isPickable)
        {
            // assign the object to picked obejct
            pickedItem = hit.collider.GetComponent<InteractableCollider>().GetBindingObject();
            // assign to pickupParent of the player
            pickedItem.transform.SetParent(pickupParent.transform);
            // start moving animation
            pickedItem.GetComponent<PickupAnim>()?.StartMoving();
            // disable the rigidbody of picked obj if needed
            Rigidbody rb = pickedItem.GetComponent<Rigidbody>();
            if(rb != null) rb.isKinematic = true;

            return;
        }
    }
}
