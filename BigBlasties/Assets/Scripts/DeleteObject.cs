using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    [SerializeField] float destroyTime;
    float destroyTimer;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        destroyTimer += Time.deltaTime;

        if (destroyTimer >= destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
