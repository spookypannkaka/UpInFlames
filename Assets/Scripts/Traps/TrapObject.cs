using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObject : MonoBehaviour, ITrap
{
    public TrapBehavior behavior;
    private Animator animator;
    private bool isActive = true; // Tracks if the trap is currently active

    private void Start()
    {
        // Dynamically assign behavior if none is set
        if (behavior == null)
        {
            AssignBehaviorFromPrefabName();
        }
        animator = GetComponent<Animator>();
    }

    private void AssignBehaviorFromPrefabName()
    {
        // Use the prefab's name to fetch a matching behavior
        string prefabName = gameObject.name.Replace("(Clone)", "").Trim(); // Remove "(Clone)" from instantiated prefab names
        behavior = TrapBehaviorManager.GetBehaviorByName(prefabName);

        if (behavior != null)
        {
            Debug.Log($"Assigned behavior '{behavior.trapName}' to trap '{prefabName}'.");
        }
        else
        {
            Debug.LogWarning($"No matching behavior found for trap '{prefabName}'.");
        }
    }

    public void TriggerAnimation(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning($"Animator not found on {gameObject.name}!");
        }
    }

    public bool UseTrap(GameObject player)
    {
        if (!isActive) return false;

        if (behavior != null)
        {
            bool activated = behavior.UseTrap(player, this);
            if (activated)
            {
                StartCooldown(2.0f);
            }
            return activated;
        }
        Debug.LogWarning("No behavior assigned to this trap.");
        return false;
    }

    private void StartCooldown(float duration)
    {
        StartCoroutine(CooldownCoroutine(duration));
    }

    private IEnumerator CooldownCoroutine(float duration) {
        isActive = false;

        yield return new WaitForSeconds(duration);

        isActive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            UseTrap(collision.gameObject);
        }
    }
}
