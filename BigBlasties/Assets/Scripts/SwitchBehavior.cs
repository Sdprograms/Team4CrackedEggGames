using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public bool mCanOpen;

    public bool mShowNoti;

    Vector3 mDoorPos;
    Vector3 mDoorOffPos;
    Vector3 mDoorOnPos;

    private void Start()
    {
        mSwitchInst = this;
        mOn = false;
        mOpen = false;
        mSwitchableDoor.GetComponent<GameObject>();

        //      For some reason, the engine doesn't like pre set vector3's
        //mDoorPos = mSwitchableDoor.transform.position;
        //mDoorOffPos = mDoorOff.transform.position;
        //mDoorOnPos = mDoorOn.transform.position;
    }

    private void Update()
    {
        if (mOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

        if (mShowNoti)
        {
            NotificationManager.mNotiManagrInst.ShowNotification("Activate");
        }
        else
        {
            NotificationManager.mNotiManagrInst.HideNotification();
        }
    }

    private void OpenDoor()
    {
        mSwitchableDoor.transform.position = Vector3.MoveTowards(mSwitchableDoor.transform.position, mDoorOn.transform.position, mMoveSpeed * Time.deltaTime);
    }
    private void CloseDoor()
    {
        mSwitchableDoor.transform.position = Vector3.MoveTowards(mSwitchableDoor.transform.position, mDoorOff.transform.position, mMoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !mShowNoti)
        {
            mShowNoti = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //checks if the player is in the collider 
        if (other.CompareTag("Player") && Input.GetButton("Interact") && !mCanOpen)
        {

            Debug.Log("E Pressed");

            //move the switch and make it pretty
            //StartCoroutine(SwitchDoorOpen());
            mCanOpen = true;
            StartCoroutine(SwitchDoorOpen());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && mShowNoti)
        {
            mShowNoti = false;
        }
    }
    IEnumerator SwitchDoorOpen()
    {
        Debug.Log("Clicked Switch");
        mOpen = !mOpen;
        yield return new WaitForSeconds(.5f);
        mCanOpen = false;
    }
}
