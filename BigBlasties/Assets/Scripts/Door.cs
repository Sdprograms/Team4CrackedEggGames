using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public float interDistance;

    public GameObject InterText;

    public string doorOpen;
    public string doorClose;

    public void Interact()
    {
        
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interDistance)) 
        {
            if (hit.collider.gameObject.tag == "door") 
            {
                GameObject Parent = hit.collider.transform.root.gameObject;
                Animator doorAnim = Parent.GetComponent<Animator>();
                InterText.SetActive(true);

                if(Input.GetKeyDown(KeyCode.E)) 
                {
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorOpen)) 
                    {
                        doorAnim.ResetTrigger("open");
                        doorAnim.SetTrigger("close");
                    }
                    if (doorAnim.GetCurrentAnimatorStateInfo(0).IsName(doorClose))
                    {
                        doorAnim.ResetTrigger("close");
                        doorAnim.SetTrigger("open");
                    }
                }
                else 
                {
                    InterText.SetActive(false);
                }
            }
            else
            {
                InterText.SetActive(false);
            }
        }
    }

}
