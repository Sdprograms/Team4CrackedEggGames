using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] string level;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        ChangeScene();
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(level);
    }
}
