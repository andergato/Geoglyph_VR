using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Minimal global flag store. Your quest system (or anything else) calls
/// QuestFlags.Set("quest_wolves_done") and dialogue conditions react to it.
/// Swap this for your real quest/save system later; only FlagCondition talks to it.
/// </summary>
public static class QuestFlags
{
    static readonly HashSet<string> flags = new HashSet<string>();

    public static event Action<string> OnFlagSet;

    public static bool Has(string id) => !string.IsNullOrEmpty(id) && flags.Contains(id);

    public static void Set(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        if (flags.Add(id)) OnFlagSet?.Invoke(id);
    }

    public static void Clear(string id) => flags.Remove(id);

    public static void ClearAll() => flags.Clear();

    // Static state survives between play sessions when "Enter Play Mode Options" disables domain reload.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        flags.Clear();
        OnFlagSet = null;
    }
}
