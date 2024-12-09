using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KillRoomDetector : MonoBehaviour
{
    public static KillRoomDetector mKillRoomInst;

    [Header("The GameObject you wish to move")]
    [SerializeField] public GameObject mDeadMansDoor;

    [Header("Speed that it moves")]
    [SerializeField] float mSpeed;

    [Header("Write either Up, Down, or Rest *case sensitive*")]
    [Header(" for direction after detection is absent")]
    [SerializeField] string mUpOrDownOrRest;

    [Header("Positions")]
    [SerializeField] GameObject mUp;
    [SerializeField] GameObject mRest;
    [SerializeField] GameObject mDown;

    public Vector3 mDoorOrigPos;

    [SerializeField] GlobalSpawner[] mSpawners;

    public bool mMoveDoor;
    public bool mSpawnEnemies;
    public int mSpawnedEnemies;
    public int mEnemiesToSpawn;

    private void Start()
    {
        mKillRoomInst = this;
        CountSpawned();
        mDoorOrigPos = mDeadMansDoor.transform.position;
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && this.mSpawners.Count() != 0)
        {
            GameManager.mInstance.mEnemyCount = mEnemiesToSpawn;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // this logic is particular with closing the door behing the player,
        // when you want to close the door is far enough into the room to stop fleeing from the player
        if (other.CompareTag("Player") && this.mSpawners.Count() == 0 && GameManager.mInstance.mEnemyCount == 0)
        {
            StartCoroutine(MoveDoor());
        }

    }

    private void OnTriggerStay(Collider other)
    {
        //checks if all enemies have been spawned
        if (mSpawnedEnemies == mEnemiesToSpawn)
        {
            //checks if the player is still in the room, and if all enemies have been destroyed that have been spawned
            if (other.CompareTag("Player") && GameManager.mInstance.mEnemyCount <= 0)
            {
                StartCoroutine(MoveDoor());
                mSpawnEnemies = false;
            }
        }
        else
        {
            mSpawnEnemies = true;
        }
    }

    IEnumerator MoveDoor()
    {
        //moves the door and then destroys the detector, leaving the door and its positions intact
        mMoveDoor = true;
        yield return new WaitForSeconds(1f);
        mMoveDoor = false;
        Destroy(this.gameObject);
    }

    void UpDoor()
    {
        //moves the door to up
        mDeadMansDoor.transform.position = Vector3.MoveTowards(mDeadMansDoor.transform.position, mUp.transform.position, mSpeed * Time.deltaTime);
    }

    void DownDoor()
    {
        //moves the door to down
        mDeadMansDoor.transform.position = Vector3.MoveTowards(mDeadMansDoor.transform.position, mDown.transform.position, mSpeed * Time.deltaTime);
    }

    void RestDoor()
    {
        //moves the door to rest
        mDeadMansDoor.transform.position = Vector3.MoveTowards(mDeadMansDoor.transform.position, mRest.transform.position, mSpeed * Time.deltaTime);
    }

    void CountSpawned()
    {
        //tallies up the number of enemies to spawn if the detector has enemies to count
        if (mSpawners.Length > 0)
        {
            for (int i = 0; i < mSpawners.Length; i++)
            {
                mEnemiesToSpawn += mSpawners[i].numberToSpawn;
            }
        }
    }
}
