using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillRoomDetector : MonoBehaviour
{
    private KillRoomDetector mKillRoomInst;

    [Header("The GameObject you wish to move")]
    [SerializeField] GameObject mDeadMansDoor;

    [Header("Speed that it moves")]
    [SerializeField] float mSpeed;

    [Header("Input the tag you want the detector to track")]
    [SerializeField] string mTagForDetection;

    [Header("Write either Up, Down, or Rest *case sensitive*")]
    [Header(" for direction after detection is absent")]
    [SerializeField] string mUpOrDownOrRest;

    [Header("Positions")]
    [SerializeField] GameObject mUp;
    [SerializeField] GameObject mRest;
    [SerializeField] GameObject mDown;
    public bool mMoveDoor;

    private void Start()
    {
        mKillRoomInst = this;
    }
    private void Update()
    {
        if (mMoveDoor)
        {
            if (mUpOrDownOrRest == "Up")
            {
               UpDoor();
                Debug.Log("Door Up");
            }
            else if (mUpOrDownOrRest == "Down")
            {
                DownDoor();
                Debug.Log("Door Down");
            }
            else if (mUpOrDownOrRest == "Rest")
            {
                RestDoor();
                Debug.Log("Door Rest");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(mTagForDetection))
        {
            Debug.Log($"the tag grabbed was {mTagForDetection}");
            StartCoroutine(MoveDoor());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI"))
        {
            Debug.Log("Enemy in");
        }
    }

    IEnumerator MoveDoor()
    {
        mMoveDoor = true;
        yield return new WaitForSeconds(1f);
        mMoveDoor = false;
        Destroy(this.gameObject);
        Debug.Log("Tag Left, and this is to show where it sits, the debug...");
    }

    void UpDoor()
    {
        mDeadMansDoor.transform.position = Vector3.MoveTowards(mDeadMansDoor.transform.position, mUp.transform.position, mSpeed * Time.deltaTime);
    }

    void DownDoor()
    {
        mDeadMansDoor.transform.position = Vector3.MoveTowards(mDeadMansDoor.transform.position, mDown.transform.position, mSpeed * Time.deltaTime);
    }

    void RestDoor()
    {
        mDeadMansDoor.transform.position = Vector3.MoveTowards(mDeadMansDoor.transform.position, mRest.transform.position, mSpeed * Time.deltaTime);
    }
}
