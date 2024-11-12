using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IInteractable 
{
    public void Interact();
}
public class Interact : MonoBehaviour
{
    public Transform Source;

    public float Range;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check to see if E is pressed
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            Ray r = new Ray(Source.position, Source.forward);

            if(Physics.Raycast(r, out RaycastHit Info, Range)) 
            {
                if (Info.collider.gameObject.TryGetComponent(out IInteractable InterObject))
                {
                    InterObject.Interact();
                }
            }
        }
    }
}
