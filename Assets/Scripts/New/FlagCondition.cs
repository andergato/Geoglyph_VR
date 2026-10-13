using UnityEngine;

/// <summary>Met when a QuestFlags flag is set (or NOT set, if inverted).</summary>
[CreateAssetMenu(menuName = "Dialogue/Conditions/Flag Condition")]
public class FlagCondition : DialogueCondition
{
    public string flagId;
    [Tooltip("If true, the condition is met when the flag is NOT set.")]
    public bool invert;

    public override bool IsMet() => QuestFlags.Has(flagId) != invert;
}
