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

    [Header("Sound Effects")]
    public AudioClip impactSound;
    [Range(0f, 1f)]
    public float impactVolume = 1f;

    private bool hasHit = false;

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

        Vector3 direction =
        (target.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(
        transform.position,
        target.position,
        speed * Time.deltaTime
        );

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        DummyTarget dummy =
        other.GetComponentInParent<DummyTarget>();

        if (dummy == null)
            return;

        hasHit = true;

        // Deal damage
        dummy.TakeDamage(damage);

        // Play impact sound
        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(
            impactSound,
            transform.position,
            impactVolume
            );
        }

        // Spawn impact particle effect
        if (impactPrefab != null)
        {
            GameObject effect = Instantiate(
            impactPrefab,
            transform.position,
            Quaternion.identity
            );

            Destroy(effect, impactEffectLifetime);
        }

        // Start hit-stop
        StartCoroutine(ImpactSequence());
    }

    private IEnumerator ImpactSequence()
    {
        float previousTimeScale = Time.timeScale;

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(hitStopDuration);

        Time.timeScale = previousTimeScale;

        Destroy(gameObject);
    }
}



