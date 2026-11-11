using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCaster : MonoBehaviour
{
    [Header("Spell Settings")]
    public GameObject boltPrefab;
    public Transform castPoint;
    public float targetingRange = 30f;

    [Header("Sound Effects")]
    public AudioClip castSound;
    [Range(0f, 1f)]
    public float castVolume = 1f;

    void Update()
    {
        if (Keyboard.current != null &&
        Keyboard.current.eKey.wasPressedThisFrame)
        {
            CastSpell();
        }
    }

    void CastSpell()
    {
        if (boltPrefab == null || castPoint == null)
        {
            Debug.LogWarning("SpellCaster: Missing Bolt Prefab or Cast Point!");
            return;
        }

        DummyTarget nearestTarget = null;
        float nearestDistance = targetingRange;

        DummyTarget[] targets =
        FindObjectsByType<DummyTarget>(FindObjectsSortMode.None);

        foreach (DummyTarget target in targets)
        {
            float distance = Vector3.Distance(
            transform.position,
            target.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }

        if (nearestTarget == null)
        {
            Debug.Log("No dummy within targeting range.");
            return;
        }

        // Spawn the bolt
        GameObject bolt = Instantiate(
        boltPrefab,
        castPoint.position,
        castPoint.rotation
        );

        // Assign target
        SpellBolt spellBolt = bolt.GetComponent<SpellBolt>();

        if (spellBolt != null)
        {
            spellBolt.SetTarget(nearestTarget.transform);
        }

        // Play casting sound
        if (castSound != null)
        {
            AudioSource.PlayClipAtPoint(
            castSound,
            castPoint.position,
            castVolume
            );
        }
    }
}

