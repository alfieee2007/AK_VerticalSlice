using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float floatSpeed = 1.5f;
    public float lifetime = 1f;

    private TMP_Text damageText;
    private Transform cameraTransform;

    void Awake()
    {
        damageText = GetComponent<TMP_Text>();

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        Destroy(gameObject, lifetime);
    }

    public void SetDamage(int amount)
    {
        if (damageText != null)
        {
            damageText.text = amount.ToString();
        }
    }

    void Update()
    {
        transform.position +=
        Vector3.up * floatSpeed * Time.deltaTime;

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (cameraTransform != null)
        {
            transform.rotation = Quaternion.LookRotation(
            transform.position - cameraTransform.position
            );
        }
    }
}

