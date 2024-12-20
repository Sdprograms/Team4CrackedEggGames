using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager mInstance;

    [SerializeField] GameObject mMenuActive;
    [SerializeField] GameObject mMenuPlay;
    [SerializeField] GameObject mMenuPause;
    [SerializeField] GameObject mMenuSettings;
    [SerializeField] GameObject mMenuLose;
    [SerializeField] GameObject mMenuWin;

    [SerializeField] public GameObject notePage;

    [SerializeField] public List<GameObject> mListOfKillRoomDets;

    public TMP_Text sensitivityText;

    public Image mPlayerHealth;
    public TMP_Text mAmmoCurrent;
    public TMP_Text mAmmoReserve;
    public TMP_Text mKeyAmount;

    public bool mPaused;

    public cameraController mCameraController;
    public playerController mPlayerController;
    public GameObject mPlayer;
    public GameObject mPlayerSpawnPos;
    public GameObject mPlayerDamageTaken;
    public GameObject mWalkingSound;
    public GameObject mEnemyDamageHitmarker;

    private int mLookSensDisplay;
    private float mTimeScaleOrig;

    public int mEnemyCount;

    public GameObject GetActiveMenu() {  return mMenuActive; }
    public void SetActiveMenu(GameObject menuActive) { mMenuActive = menuActive; }

    public bool mShowNoti;
    public bool mIsLocked;
    public bool mIsOpen;

    public bool mNoteActive;

    public GameObject GetMenuPause() 
    {
        mMenuActive.SetActive(false);
        mMenuActive = mMenuPause;
        mMenuActive.SetActive(true);
        return mMenuPause; 

    }
    //these getters and setters do not update the checkbox in unity
    //public bool GetPausedState() { return mPaused; }
    //public void SetPausedState(bool state) { state = mPaused; }
    // Start is called before the first frame update

    void Awake()
    {
  

        //grants a single instance to the game object
        mInstance = this;
        //tracks the player
        mPlayer = GameObject.Find("Player");
        mPlayerController = mPlayer.GetComponent<playerController>();

        GameObject mCamera = GameObject.Find("Player/Main Camera");
        mCameraController = mCamera.GetComponent<cameraController>();
        // Update is called once per frame
        mTimeScaleOrig = Time.timeScale;
        SetActiveMenu(mMenuPlay);
        mMenuActive.SetActive(true);
        mPlayerSpawnPos = GameObject.Find("PlayerSpawnPoint");//finds the players spawn
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !mNoteActive) 
        {
            if (mMenuActive == mMenuPlay)
            {
                mMenuActive.SetActive(false);
                StatePaused();
            }
            else if (mMenuActive == mMenuPause || mMenuActive == mMenuSettings)
            {              
                StateUnpaused();
            }
        }

        if (mShowNoti == true)
        {
            if (mIsLocked)
            {
                NotificationManager.mNotiManagrInst.ShowNotification("Locked");
            }
            else if (!mIsLocked && !mIsOpen)
            {
                NotificationManager.mNotiManagrInst.ShowNotification("Press 'E'");
            }
            else if (mIsOpen)
            {
                NotificationManager.mNotiManagrInst.ShowNotification("");
            }
        }
        else
        {
            NotificationManager.mNotiManagrInst.HideNotification();
        }

        SoundEffects.noiseMaker.LevelMusic(SoundEffects.noiseMaker.ambientMusic); // plays ambience

        if (mNoteActive == true && Input.GetButtonUp("Interact") || mNoteActive == true && Input.GetButton("Cancel"))
        {
            notePage.SetActive(false);
            mNoteActive = false;
            StateUnpaused();
        }

    }

    public void FreezeGame()
    {
        //inverses pause state
        mPaused = !mPaused;
        //halts activity, and allows for caged mouse cursor
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void UnfreezeGame()
    {
        mPaused = !mPaused;
        Time.timeScale = mTimeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StatePaused()
    {
        mMenuActive.SetActive(false);
        //sets the active menu to pause
        SetActiveMenu(mMenuPause);
        //makes it visible
        mMenuActive.SetActive(true);
        FreezeGame();
    }

    public void StateUnpaused()
    {

        mMenuActive.SetActive(false);
        SetActiveMenu(null);
        mMenuActive = mMenuPlay;
        mMenuActive.SetActive(true);
        UnfreezeGame();
    }

    public void StateSettingsOn()
    {
        //brings up the settings menu by turning off the pause menu, shifting active menu to settings, and setting that to active
        mMenuActive.SetActive(false);
        SetActiveMenu(mMenuSettings);
        mMenuActive.SetActive(true);
        if (Input.GetButtonDown("Cancel"))
        {
            StateSettingsOff();
            StateUnpaused();
        }
    }
    public void StateSettingsOff()
    {
        //turns off the settings window
        mMenuActive.SetActive(false);
        StatePaused();
    }

    //turns off the play hud, stops play and open the lose menu upon death
    public void GameOver()
    {
        mPaused = true;
        mMenuActive.SetActive(false);
        FreezeGame();
        SetActiveMenu(mMenuLose);
        mMenuActive.SetActive(true);
    }

    public void Win()
    {
        mPaused = true;
        mMenuActive.SetActive(false);
        FreezeGame();
        SetActiveMenu(mMenuWin);
        mMenuActive.SetActive(true);
    }

    public void UpdateEnemyCount(int count)
    {
        mEnemyCount += count;
    }
}
