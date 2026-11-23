using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DummyHealthUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider healthSlider;
    public TMP_Text healthText;
    public TMP_Text dummyNameText;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth + " HP";
        }
    }
}
