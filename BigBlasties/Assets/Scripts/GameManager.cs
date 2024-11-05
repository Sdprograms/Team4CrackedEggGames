using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager mInstance;

    [SerializeField] GameObject mMenuActive;
    [SerializeField] GameObject mMenuPlay;
    [SerializeField] GameObject mMenuPause;
    [SerializeField] GameObject mMenuSettings;
    [SerializeField] GameObject mMenuLose;

    public GameObject mPlayer;

    public bool mPaused;


    private float mTimeScaleOrig;


    public GameObject GetActiveMenu() {  return mMenuActive; }
    public void SetActiveMenu(GameObject menuActive) { mMenuActive = menuActive; }
    //these getters and setters do not update the checkbox in unity
    //public bool GetPausedState() { return mPaused; }
    //public void SetPausedState(bool state) { state = mPaused; }
    // Start is called before the first frame update

    void Awake()
    {
        //grants a single instance to the game object
        mInstance = this;
        //tracks the player
        mPlayer = GameObject.FindWithTag("Player");
        // Update is called once per frame
        mTimeScaleOrig = Time.timeScale;
        mMenuActive = mMenuPlay;
        mMenuActive.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")) 
        {
            if (mMenuActive == mMenuPlay)
            {
                mMenuActive.SetActive(false);
                StatePaused();
            }
            else if (mMenuActive == mMenuPause || mMenuActive == mMenuSettings)
            {
                StateUnpaused();
                mMenuActive = mMenuPlay;
                mMenuActive.SetActive(true);
            }
        }
    }

    public void FreezeGame()
    {
        //halts activity, and allows for caged mouse cursor
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void UnfreezeGame()
    {
        Time.timeScale = mTimeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StatePaused()
    {
        //inverses pause state
        //SetPausedState(true);
        mPaused = !mPaused;
        //sets the active menu to pause
        SetActiveMenu(mMenuPause);
        //makes it visible
        mMenuActive.SetActive(true);
        FreezeGame();
    }

    public void StateUnpaused()
    {
        //SetPausedState(false);
        mPaused = !mPaused;
        mMenuActive.SetActive(false);
        SetActiveMenu(null);
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
        mMenuActive.SetActive(false);
        FreezeGame();
        SetActiveMenu(mMenuLose);
        mMenuActive.SetActive(true);
    }
}
