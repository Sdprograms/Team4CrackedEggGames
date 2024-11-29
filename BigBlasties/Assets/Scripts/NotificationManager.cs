using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager mNotiManagrInst;

    [SerializeField] GameObject mNotificationPopup;
    public TMP_Text mNotificationText;

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
}
