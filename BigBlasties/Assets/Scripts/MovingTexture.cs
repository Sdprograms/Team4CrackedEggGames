using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTexture : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 1.0f;

    private Material material;



    void Start()
    {
        material = GetComponent<Renderer>().material; 
    }



    void Update()
    {

        float offset = Time.time * scrollSpeed; 

        material.SetTextureOffset("_MainTex", new Vector2(offset, 0)); 
    }
}
