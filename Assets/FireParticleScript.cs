using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticleScript : MonoBehaviour
{
    public GameObject fire;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireToggle()
    {
        if(isActive==false)
        { 
            isActive = true;
            fire.SetActive(true);
        }
        else
        {
            isActive = false;
            fire.SetActive(false);
        }
    }
}
