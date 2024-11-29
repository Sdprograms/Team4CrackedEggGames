using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchBehavior : MonoBehaviour
{
    public static SwitchBehavior mSwitchInst;

    [Header("Objects")]
    [SerializeField] GameObject mSwitch;
    [SerializeField] GameObject mSwitchableDoor;

    [Header("Positions")]
    [SerializeField] GameObject mDoorOff;
    [SerializeField] GameObject mDoorOn;

    [Header("Modifiers")]
    [SerializeField] int mMoveSpeed;

    public bool mOn;
    public bool mOpen;

    Vector3 mDoorPos;
    Vector3 mDoorOffPos;
    Vector3 mDoorOnPos;

    private void Start()
    {
        mSwitchInst = this;
        mOn = false;
        mOpen = false;
        mSwitchableDoor.GetComponent<GameObject>();

        mDoorPos = mSwitchableDoor.transform.position;
        mDoorOffPos = mDoorOff.transform.position;
        mDoorOnPos = mDoorOn.transform.position;
    }

    private void Update()
    {
        if (mOpen == true)
        {
            mSwitchableDoor.transform.position = Vector3.MoveTowards(mSwitchableDoor.transform.position, mDoorOn.transform.position, mMoveSpeed * Time.deltaTime);
        }
        else if (mOpen == false)
        {
            mSwitchableDoor.transform.position = Vector3.MoveTowards(mSwitchableDoor.transform.position, mDoorOff.transform.position, mMoveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //checks if the player is in the collider
        if (other.CompareTag("Player") && Input.GetButtonDown("Interact"))
        {
                //move the switch and make it pretty
                StartCoroutine(SwitchDoorOpen());  
        }
        NotificationManager.mNotiManagrInst.ShowNotification("Activate");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NotificationManager.mNotiManagrInst.HideNotification();
        }
    }
    IEnumerator SwitchDoorOpen()
    { 
        Debug.Log("Clicked Switch");
        mOpen = !mOpen;

        yield return null;
    }
}
