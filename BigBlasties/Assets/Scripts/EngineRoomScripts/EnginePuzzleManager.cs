using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnginePuzzleManager : MonoBehaviour
{
    EnginePuzzleManager mEnginePuzzleManag;

    [SerializeField] List<GameObject> mListOfEngineBlocks;
    [SerializeField] List<GameObject> mListOfBlockPositions;
    [SerializeField] List<GameObject> mListOfCamPositions;

    [SerializeField] GameObject mRestPos;
    [SerializeField] GameObject mRightPos;
    [SerializeField] GameObject mLeftPos;

    [SerializeField] GameObject mMonitorCam;
    [SerializeField] TMP_Text mCamPosition;

    [SerializeField] GameObject mRightSwitch;
    [SerializeField] GameObject mLeftSwitch;
    [SerializeField] GameObject mSwitchBlock;
    [SerializeField] GameObject mSwitchRotClockwise;
    [SerializeField] GameObject mSwitchRotCounterClockwise;

    [SerializeField] float mTime;

    public int listIterator;
    public bool moveRight;
    public bool moveLeft;
    public bool moveRest;
    public bool atRight;
    public bool atLeft;
    public bool rotateClock;
    public bool rotateCounterClock;
    public bool canMove;

    float angle;
    Quaternion nextRotation;

    // Start is called before the first frame update
    void Start()
    {
        mEnginePuzzleManag = this;
        mMonitorCam.transform.SetLocalPositionAndRotation(mListOfCamPositions[listIterator].transform.localPosition, mListOfCamPositions[listIterator].transform.localRotation);
        canMove = true;

        mCamPosition = GameObject.Find("EnginePuzzleRoomComplete 2/EngineRoomScreen/Screen/CameraPositionText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveStates(); 
    }

    void MoveStates()
    {
        PressLeftSwitch();
        PressRightSwitch();

        PressChangeBlock();

        PressClockwise();
        PressCounterClockwise();

        if (canMove)
        {
            if (moveRight && !moveRest)
            {
                StartCoroutine(MoveRight());
            }
            if (moveLeft && !moveRest)
            {
                StartCoroutine(MoveLeft());
            }
            if (moveRest)
            {
                StartCoroutine(MoveRest());
            }
            if (rotateClock)
            {
                StartCoroutine(RotClockwise());
            }
            else if (rotateCounterClock)
            {
                StartCoroutine(RotCounterClockwise());
            }

            if (moveRight && moveLeft)
            {
                moveLeft = false;
                moveRight = false;
                moveRest = true;
            }
        }
    }

    private void PressRightSwitch()
    {
        //simply manages when you press the right switch
        //checks to make sure that the object is loaded and that the hHit is hitting something
        if (mRightSwitch != null && GunRotation.mGunRotInst.mHit.collider != null)
        {
            //should it hit, it checks to see if you have a notification, you're pressing e, and that you're looking at the right switch
            if (GameManager.mInstance.mShowNoti && Input.GetButtonUp("Interact") && GunRotation.mGunRotInst.mHit.transform.name.Equals(mRightSwitch.name))
            {
                    moveRight = true;
            }
        }
    }

    private void PressLeftSwitch()
    {
        //simply manages when you press the left switch
        //checks to make sure that the object is loaded and that the hHit is hitting something
        if (mLeftSwitch != null && GunRotation.mGunRotInst.mHit.collider != null)
        {
            //should it hit, it checks to see if you have a notification, you're pressing e, and that you're looking at the right switch
            if (GameManager.mInstance.mShowNoti && Input.GetButtonUp("Interact") && GunRotation.mGunRotInst.mHit.transform.name.Equals(mLeftSwitch.name))
            {
                    moveLeft = true;
            }
        }
    }

    private void ChangeIterator()
    {
        //iterates through the vectors collectively, resetting when at the last number
        if (listIterator == mListOfEngineBlocks.Count - 1)
        {
            listIterator = 0;
        }
        else
        {
            listIterator++;
        }
    }
    private void PressChangeBlock()
    {
        //checks to make sure that the object is loaded and that the hHit is hitting something
        if (mSwitchBlock != null && GunRotation.mGunRotInst.mHit.collider != null)
        {
            //should it hit, it checks to see if you have a notification, you're pressing e, and that you're looking at the right switch
            if (GameManager.mInstance.mShowNoti && Input.GetButtonDown("Interact") && GunRotation.mGunRotInst.mHit.transform.name.Equals(mSwitchBlock.name))
            {
                canMove = false;
                ChangeIterator();
                StartCoroutine(SwitchBlocks());
            }
        }
    }

    private void PressClockwise()
    {
        //checks to make sure that the object is loaded and that the hHit is hitting something
        if (mSwitchBlock != null && GunRotation.mGunRotInst.mHit.collider != null)
        {
            //should it hit, it checks to see if you have a notification, you're pressing e, and that you're looking at the right switch
            if (GameManager.mInstance.mShowNoti && Input.GetButton("Interact") && GunRotation.mGunRotInst.mHit.transform.name.Equals(mSwitchRotClockwise.name))
            {
                //IMPORTANT, sets the new angle and doesn't change it until the part reaches the set angel, otherwise it gets choppy/doesnt move
                if (rotateClock == false)
                {
                    angle += 45;
                    nextRotation = Quaternion.Euler(0, angle, 0);
                }
                rotateClock = true;
            }
        }
    }
    private void PressCounterClockwise()
    {
        //checks to make sure that the object is loaded and that the hHit is hitting something
        if (mSwitchBlock != null && GunRotation.mGunRotInst.mHit.collider != null)
        {
            //should it hit, it checks to see if you have a notification, you're pressing e, and that you're looking at the right switch
            if (GameManager.mInstance.mShowNoti && Input.GetButton("Interact") && GunRotation.mGunRotInst.mHit.transform.name.Equals(mSwitchRotCounterClockwise.name))
            {
                //IMPORTANT, sets the new angle and doesn't change it until the part reaches the set angel, otherwise it gets choppy/doesnt move
                if (rotateCounterClock == false)
                {
                    angle -= 45;
                    nextRotation = Quaternion.Euler(0, angle, 0);
                }
                rotateCounterClock = true;
            }
        }
    }

    IEnumerator MoveRight()
    {
        //ensures that if the object is in the right position, it moves to rest
        if (moveRest)
        {
            StartCoroutine(MoveRight());
        }
        else if (moveRight && !moveLeft && !atLeft)
        {
            mListOfEngineBlocks[listIterator].transform.localPosition = Vector3.MoveTowards(mListOfEngineBlocks[listIterator].transform.localPosition, mRightPos.transform.localPosition, mTime * Time.deltaTime);
            //once it reaches its destination, it sets appropriate bools
            if (mListOfEngineBlocks[listIterator].transform.localPosition == mRightPos.transform.localPosition)
            {
                moveRight = false;
                atRight = true;
                atLeft = false;
            }
        }

        yield return null;
    }
    IEnumerator MoveLeft()
    {

        //ensures that if the object is in the right position, it moves to rest
        if (moveRest)
        {
            StartCoroutine(MoveLeft());
        }
        else if (moveLeft && !moveRight && !atRight)
        {
            mListOfEngineBlocks[listIterator].transform.localPosition = Vector3.MoveTowards(mListOfEngineBlocks[listIterator].transform.localPosition, mLeftPos.transform.localPosition, mTime * Time.deltaTime);
            //once it reaches its destination, it sets appropriate bools
            if (mListOfEngineBlocks[listIterator].transform.localPosition == mLeftPos.transform.localPosition)
            {
                moveLeft = false;
                atLeft = true;
                atRight = false;
            }
        }

        yield return null;
    }

    IEnumerator MoveRest()
    {
        mListOfEngineBlocks[listIterator].transform.localPosition = Vector3.MoveTowards(mListOfEngineBlocks[listIterator].transform.localPosition, mRestPos.transform.localPosition, mTime * Time.deltaTime);
        yield return null;
        //ensuers all are reset to move in either direction
        if (mListOfEngineBlocks[listIterator].transform.localPosition == mRestPos.transform.localPosition)
        {
            moveRest = false;
            moveLeft = false;
            moveRight = false;
            atRight = false;
            atLeft = false;
        }
    }

    IEnumerator SwitchBlocks()
    {
 
        mRightPos = mListOfBlockPositions[listIterator].gameObject.transform.GetChild(0).gameObject;
        mLeftPos = mListOfBlockPositions[listIterator].gameObject.transform.GetChild(1).gameObject;
        mRestPos = mListOfBlockPositions[listIterator].gameObject.transform.GetChild(2).gameObject;
        angle = mListOfEngineBlocks[listIterator].gameObject.transform.rotation.y;
        rotateClock = false;
        rotateCounterClock = false;

        mCamPosition.text = (listIterator + 1).ToString();

        mMonitorCam.transform.SetLocalPositionAndRotation(mListOfCamPositions[listIterator].transform.localPosition, mListOfCamPositions[listIterator].transform.localRotation);

        canMove = true;
        yield return new WaitForSeconds(mTime);
    }

    IEnumerator RotClockwise()
    {
        //while the ending rotation is set, slerp into its rotation over time * deltaTime and * 2 for speed
        mListOfEngineBlocks[listIterator].transform.rotation = Quaternion.Slerp(mListOfEngineBlocks[listIterator].transform.rotation, nextRotation, mTime * Time.deltaTime * 2);

        if (mListOfEngineBlocks[listIterator].transform.rotation == nextRotation)
        {
            rotateClock = false;
        }
        yield return null;
    }

    IEnumerator RotCounterClockwise()
    {
        //while the ending rotation is set, slerp into its rotation over time * deltaTime and * 2 for speed
        mListOfEngineBlocks[listIterator].transform.rotation = Quaternion.Slerp(mListOfEngineBlocks[listIterator].transform.rotation, nextRotation, mTime * Time.deltaTime * 2);

        if (mListOfEngineBlocks[listIterator].transform.rotation == nextRotation)
        {
            mListOfEngineBlocks[listIterator].transform.rotation = nextRotation;
            rotateCounterClock = false;
        }
        yield return null;
    }
}
