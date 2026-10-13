/// <summary>
/// Anything the VR ray can "click". Your existing ray code just needs to call
/// hit.collider.GetComponentInParent<IInteractable>()?.Interact();
/// </summary>
public interface IInteractable
{
    void Interact();
}
