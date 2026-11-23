using System.Collections;
using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5;

    private int currentHealth;
    private bool isDead = false;

    [Header("Damage Number Settings")]
    public GameObject damageNumberPrefab;
    public Vector3 damageNumberOffset = new Vector3(0f, 1.5f, 0f);

    [Header("Respawn Settings")]
    public float respawnDelay = 2f;

    [Header("Hex Settings")]
    public float hexDuration = 5f;
    public float hexDamageMultiplier = 2f;

    [Header("Hex Visual Effect")]
    public GameObject hexEffectPrefab;

    private bool isHexed = false;
    private Coroutine hexCoroutine;
    private GameObject activeHexEffect;

    private Renderer[] renderers;
    private Collider[] colliders;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    void Awake()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        currentHealth = maxHealth;
    }

    // DAMAGE
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        int actualDamage = damage;

        if (isHexed)
        {
            actualDamage = Mathf.CeilToInt(
            damage * hexDamageMultiplier
            );
        }

        currentHealth -= actualDamage;

        SpawnDamageNumber(actualDamage);

        Debug.Log("Dummy health: " + currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(DieAndRespawn());
        }
    }

    // HEAL
    public void Heal(int amount)
    {
        if (isDead)
            return;

        currentHealth += amount;

        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log("Dummy healed! Health: " + currentHealth);
    }

    // HEX
    public void ApplyHex()
    {
        if (isDead)
            return;

        // Refresh the curse duration.
        if (hexCoroutine != null)
        {
            StopCoroutine(hexCoroutine);
        }

        // Create the visual effect if it doesn't exist.
        if (activeHexEffect == null && hexEffectPrefab != null)
        {
            activeHexEffect = Instantiate(
            hexEffectPrefab,
            transform
            );

            activeHexEffect.transform.localPosition =
            new Vector3(0f, 1f, 0f);

            activeHexEffect.transform.localRotation =
            Quaternion.identity;
        }

        if (activeHexEffect != null)
        {
            activeHexEffect.SetActive(true);
        }

        hexCoroutine = StartCoroutine(HexEffect());
    }

    private IEnumerator HexEffect()
    {
        isHexed = true;

        Debug.Log("Dummy has been HEXED!");

        yield return new WaitForSeconds(hexDuration);

        isHexed = false;
        hexCoroutine = null;

        if (activeHexEffect != null)
        {
            Destroy(activeHexEffect);
            activeHexEffect = null;
        }

        Debug.Log("Hex has expired!");
    }

    // DAMAGE NUMBERS
    private void SpawnDamageNumber(int amount)
    {
        if (damageNumberPrefab == null)
            return;

        Vector3 spawnPosition =
        transform.position + damageNumberOffset;

        GameObject number = Instantiate(
        damageNumberPrefab,
        spawnPosition,
        Quaternion.identity
        );

        DamageNumber damageNumber =
        number.GetComponent<DamageNumber>();

        if (damageNumber != null)
        {
            damageNumber.SetDamage(amount);
        }
    }

    // DEATH AND RESPAWN
    private IEnumerator DieAndRespawn()
    {
        isDead = true;

        // Remove Hex when the dummy dies.
        if (hexCoroutine != null)
        {
            StopCoroutine(hexCoroutine);
            hexCoroutine = null;
        }

        isHexed = false;

        if (activeHexEffect != null)
        {
            Destroy(activeHexEffect);
            activeHexEffect = null;
        }

        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        Debug.Log("Dummy defeated!");

        yield return new WaitForSecondsRealtime(respawnDelay);

        transform.position = startingPosition;
        transform.rotation = startingRotation;

        currentHealth = maxHealth;

        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        isDead = false;

        Debug.Log("Dummy respawned!");
    }

    // GETTERS
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool IsHexed()
    {
        return isHexed;
    }
}


