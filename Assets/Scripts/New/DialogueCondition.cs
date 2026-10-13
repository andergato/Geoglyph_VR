using UnityEngine;

/// <summary>
/// Base class for anything that can gate a dialogue variant.
/// Make new conditions by inheriting from this (item owned, reputation >= X, time of day, etc.)
/// and adding a [CreateAssetMenu].
/// </summary>
public abstract class DialogueCondition : ScriptableObject
{
    public abstract bool IsMet();
}
