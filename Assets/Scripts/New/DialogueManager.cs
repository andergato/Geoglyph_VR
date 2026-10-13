using System;
using UnityEngine;

/// <summary>
/// Scene-level conversation state machine. One per scene.
/// Knows nothing about UI; DialogueUI (or anything else) listens to its events.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Optional: auto-cancel when the player walks away")]
    [SerializeField] Transform player;      // usually the XR camera or rig
    [SerializeField] float maxDistance = 0f; // 0 = disabled

    public event Action<NPCDialogue> Started;
    /// <summary>(line text, isLastLine)</summary>
    public event Action<string, bool> LineShown;
    public event Action Ended;

    NPCDialogue npc;
    string[] lines;
    int variantIndex;
    int lineIndex;

    public bool IsActive => npc != null;
    public NPCDialogue CurrentNPC => npc;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Update()
    {
        if (!IsActive || !player || maxDistance <= 0f) return;
        if (Vector3.Distance(player.position, npc.transform.position) > maxDistance)
            End(completed: false);
    }

    public void Begin(NPCDialogue speaker, int variant, string[] dialogueLines)
    {
        if (IsActive || dialogueLines == null || dialogueLines.Length == 0) return;

        npc = speaker;
        variantIndex = variant;
        lines = dialogueLines;
        lineIndex = 0;

        Started?.Invoke(npc);
        ShowCurrentLine();
    }

    /// <summary>Next line, or close the conversation if we're on the last one.</summary>
    public void Advance()
    {
        if (!IsActive) return;

        lineIndex++;
        if (lineIndex >= lines.Length) End(completed: true);
        else ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        LineShown?.Invoke(lines[lineIndex], lineIndex == lines.Length - 1);
    }

    void End(bool completed)
    {
        var speaker = npc;
        int variant = variantIndex;

        npc = null;
        lines = null;

        Ended?.Invoke();

        if (completed) speaker.NotifyFinished(variant);
        else speaker.NotifyCancelled();
    }
}
