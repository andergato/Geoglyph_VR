using UnityEngine;
using UltimateXR.Manipulation;

public class PickupHandler : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private UxrGrabbableObject grabbable;

    private void Awake()
    {
        grabbable = GetComponent<UxrGrabbableObject>();
       if(panel) panel.SetActive(false);
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
        if (panel) panel.SetActive(true);
        SetLayer(gameObject, 6);
    }

    private void OnReleased(object sender, UxrManipulationEventArgs e)
    {
        if (panel) panel.SetActive(false);
        SetLayer(gameObject, 3);
    }

    // Recursively changes child layers of gameObject to new layer
    private void SetLayer(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayer(child.gameObject, newLayer);
        }
    }

}