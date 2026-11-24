using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCaster : MonoBehaviour
{
    [Header("Magic Bolt Settings")]
    public GameObject boltPrefab;
    public Transform castPoint;
    public float targetingRange = 30f;

    [Header("Magic Bolt Sound")]
    public AudioClip castSound;
    [Range(0f, 1f)]
    public float castVolume = 1f;

    [Header("Heal Settings")]
    public int healAmount = 2;
    public float healRange = 30f;

    [Header("Hex Settings")]
    public float hexRange = 30f;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // Magic Bolt
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            CastSpell();
        }

        // Heal
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            HealDummy();
        }

        // Hex
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            CastHex();
        }
    }

    DummyTarget FindNearestTarget(float range)
    {
        DummyTarget[] targets =
        FindObjectsByType<DummyTarget>(FindObjectsSortMode.None);

        DummyTarget nearestTarget = null;
        float nearestDistance = range;

        foreach (DummyTarget target in targets)
        {
            if (target == null)
                continue;

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

        return nearestTarget;
    }

    // MAGIC BOLT
    void CastSpell()
    {
        if (boltPrefab == null || castPoint == null)
        {
            Debug.LogWarning("SpellCaster: Missing Bolt Prefab or Cast Point!");
            return;
        }

        DummyTarget target = FindNearestTarget(targetingRange);

        if (target == null)
        {
            Debug.Log("No dummy within targeting range.");
            return;
        }

        GameObject bolt = Instantiate(
        boltPrefab,
        castPoint.position,
        castPoint.rotation
        );

        SpellBolt spellBolt = bolt.GetComponent<SpellBolt>();

        if (spellBolt != null)
        {
            spellBolt.SetTarget(target.transform);
        }

        if (castSound != null)
        {
            AudioSource.PlayClipAtPoint(
            castSound,
            castPoint.position,
            castVolume
            );
        }

        Debug.Log("Magic Bolt cast!");
    }

    // HEAL
    void HealDummy()
    {
        DummyTarget target = FindNearestTarget(healRange);

        if (target != null)
        {
            target.Heal(healAmount);
            Debug.Log("Heal spell cast!");
        }
        else
        {
            Debug.Log("No dummy in healing range!");
        }
    }

    // HEX
    void CastHex()
    {
        DummyTarget target = FindNearestTarget(hexRange);

        if (target != null)
        {
            target.ApplyHex();
            Debug.Log("Hex spell cast!");
        }
        else
        {
            Debug.Log("No dummy in Hex range!");
        }
    }
}

