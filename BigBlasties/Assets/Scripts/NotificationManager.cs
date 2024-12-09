using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager mNotiManagrInst;

    [SerializeField] GameObject mNotificationPopup;
    [SerializeField] GameObject mGreenKeyPopup;
    [SerializeField] GameObject mBlueKeyPopup;
    [SerializeField] GameObject mRedKeyPopup;
    [SerializeField] GameObject mBossKeyPopup;

    [SerializeField] float mArmRange;

    public TMP_Text mNotificationText;

    public TMP_Text mGreenKeyText;

    public RaycastHit mInteract;

    private void Start()
    {
        mNotiManagrInst = this;
    }
    public void ShowNotification(string message)
    {
        mNotificationText.text = message;
        mNotificationPopup.SetActive(true);
    }

    public void HideNotification()
    {
        mNotificationPopup.SetActive(false);
    }
    
    public void ShowGreenKey()
    { 
        mGreenKeyPopup.SetActive(true);
    }

    public void ShowBlueKey()
    {
        mBlueKeyPopup.SetActive(false);
    }

    public void ShowRedKey()
    {
        mRedKeyPopup.SetActive(true);
    }

    public void ShowBossKey()
    {
        mBossKeyPopup.SetActive(true);
    }
}
