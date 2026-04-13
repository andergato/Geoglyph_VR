using UnityEngine;
using UltimateXR.Manipulation;

public class PickupUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private UxrGrabbableObject grabbable;

    private void Awake()
    {
        grabbable = GetComponent<UxrGrabbableObject>();
        panel.SetActive(false);
    }

    private void OnEnable()
    {
        if (grabbable != null)
        {
            grabbable.Grabbed += OnGrabbed;
            grabbable.Released += OnReleased;
        }
    }

    private void OnDisable()
    {
        if (grabbable != null)
        {
            grabbable.Grabbed -= OnGrabbed;
            grabbable.Released -= OnReleased;
        }
    }

    private void OnGrabbed(object sender, UxrManipulationEventArgs e)
    {
        panel.SetActive(true);
    }

    private void OnReleased(object sender, UxrManipulationEventArgs e)
    {
        panel.SetActive(false);
    }
}