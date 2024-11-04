using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager mInstance;

    [SerializeField] GameObject mMenuActive;
    [SerializeField] GameObject mMenuPause;


    public GameObject mPlayer;

    private bool mPaused;
    private float mTimeScaleOrig;

    public bool GetPausedState() { return mPaused; }
    public void SetPausedState(bool state) { state = mPaused; }
    // Start is called before the first frame update

    void Awake()
    {
        //grants a single instance to the game object
        mInstance = this;
        //tracks the player
        mPlayer = GameObject.FindWithTag("Player");
        // Update is called once per frame
        mTimeScaleOrig = Time.timeScale;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")) 
        {
            if (mMenuActive == null)
            {
                StatePaused();
                mMenuActive = mMenuPause;
                mMenuActive.SetActive(true);
            }
            else if (mMenuActive = mMenuPause)
            {
                StateUnpaused();
            }
        }
    }


    public void Paused()
    {

    }

    public void StatePaused()
    {
        //pauses the game by setting the menu active to pause
        SetPausedState(!mPaused);
        mMenuActive = mMenuPause;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StateUnpaused()
    {
        SetPausedState(!mPaused);
        Time.timeScale = mTimeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mMenuActive.SetActive(false);
        mMenuActive = null;
    }
}
