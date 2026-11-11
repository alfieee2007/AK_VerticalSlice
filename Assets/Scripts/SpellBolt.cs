
using System.Collections;
using UnityEngine;

public class SpellBolt : MonoBehaviour
{
    [Header("Projectile Settings")]
    public Transform target;
    public float speed = 20f;

    [Header("Impact Settings")]
    public GameObject impactPrefab;
    public float impactEffectLifetime = 2f;
    public float hitStopDuration = 0.08f;
    public int damage = 1;

    private bool hasHit = false;

    // Allows SpellCaster to assign the target
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (hasHit)
            return;

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move the bolt towards its target
        Vector3 direction = (target.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(
        transform.position,
        target.position,
        speed * Time.deltaTime
        );

        // Rotate the bolt towards its target
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        // Find the dummy, including on parent objects
        DummyTarget dummy = other.GetComponentInParent<DummyTarget>();

        if (dummy == null)
            return;

        hasHit = true;

        // Deal damage
        dummy.TakeDamage(damage);

        // Spawn the impact particle effect
        if (impactPrefab != null)
        {
            GameObject effect = Instantiate(
            impactPrefab,
            transform.position,
            Quaternion.identity
            );

            Destroy(effect, impactEffectLifetime);
        }

        // Start hit-stop and destroy the bolt
        StartCoroutine(ImpactSequence());
    }

    private IEnumerator ImpactSequence()
    {
        float previousTimeScale = Time.timeScale;

        // Freeze the game briefly
        Time.timeScale = 0f;

        // Wait using real time, not game time
        yield return new WaitForSecondsRealtime(hitStopDuration);

        // Restore normal game speed
        Time.timeScale = previousTimeScale;

        // Destroy the projectile
        Destroy(gameObject);
    }
}


