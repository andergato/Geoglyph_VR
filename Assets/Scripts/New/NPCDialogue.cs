using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Put on the NPC (needs a Collider for the ray to hit). Picks which dialogue variant is
/// active, shows the "!" marker when there's something new to hear, and starts the conversation
/// when the ray interacts with it.
/// </summary>
public class NPCDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] DialogueSet dialogueSet;

    [Tooltip("Where the dialogue panel appears (e.g. an empty child beside/above the NPC). Defaults to this transform.")]
    [SerializeField] Transform uiAnchor;

    [SerializeField] NPCMarker marker;

    [Tooltip("Conditions are also re-checked on this interval so non-flag conditions work too.")]
    [SerializeField] float refreshInterval = 0.5f;

    [Tooltip("Fires with the variant index when the player finishes a conversation.")]
    public UnityEvent<int> onDialogueFinished;

    int currentVariant = -1;
    int lastHeardVariant = -1;
    bool inConversation;
    float nextRefresh;

    public string SpeakerName => dialogueSet ? dialogueSet.speakerName : name;
    public Transform UIAnchor => uiAnchor ? uiAnchor : transform;
    public bool HasNewDialogue => currentVariant >= 0
                                  && currentVariant != lastHeardVariant
                                  && dialogueSet.variants[currentVariant].showMarker;

    void OnEnable()
    {
        QuestFlags.OnFlagSet += HandleFlagSet;
        Refresh();
    }

    void OnDisable()
    {
        QuestFlags.OnFlagSet -= HandleFlagSet;
    }

    void Update()
    {
        if (Time.time < nextRefresh) return;
        nextRefresh = Time.time + refreshInterval;
        Refresh();
    }

    void HandleFlagSet(string _) => Refresh();

    /// <summary>Re-evaluates which variant is active and updates the "!" marker.</summary>
    public void Refresh()
    {
        currentVariant = dialogueSet ? dialogueSet.GetActiveVariantIndex() : -1;
        if (marker) marker.SetVisible(HasNewDialogue && !inConversation);
    }

    // Called by NPCClickArea (the invisible button over the NPC), or any other script.
    public void Interact()
    {
        var manager = DialogueManager.Instance;
        if (!dialogueSet || manager == null || manager.IsActive) return;

        Refresh();
        if (currentVariant < 0) return;

        var lines = dialogueSet.variants[currentVariant].lines;
        if (lines == null || lines.Length == 0) return;

        inConversation = true;
        if (marker) marker.SetVisible(false);

        // Lines are snapshotted here, so a condition changing mid-conversation can't swap text under the player.
        manager.Begin(this, currentVariant, lines);
    }

    // Called by DialogueManager when the player reaches the last line and closes it.
    public void NotifyFinished(int variantIndex)
    {
        inConversation = false;
        lastHeardVariant = variantIndex;

        string flag = dialogueSet.variants[variantIndex].setFlagOnFinish;
        if (!string.IsNullOrEmpty(flag)) QuestFlags.Set(flag);

        onDialogueFinished?.Invoke(variantIndex);
        Refresh();
    }

    // Called by DialogueManager if the player walks away mid-conversation (the "!" comes back).
    public void NotifyCancelled()
    {
        inConversation = false;
        Refresh();
    }
}
