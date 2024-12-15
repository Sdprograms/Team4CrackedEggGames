using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] float yoffsetAmount;
    [SerializeField] bool spawnTime;
    [SerializeField] float spawnAmount;

    int spawnCount;
    private float spawnTimer;

    private void Start()
    {
        if(spawnTime)
        Drop();
    }
    public void Drop()
    {
        Vector3 offset = new Vector3(0, yoffsetAmount, 0);

        if (item != null)
        Instantiate(item, transform.position + offset, transform.rotation);
    }
}
