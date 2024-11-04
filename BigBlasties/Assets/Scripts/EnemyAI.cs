using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, damageInterface
{
    [SerializeField] int HP;
    [SerializeField] int MaxHP;

    Color orginalColor;
   

    // on start set HP to max HP, saving hp and Max HP seperately for possible 'next level' functionality.
    void Start()
    {
        HP = MaxHP;
    }

    void Update()
    {
        
    }

    //enemy take damage function
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(hitmarker());
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    //IEnumerator so that when enemy takes damage a hitmarker appears on the UI.
    IEnumerator hitmarker()
    {
        //Gamemanager.instance.playerhitmarker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        //Gamemanager.instance.playerhitmarker.SetActive(false);
        
    }
}
