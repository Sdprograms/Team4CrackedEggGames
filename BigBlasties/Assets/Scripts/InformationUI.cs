using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InformationUI : MonoBehaviour
{
    [SerializeField] GameObject mInfoPopUp;
    [SerializeField] TMP_Text mtext;
    [SerializeField] int mDestroyIfOne;
    TMP_Text mOtherText;

    // Start is called before the first frame update
    private void Start()
    {
        mInfoPopUp = GameObject.Find("UI(official)/Menus/Play Screen/InfoText");
        mtext = mInfoPopUp.GetComponent<TMP_Text>();
        mInfoPopUp.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                mOtherText = this.GetComponent<TMP_Text>();
                mtext.text = mOtherText.text;
                mInfoPopUp.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        mInfoPopUp.SetActive(false);
        if (mDestroyIfOne == 1)
        {
            Destroy(this);
        }
    }
}
