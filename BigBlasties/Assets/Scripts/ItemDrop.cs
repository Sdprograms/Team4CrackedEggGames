using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] GameObject item;
    public void Drop()
    {
        if(item != null)
        Instantiate(item, transform.position, transform.rotation);
    }
}
