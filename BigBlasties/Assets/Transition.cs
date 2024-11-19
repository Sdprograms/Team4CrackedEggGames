using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{

    [SerializeField] Animator transitionAnime;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player") 
        {
            StartCoroutine(LoadLevel());
        }
    }

    IEnumerator LoadLevel() 
    {
        transitionAnime.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
