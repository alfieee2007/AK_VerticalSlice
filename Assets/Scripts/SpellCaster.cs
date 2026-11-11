using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCaster : MonoBehaviour
{
    [Header("Spell Settings")]
    public GameObject boltPrefab;
    public Transform castPoint;

    [Header("Targeting")]
    public float targetingRange = 30f;

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
        DummyTarget nearestDummy = null;

        float nearestDistance = targetingRange;

        DummyTarget[] dummies =
        FindObjectsByType<DummyTarget>(
        FindObjectsSortMode.None
        );

        foreach (DummyTarget dummy in dummies)
        {
            float distance = Vector3.Distance(
            transform.position,
            dummy.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestDummy = dummy;
            }
        }

        if (nearestDummy == null)
        {
            Debug.Log("No dummy in range!");
            return;
        }

        GameObject bolt = Instantiate(
        boltPrefab,
        castPoint.position,
        Quaternion.identity
        );

        SpellBolt spell =
        bolt.GetComponent<SpellBolt>();

        spell.SetTarget(nearestDummy.transform);
    }
}
