using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Core.Components.Composite;
using UltimateXR.Manipulation;

using UltimateXR.Avatar;
using UltimateXR.Devices;
using UltimateXR.Core;

public class BlowdartScript : MonoBehaviour
{
    private GameObject currentDart;
    public GameObject dartLoc;
    public bool mouth = false;
    public bool loaded = false;
    public UxrGrabbableObject blowgun;
    private bool grabbed = false;

    public float velocityMult = 10f;

     bool LTwasPressed = false;
     bool RTwasPressed = false;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grabbed = UxrGrabManager.Instance.IsBeingGrabbed(blowgun);

        if(grabbed == true)
        {
            Debug.Log("grabbed");
            LTwasPressed = UxrAvatar.LocalAvatarInput.GetButtonsPressDown(UxrHandSide.Left, UxrInputButtons.Trigger);
            RTwasPressed = UxrAvatar.LocalAvatarInput.GetButtonsPressDown(UxrHandSide.Right, UxrInputButtons.Trigger);

             if(LTwasPressed == true || RTwasPressed == true)
             {
                 Debug.Log("Ayo It worked LETS GOOOOO");
                 FireDart();
             }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Blowdart"))
        {
        
            
       
         //Debug.Log("okay so this works");
         currentDart = other.gameObject;
        currentDart.transform.position = dartLoc.transform.localPosition;
        currentDart.transform.rotation = dartLoc.transform.rotation;
        currentDart.GetComponent<Rigidbody>().isKinematic = true;
        currentDart.transform.position = dartLoc.transform.position;
        currentDart.transform.SetParent(dartLoc.transform, true);
        loaded = true;
    
        }
        if(other.CompareTag("PlayerHead"))
        {
            mouth = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PlayerHead"))
        {
            mouth = false;
        }
    }
   /* private void OnButtonPressed(UxrControllerHand hand, UxrInputButtons buttons)
    {
        if(loaded==true && buttons == UxrInputButtons.Trigger)
        {
            if (currentDart != null)
        {
            Rigidbody projF = currentDart.GetComponent<Rigidbody>();
             projF.isKinematic = false;                                      
             projF.collisionDetectionMode = CollisionDetectionMode.Continuous;
             projF.velocity = Grab.transform.forward * velocityMult;
             currentDart.transform.SetParent(null);
s
             currentDart = null;
        }
        }
    }*/
    private void OnGrabbed(object sender, UxrManipulationEventArgs e)
    {
        grabbed = true; 
        Debug.Log("OnGrabbed");
    }
  private void OnReleased(object sender, UxrManipulationEventArgs e)
  {
    grabbed = false;
  }

  private void FireDart()

  {     
    if(currentDart != null)
    {
        Rigidbody projF = currentDart.GetComponent<Rigidbody>();
             projF.isKinematic = false;                                      
             projF.collisionDetectionMode = CollisionDetectionMode.Continuous;
             projF.velocity = blowgun.transform.forward * velocityMult;
             currentDart.transform.SetParent(null);

             currentDart = null;
    }
        
  }
    
    
}
