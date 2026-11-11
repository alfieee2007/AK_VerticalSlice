using UnityEngine;
using System.Collections;

public class DummyTarget : MonoBehaviour
{
    [Header("Health")]
    public int health = 100;

    [Header("Damage Flash")]
    public Color damageColour = Color.red;
    public float flashDuration = 0.5f;

    private Renderer[] dummyRenderers;
    private MaterialPropertyBlock propertyBlock;
    private Color[][] originalColours;

    private Coroutine flashRoutine;

    void Awake()
    {
        dummyRenderers = GetComponentsInChildren<Renderer>();

        propertyBlock = new MaterialPropertyBlock();

        originalColours = new Color[dummyRenderers.Length][];

        for (int i = 0; i < dummyRenderers.Length; i++)
        {
            Material[] materials = dummyRenderers[i].sharedMaterials;

            originalColours[i] = new Color[materials.Length];

            for (int j = 0; j < materials.Length; j++)
            {
                if (materials[j].HasProperty("_BaseColor"))
                {
                    originalColours[i][j] =
                    materials[j].GetColor("_BaseColor");
                }
                else
                {
                    originalColours[i][j] = Color.white;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Dummy hit! Health: " + health);

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        // Flash bright red
        SetDummyColour(damageColour);

        // Keep red visible during impact freeze
        yield return new WaitForSecondsRealtime(flashDuration);

        // Restore original colour
        RestoreOriginalColour();

        flashRoutine = null;
    }

    void SetDummyColour(Color colour)
    {
        for (int i = 0; i < dummyRenderers.Length; i++)
        {
            Material[] materials = dummyRenderers[i].sharedMaterials;

            for (int j = 0; j < materials.Length; j++)
            {
                dummyRenderers[i].GetPropertyBlock(propertyBlock, j);

                propertyBlock.SetColor("_BaseColor", colour);

                dummyRenderers[i].SetPropertyBlock(propertyBlock, j);
            }
        }
    }

    void RestoreOriginalColour()
    {
        for (int i = 0; i < dummyRenderers.Length; i++)
        {
            Material[] materials = dummyRenderers[i].sharedMaterials;

            for (int j = 0; j < materials.Length; j++)
            {
                dummyRenderers[i].GetPropertyBlock(propertyBlock, j);

                propertyBlock.SetColor(
                "_BaseColor",
                originalColours[i][j]
                );

                dummyRenderers[i].SetPropertyBlock(propertyBlock, j);
            }
        }
    }
}


