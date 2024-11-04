using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, damageInterface
{
    [SerializeField] Renderer model;

    [SerializeField] int HP;
    [SerializeField] int MaxHP;

    Color orginalColor;
   
    void Start()
    {
        orginalColor = model.material.color;
        HP = MaxHP;
    }

    void Update()
    {
        
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(hitmarker());
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator hitmarker()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = orginalColor;
    }
}
