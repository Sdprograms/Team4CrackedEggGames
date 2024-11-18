using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Renderer mModel;
    [SerializeField] GameObject mVFX;

    Color mColorOrig;
    // Start is called before the first frame update
    void Start()
    {
        mColorOrig = mModel.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
