using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryScript : MonoBehaviour, damageInterface
{
    [SerializeField] float HP;
    [SerializeField] float MaxHP;
    private GiantMech giantMech;
    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(GiantMech mech)
    {
        giantMech = mech;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(hitmarker());
        if (HP <= 0)
        {
            Destroy(gameObject);
            giantMech.OnBatteryCellDestroyed(gameObject);
            GameManager.mInstance.mEnemyDamageHitmarker.SetActive(false);
        }
    }

    IEnumerator hitmarker()
    {
        GameManager.mInstance.mEnemyDamageHitmarker.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        GameManager.mInstance.mEnemyDamageHitmarker.SetActive(false);
    }
}
