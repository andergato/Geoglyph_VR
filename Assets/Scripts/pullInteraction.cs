using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Core.Components.Composite;
using UltimateXR.Manipulation;

public class pullInteraction : MonoBehaviour
{
    public UxrGrabbableObject Grab;
    public LineRenderer lineRenderer;
    private Vector3 grabPosition;
    public GameObject projectile;
    public GameObject Arrow;

    public GameObject ArrowSt;

    private Vector3 launchPos;

    public float velocityMult = 10f;
    private int numArrows = 0;
    private float distance = 0;

    //private bool loaded;

    private GameObject currentArrow;
    // Start is called before the first frame update
    void Start()
    {
        grabPosition = Grab.transform.localPosition;


        Grab.Grabbed += OnGrabbed;
        Grab.Released += OnReleased;
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer != null && Grab != null)
        {
            
            lineRenderer.SetPosition(1, Grab.transform.localPosition);
            distance = Vector3.Distance (grabPosition, Grab.transform.localPosition);

     
        }
        else
        {
            Debug.LogError("something aint right");
        }
    }

    private void OnGrabbed(object sender, UxrManipulationEventArgs e)
    {
        numArrows++;
        //projectile = Instantiate(Arrow) as GameObject;
        //projectile.name = "Arrow" + numArrows;
        
        
        //projectile.transform.position = ArrowSt.transform.localPosition;
       // projectile.transform.rotation = ArrowSt.transform.rotation;
        //projectile.GetComponent<Rigidbody>().isKinematic = true;
        //projectile.transform.position = ArrowSt.transform.position;
       // projectile.transform.SetParent(ArrowSt.transform, true);
        

        
    }

    private void OnReleased(object sender, UxrManipulationEventArgs e)
    {
        // Return the object to its original position and rotation when released

        Grab.transform.localPosition = grabPosition;
        
        if (currentArrow != null)
        {
            Rigidbody projF = currentArrow.GetComponent<Rigidbody>();
             projF.isKinematic = false;                                      
             projF.collisionDetectionMode = CollisionDetectionMode.Continuous;
             projF.velocity = Grab.transform.forward * (distance * velocityMult);
             currentArrow.transform.SetParent(null);

             currentArrow = null;
        }
            
             
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
        currentArrow = other.gameObject;
        currentArrow.transform.position = ArrowSt.transform.localPosition;
        currentArrow.transform.rotation = ArrowSt.transform.rotation;
        currentArrow.GetComponent<Rigidbody>().isKinematic = true;
        currentArrow.transform.position = ArrowSt.transform.position;
        currentArrow.transform.SetParent(ArrowSt.transform, true);
        }
    }
}
