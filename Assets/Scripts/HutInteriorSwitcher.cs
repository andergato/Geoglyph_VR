using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutInteriorSwitcher : MonoBehaviour
{
    [Header("Shared Exterior")]
    [SerializeField] private GameObject outsideContent;

    [Header("This Hut Interior")]
    [SerializeField] private GameObject thisInterior;

    [Header("Optional Other Interiors To Turn Off")]
    [SerializeField] private GameObject[] otherInteriorsToDisable;

    [Header("Player Tag")]
    [SerializeField] private string playerTag = "Player";

    private void Start()
    {
        if (thisInterior != null)
        {
            thisInterior.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        EnterHut();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        ExitHut();
    }

    private void EnterHut()
    {
        if (outsideContent != null)
            outsideContent.SetActive(false);

        if (thisInterior != null)
            thisInterior.SetActive(true);

        if (otherInteriorsToDisable != null)
        {
            foreach (GameObject interior in otherInteriorsToDisable)
            {
                if (interior != null && interior != thisInterior)
                    interior.SetActive(false);
            }
        }
    }

    private void ExitHut()
    {
        if (outsideContent != null)
            outsideContent.SetActive(true);

        if (thisInterior != null)
            thisInterior.SetActive(false);
    }
}
