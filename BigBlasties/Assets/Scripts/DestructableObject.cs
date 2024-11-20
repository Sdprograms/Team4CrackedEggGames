using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour, damageInterface
{
    [SerializeField] int HP;
    [SerializeField] int MaxHP;
    [SerializeField] ParticleSystem deathEffect;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip tookdmgAudioClip;
 

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        audioSource.clip = tookdmgAudioClip;
    }

    // Update is called once per frame

    public void takeDamage(int amount)
    {
        HP -= amount;
        audioSource.PlayOneShot(tookdmgAudioClip);

        if (HP <= 0 )
        {
            //Instantiate death effect.
            Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
