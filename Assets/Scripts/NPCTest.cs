using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTest : MonoBehaviour
{
    public bool i = false;
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f; 
    public float minDistance = 2f; 
    public float maxDistance = 5f; 
    public List<Transform> areasToGo;
    //public List<String> animationsToGo;

    private Vector3 currentTarget; 
    

    void Start()
    {
        
        currentTarget = pointA.position;
        //SetRandomPointB(); 

      
        
    }

    void Update()
    {
        
        
        if(i==true)
        {

       Vector3 relativePos = currentTarget - transform.position;

       Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
       transform.rotation = rotation;


        
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            // If the current target was point A, switch to point B, and vice versa
            if (currentTarget == pointA.position)
            {
                //SetRandomPointB();
                currentTarget = pointB.position;
                Debug.Log("Current Target is B");
                
            }
            else
            {
                currentTarget = pointA.position;
                Debug.Log("Current Target is A");
                
            }
        }
         }
    }

    void SetRandomPointB()
    {
        
        float randomDistance = Random.Range(minDistance, maxDistance);
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        currentTarget = pointA.position + randomDirection * randomDistance;
        
    }

    public void toggleAvatar()
    {
        if(i==true)
        {
            i=false;
        }
        else
        i=true;
    }
}