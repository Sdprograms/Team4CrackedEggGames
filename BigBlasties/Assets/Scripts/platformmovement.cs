using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformmovement : MonoBehaviour
{

    [SerializeField] private platformpath _platformpath;

    [SerializeField] private float speed;

    private int targetWPI;

    private Transform previous;
    private Transform target;

    private float time;
    private float duration;

    // Start is called before the first frame update
    void Start()
    {
        nextWPI();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        duration += Time.deltaTime;

        float percentage = duration / time;
        percentage = Mathf.SmoothStep(0, 1, percentage);
        transform.position = Vector3.Lerp(previous.position, target.position, percentage);
        transform.rotation = Quaternion.Lerp(previous.rotation, target.rotation, percentage);

        if (percentage >= 1) 
        {
            nextWPI();
        }
    }

    private void nextWPI() 
    {
        previous = _platformpath.GetWP(targetWPI);
        targetWPI = _platformpath.GetNextWPI(targetWPI);
        target = _platformpath.GetWP(targetWPI);

        duration = 0;

        float distance = Vector3.Distance(previous.position, target.position);
        time = distance / speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
