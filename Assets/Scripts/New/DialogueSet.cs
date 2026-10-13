using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueVariant
{
    [Tooltip("Editor label only, e.g. 'Before quest', 'After quest'.")]
    public string label;

    [Tooltip("ALL of these must be met for the variant to be available. Empty = always available.")]
    public DialogueCondition[] conditions;

    [TextArea(2, 5)]
    public string[] lines;

    [Tooltip("Show the '!' over the NPC when this variant becomes active and hasn't been heard yet. Turn off for reminders/idle chatter.")]
    public bool showMarker = true;

    [Tooltip("Optional: QuestFlags flag to set once the player reaches the end of this variant (e.g. 'quest_wolves_accepted').")]
    public string setFlagOnFinish;

    public bool IsAvailable()
    {
        if (conditions == null) return true;
        foreach (var c in conditions)
            if (c != null && !c.IsMet()) return false;
        return true;
    }
}

/// <summary>
/// All the dialogue an NPC can say. Order variants from lowest to highest priority:
/// the LAST variant whose conditions are met is the one used.
/// Put the default/greeting first with no conditions, quest-complete variants after it.
/// </summary>
[CreateAssetMenu(menuName = "Dialogue/Dialogue Set")]
public class DialogueSet : ScriptableObject
{
    public string speakerName = "NPC";
    public List<DialogueVariant> variants = new List<DialogueVariant>();

    /// <returns>Index of the active variant, or -1 if none are available.</returns>
    public int GetActiveVariantIndex()
    {
        for (int i = variants.Count - 1; i >= 0; i--)
            if (variants[i].IsAvailable()) return i;
        return -1;
    }
}

