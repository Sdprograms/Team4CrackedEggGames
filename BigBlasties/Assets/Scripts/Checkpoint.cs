using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Renderer mModel;
    [SerializeField] ParticleSystem mVFX;

    Color mColorOrig;
    // Start is called before the first frame update
    void Start()
    {
        mColorOrig = mModel.material.color;
        mVFX.Pause();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transform.position != GameManager.mInstance.mPlayerSpawnPos.transform.position) //checks the tag and if the current spawn point is at the collided spawn point
        {
            GameManager.mInstance.mPlayerSpawnPos.transform.position = transform.position;
            StartCoroutine(SpawnChecked());
        }
    }
    
    IEnumerator SpawnChecked()
    {
        mModel.material.color = Color.green;
        yield return new WaitForSeconds(0.3f);
        mModel.material.color = mColorOrig;
        mVFX.Play();
        yield return new WaitForSeconds(0.7f);

    }
}
