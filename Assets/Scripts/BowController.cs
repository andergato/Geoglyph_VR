using System.Collections;
using System.Collections.Generic;
using UltimateXR.Manipulation;
using UnityEngine;

public class BowController : MonoBehaviour
{
    public GameObject item;
    private GameObject bowString;
    public UxrGrabber grabber;
    // Inside your Bow / Player Aiming Script:
    public ArrowProjection trajectoryPredictor;
    public float arrowSpeed = 25f; // Match the speed your Rigidbody arrow uses when shot
    bool isAiming = false;
    float currentArrowVelocity = 0;
    private void Start()
    {
        bowString = item.transform.GetChild(0).GetChild(0).gameObject;
    }
    void Update()
    {
        if (bowString.GetComponent<PullInteraction>().isNotched())
        {
            currentArrowVelocity = bowString.GetComponent<PullInteraction>().getVelocity();
            isAiming = true;
            // Keep calculating and displaying the line every frame as player rotates the bow
            Debug.Log(currentArrowVelocity.ToString());
            trajectoryPredictor.ShowTrajectory(currentArrowVelocity);
        }
        else  // Released / Shot
        {
            isAiming = false;
            trajectoryPredictor.HideTrajectory();
        }
    }

}
