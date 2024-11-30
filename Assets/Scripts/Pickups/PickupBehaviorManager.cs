using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviorManager : Singleton<PickupBehaviorManager>
{
    [Header("Available Pickup Behaviors")]
    public List<PickupBehavior> behaviors; 

    protected override void Awake()
    {
        base.Awake();
    }

    public static PickupBehavior GetRandomBehavior()
    {
        if (Instance == null || Instance.behaviors.Count == 0)
        {
            Debug.LogWarning("No behaviors available in PickupBehaviorManager.");
            return null;
        }

        return Instance.behaviors[Random.Range(0, Instance.behaviors.Count)];
    }
}
