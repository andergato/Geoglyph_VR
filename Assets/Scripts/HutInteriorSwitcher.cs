using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Avatar;

public class HutInteriorSwitcher : MonoBehaviour
{
    [Header("Shared Exterior")]
    [SerializeField] private GameObject outsideContent;

    [Header("This Hut Interior")]
    [SerializeField] private GameObject thisInterior;

    [Header("Optional Other Interiors To Turn Off")]
    [SerializeField] private GameObject[] otherInteriorsToDisable;

    [Header("Player Tag (optional fallback)")]
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
        Debug.Log("Entered trigger: " + other.name + " | Tag: " + other.tag);

        if (!IsPlayer(other))
            return;

        Debug.Log("Player entered hut");
        EnterHut();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited trigger: " + other.name + " | Tag: " + other.tag);

        if (!IsPlayer(other))
            return;

        Debug.Log("Player exited hut");
        ExitHut();
    }

    private bool IsPlayer(Collider other)
    {
        // Works for Ultimate XR avatars OR normal tagged player
        return other.CompareTag(playerTag) || other.GetComponentInParent<UxrAvatar>() != null;
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