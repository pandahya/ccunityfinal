using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAnim : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 9.0f;
    [SerializeField] float RotateSpeed = 8.0f;
    [SerializeField] bool DestroyOnPicked = false;
    private bool isMoving;
    private Transform originParent;
    private Vector3 targetPos = Vector3.zero; // target pos is always (0,0,0) of parent carrier
    private Quaternion targetRot = Quaternion.identity; // target rot is always 0

    [SerializeField] List<Vector3> preselectedPos;
    [SerializeField] List<Quaternion> preselectedRot;

    void Awake()
    {
        originParent = this.transform.parent; // record the original parent

        // set default preselectedPos & preselectedRot
        if(preselectedPos.Count <= 0) preselectedPos.Add(this.transform.position);
        if(preselectedRot.Count <= 0) preselectedRot.Add(this.transform.rotation);
    }
    void Update()
    {
        if(isMoving)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, MoveSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, RotateSpeed * Time.deltaTime);
            // stop moving
            if(Vector3.Distance(transform.localPosition, targetPos) < 0.01)
            {
                isMoving = false;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                // destroy the object
                if(DestroyOnPicked) Destroy(gameObject, 0.2f);
            }
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }
    public void DropDown(int num) // drop down the item to a preselected point
    {
        transform.position = preselectedPos[num];
        transform.rotation = preselectedRot[num];
    }

    public Transform GetOriginParent()
    {
        return originParent;
    }
}
