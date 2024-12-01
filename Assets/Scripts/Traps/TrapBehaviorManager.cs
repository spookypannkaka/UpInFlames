using System.Collections.Generic;
using UnityEngine;

public static class TrapBehaviorManager
{
    private static Dictionary<string, TrapBehavior> behaviorDictionary = new Dictionary<string, TrapBehavior>();

    // Load all TrapBehaviors from the Resources folder
    public static void Initialize()
    {
        TrapBehavior[] behaviors = Resources.LoadAll<TrapBehavior>("");

        foreach (var behavior in behaviors)
        {
            if (!behaviorDictionary.ContainsKey(behavior.name))
            {
                behaviorDictionary.Add(behavior.name, behavior);
            }
        }
    }

    // Fetch a behavior by name
    public static TrapBehavior GetBehaviorByName(string name)
    {
        if (behaviorDictionary.TryGetValue(name, out TrapBehavior behavior))
        {
            return behavior;
        }

        Debug.LogWarning($"TrapBehavior with name '{name}' not found.");
        return null;
    }
}
