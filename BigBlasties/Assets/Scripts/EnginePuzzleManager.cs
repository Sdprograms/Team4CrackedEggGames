using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnginePuzzleManager : MonoBehaviour
{
    EnginePuzzleManager mEnginePuzzleManag;

    [SerializeField] List<GameObject> mListOfEngineBlocks;
    [SerializeField] List<GameObject> mListOfBlockPositions;
    [SerializeField] GameObject mRestPos;
    [SerializeField] GameObject mRightPos;
    [SerializeField] GameObject mLeftPos;

    [SerializeField] GameObject mRightSwitch;
    [SerializeField] GameObject mLeftSwitch;
    [SerializeField] GameObject mSwitchBlock;

    [SerializeField] float mTime;

    public int listIterator;
    public bool moveRight;
    public bool moveLeft;
    public bool moveRest;

    // Start is called before the first frame update
    void Start()
    {
        mEnginePuzzleManag = this;

        if (mLeftSwitch == null)
        {
            Debug.Log("the left switch isn't assigned");
        }
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

        if (moveRight)
        {
            StartCoroutine(MoveRight());
        }
        if (moveLeft)
        {
            StartCoroutine(MoveLeft());
        }
    }

    private void PressRightSwitch()
    {
        //simply manages when you press the right switch
        if (mRightSwitch != null && GunRotation.mGunRotInst.mHit.collider != null)
        {
            if (GameManager.mInstance.mShowNoti && Input.GetButton("Interact") && GunRotation.mGunRotInst.mHit.transform.name.Equals(mRightSwitch.name))
            {
                Debug.Log("Cast hit, and shoud move now!");
                moveRight = true;
            }
        }
    }

    private void PressLeftSwitch()
    {
        //simply manages when you press the left switch
        if (mLeftSwitch != null && GunRotation.mGunRotInst.mHit.collider != null)
        {
            if (GameManager.mInstance.mShowNoti && Input.GetButton("Interact") && GunRotation.mGunRotInst.mHit.transform.name.Equals(mLeftSwitch.name))
            {
                Debug.Log("Cast hit, and shoud move now!");
                moveLeft = true;
            }
        }
    }

    public void changeListIterator()
    {
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
        if (mSwitchBlock != null && GunRotation.mGunRotInst.mHit.collider != null)
        {
            if (GameManager.mInstance.mShowNoti && Input.GetButtonDown("Interact") && GunRotation.mGunRotInst.mHit.transform.name.Equals(mSwitchBlock.name))
            {
                Debug.Log("Switched Blocks");
         
                changeListIterator();

                StartCoroutine(SwitchBlocks());
            }
        }
    }

    IEnumerator MoveRight()
    {
        if (moveRest)
        {
            StartCoroutine(MoveRest());
        }
        else
        {
            mListOfEngineBlocks[listIterator].transform.position = Vector3.MoveTowards(mListOfEngineBlocks[listIterator].transform.position, mRightPos.transform.position, mTime * Time.deltaTime);
            if (mListOfEngineBlocks[listIterator].transform.position == mRightPos.transform.position)
            {
                moveRight = false;
                moveRest = true;
            }
        }
        yield return null;
    }
    IEnumerator MoveLeft()
    {
        if (moveRest)
        {
            StartCoroutine(MoveRest());
        }
        else
        {
            mListOfEngineBlocks[listIterator].transform.position = Vector3.MoveTowards(mListOfEngineBlocks[listIterator].transform.position, mLeftPos.transform.position, mTime * Time.deltaTime);
            if (mListOfEngineBlocks[listIterator].transform.position == mLeftPos.transform.position)
            {
                moveLeft = false;
                moveRest = true;
            }
        }
        yield return null;
    }

    IEnumerator MoveRest()
    {
        mListOfEngineBlocks[listIterator].transform.position = Vector3.MoveTowards(mListOfEngineBlocks[listIterator].transform.position, mRestPos.transform.position, mTime * Time.deltaTime);
        yield return null;
        if (mListOfEngineBlocks[listIterator].transform.position == mRestPos.transform.position)
        {
            moveRest = false;
            moveLeft = false;
            moveRight = false;
        }
    }

    IEnumerator SwitchBlocks()
    {
        mRightPos = mListOfBlockPositions[listIterator].gameObject.transform.GetChild(0).gameObject;
        mLeftPos = mListOfBlockPositions[listIterator].gameObject.transform.GetChild(1).gameObject;
        mRestPos = mListOfBlockPositions[listIterator].gameObject.transform.GetChild(2).gameObject;
        yield return null;
    }
}
