using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour, damageInterface
{
    [SerializeField] int HP;
    [SerializeField] int MaxHP;
 

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0 )
        {
            //Instantiate death effect.
            Destroy(gameObject);
        }
    }
}
