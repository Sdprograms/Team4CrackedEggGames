using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddedSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;
    // Start is called before the first frame update

    private void Start()
    {
        if(audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
