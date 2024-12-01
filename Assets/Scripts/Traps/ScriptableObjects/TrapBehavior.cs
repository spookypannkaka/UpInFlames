using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrapBehavior", menuName = "Traps/Behavior")]
public class TrapBehavior : ScriptableObject
{
    public string trapName;
    public GameObject associatedModel;

    public virtual bool UseTrap(GameObject player)
    {
        Debug.Log($"{trapName} used.");
        return true;
    }
}
