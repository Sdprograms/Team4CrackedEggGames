using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour, damageInterface
{
    public static DestructableObject instance;
    [SerializeField] float HP;
    [SerializeField] float MaxHP;
    [SerializeField] GameObject explosionObject;
    [SerializeField] ParticleSystem deathEffect;

    [SerializeField] public AudioSource audioSource;
    [SerializeField] AudioClip tookdmgAudioClip;
    [SerializeField] ItemDrop dropScript;
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this; // needed for the volume -XB
        HP = MaxHP;
        if(audioSource != null)
        audioSource.clip = tookdmgAudioClip;
    }

    // Update is called once per frame

    public void takeDamage(int amount)
    {
        HP -= amount;

        if(audioSource != null && tookdmgAudioClip !=null) 
        audioSource.PlayOneShot(tookdmgAudioClip);

        if (HP <= 0)
        {
            //Instantiate death effect.
            if (deathEffect != null)
            Instantiate(deathEffect, transform.position, transform.rotation);

            if (explosionObject != null)
            Instantiate(explosionObject, transform.position, transform.rotation);

            if (dropScript != null)
            {
                dropScript.Drop();
            }
            Destroy(gameObject);
        }
    }

    public float getHP()
    {
        return HP;
    }
    public void setHP(float amount)
    {
        HP = amount;
    }
}
