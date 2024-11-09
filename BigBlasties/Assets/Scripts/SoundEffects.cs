using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public static SoundEffects noiseMaker;
    public AudioSource weaponSource, stepSource;
    public AudioClip laser, explosion, footsteps;

    private void Awake()
    {
        noiseMaker = this;
    }
    public void LaserSound()
    {
        //sets the clip of source to the laser and plays the sound effect
        weaponSource.clip = laser;
        weaponSource.Play();
    }

    public void ExplosionSound()
    {
        weaponSource.clip = explosion;
        weaponSource.Play();
    }
    public void FootstepSound()
    {
        //allows footsteps to play while a weapon is in use
        stepSource.clip = footsteps;
        if (!stepSource.isPlaying)
        {
            stepSource.Play();
        }

        //should go into player controller to detect walking and make sound
        //if (moveDirection != Vector3.zero && jumpCount == 0) // -XB
        //{
        //    GameManager.mInstance.mWalkingSound.SetActive(true);
        //}
        //else
        //{
        //    GameManager.mInstance.mWalkingSound.SetActive(false);
        //}

    }
}
