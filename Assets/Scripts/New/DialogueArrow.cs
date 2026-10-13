using UnityEngine;

/// <summary>
/// Optional alternative to the UI Button: put this on a 3D/sprite arrow with a Collider
/// and your existing ray's IInteractable call will advance the dialogue.
/// Only use this OR the Button on DialogueUI, not both on the same arrow.
/// </summary>
[RequireComponent(typeof(Collider))]
public class DialogueArrow : MonoBehaviour, IInteractable
{
    [SerializeField] DialogueUI ui;

    public void Interact()
    {
        if (ui) ui.OnNextPressed();
    }
}
