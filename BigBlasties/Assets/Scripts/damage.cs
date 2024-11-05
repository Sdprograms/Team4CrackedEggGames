using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{

    enum damageType { bullet, stationary, grenade, explosive};
    [SerializeField] damageType dmgType;
    [SerializeField] Rigidbody rbody;
    [SerializeField] GameObject explosion;

    [SerializeField] int damageAmount;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        if (dmgType == damageType.bullet)//if bullet
        {
            rbody.velocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
        else if (dmgType == damageType.stationary) // if stationary
        {
            //In case we think of anything for stationary.
        }
        else if (dmgType == damageType.grenade) //if explosive
        {
            rbody.velocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
        else if (dmgType == damageType.explosive)
        {
            Destroy(gameObject, destroyTime);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        damageInterface dmg = other.GetComponent<damageInterface>();

        if (dmg != null)
        {
            dmg.takeDamage(damageAmount);
        }
        if (dmgType == damageType.bullet)
        {
            Destroy(gameObject);
        }
        else if (dmgType == damageType.grenade)
        {
            //spawn explosion.
            Instantiate(explosion, transform.position, transform.rotation);
            //destroy object.
            Destroy(gameObject);
        }
    }
}
