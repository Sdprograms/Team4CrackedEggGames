using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mOtherText = this.GetComponent<TMP_Text>();
            mtext.text = mOtherText.text;
            mInfoPopUp.SetActive(true);

            StartCoroutine(HideText());
        }
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(150f *  Time.deltaTime);
        mInfoPopUp.SetActive(false);
        if (mDestroyIfOne == 1)
        {
            Destroy(this);
        }
    }
}
