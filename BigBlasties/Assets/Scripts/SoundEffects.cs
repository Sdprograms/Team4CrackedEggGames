using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public static SoundEffects noiseMaker;
    public AudioSource weaponSource, /*stepSource,*/ swapWeaponSouce, levelSoundSource;
    public AudioClip laser, grenadeShot, explosion, /*footsteps,*/ laserSwap, grenadeLauncherSwap;

    [Header("Level Music")]
    public AudioClip ambientMusic;

    [SerializeField] float UnPausedVol;
    [SerializeField] float PausedVol;

    private void Awake()
    {
        noiseMaker = this;
        levelSoundSource = GetComponent<AudioSource>();
        //stepSource = GetComponentInChildren<AudioSource>(stepSource);
        //stepSource.clip = footsteps;
    }
    //private void Update()
    //{
    //    if (playerController.mPlayerInstance.moveDirection != Vector3.zero && playerController.mPlayerInstance.jumpCount == 0) // -XB
    //    {
    //        //isMoving = true;
    //        FootstepSound();
    //    }
    //    else
    //    {
    //        //isMoving = false;
    //        FootStepSilence();
    //    }
    //}
    /*public void LaserSound() --SD new system for projectile sounds.
    {
        //sets the clip of source to the laser and plays the sound effect
        weaponSource.clip = laser;
        weaponSource.Play();
    }

    public void GrenadeShotSound()
    {
        weaponSource.clip = grenadeShot;
        weaponSource.Play();
    }

    public void ExplosionSound()
    {
        weaponSource.clip = explosion;
        weaponSource.Play();
    }*/ 
    //public void FootstepSound()
    //{
    //    if (Input.GetButton("Fire1") && playerController.mPlayerInstance.isShooting == false) //if leftclick
    //    {
    //        StartCoroutine(playerController.mPlayerInstance.shoot());
    //        //StartCoroutine(shoot()); //shoot
    //    }
    //    //allows footsteps to play while a weapon is in use
    //    if (!stepSource.isPlaying)
    //    {
    //        stepSource.Play();
    //    }
    //}

    //public void FootStepSilence()
    //{
    //    if (Input.GetButton("Fire1") && playerController.mPlayerInstance.isShooting == false) //if leftclick
    //    {
    //        StartCoroutine(playerController.mPlayerInstance.shoot());
    //        //StartCoroutine(shoot()); //shoot
    //    }
    //    if (stepSource.isPlaying)
    //    {
    //        stepSource.Stop();
    //    }
    //}

    public void LaserSwap()
    {
        swapWeaponSouce.clip = laserSwap;
        swapWeaponSouce.Play();
    }

    public void GrenadeLaunceherSwap()
    {
        swapWeaponSouce.clip = grenadeLauncherSwap;
        swapWeaponSouce.Play();
    }
    
    public void LevelMusic(AudioClip clip)
    {
        levelSoundSource.clip = clip;
        if (levelSoundSource.clip == ambientMusic && !levelSoundSource.isPlaying) //plays ambience when the clip is set to ambient and the player isnt playing
        {
            levelSoundSource.Play();
        }
        if (GameManager.mInstance.mPaused == true)
        {
            levelSoundSource.volume = PausedVol;
        }
        else
        {
            levelSoundSource.volume = UnPausedVol;
        }
    }
}
