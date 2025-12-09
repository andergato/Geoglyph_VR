using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnScript : MonoBehaviour
{
    public GameObject Grab;
    private Vector3 grabPosition;
    // Start is called before the first frame update
    void Start()
    {
        grabPosition = Grab.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Collider exited: " + other.name);
        Grab.transform.localPosition = grabPosition;
    }
}
