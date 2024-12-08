using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginePuzzleDoor : MonoBehaviour
{
    [SerializeField] GameObject mOpenPos;

    [SerializeField] GameObject mKeyOne;
    [SerializeField] GameObject mLockOne;
    [SerializeField] GameObject mLightOne;

    [SerializeField] GameObject mKeyTwo;
    [SerializeField] GameObject mLockTwo;
    [SerializeField] GameObject mLightTwo;

    [SerializeField] GameObject mKeyThree;
    [SerializeField] GameObject mLockThree;
    [SerializeField] GameObject mLightThree;


    public bool mOneSolved;
    public bool mTwoSolved;
    public bool mThreeSolved;
    bool mDoorOpen;
    private void Start()
    {
        mLightOne.SetActive(false);
        mLightTwo.SetActive(false);
        mLightThree.SetActive(false);
    }

    private void Update()
    {
        IsOneSolved();
        IsTwoSolved();
        IsThreeSolved();
        CanDoorOpen();
    }

    private void IsOneSolved()
    {
        if (mKeyOne.transform.position == mLockOne.transform.position && !mOneSolved)

        {
            Debug.Log("Unlocked One");
            StartCoroutine(OneSolved());
        }
    }

    private void IsTwoSolved()
    {
        if (mKeyTwo.transform.position == mLockTwo.transform.position && !mTwoSolved)
        {
            Debug.Log("Unlocked Two");
            StartCoroutine(TwoSolved());
        }
    }

    private void IsThreeSolved()
    {
        if (mKeyThree.transform.position == mLockThree.transform.position && !mThreeSolved)
        {
            Debug.Log("Unlocked Three");
            StartCoroutine(ThreeSolved());
        }
    }

    private void CanDoorOpen()
    {
        if (mOneSolved && mTwoSolved && mThreeSolved)
        {
            StartCoroutine(MoveDoor());
        }
    }

    IEnumerator OneSolved()
    {
        mOneSolved = true;
        mLightOne.SetActive(true);
        yield return null;
    }

    IEnumerator TwoSolved()
    {
        mTwoSolved = true;
        mLightTwo.SetActive(true);
        yield return null;
    }

    IEnumerator ThreeSolved()
    {
        mThreeSolved = true;
        mLightThree.SetActive(true);
        yield return null;
    }

    IEnumerator MoveDoor()
    {
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, mOpenPos.transform.localPosition, 5f * Time.deltaTime);
        yield return new WaitForSeconds(5f);
    }
}
