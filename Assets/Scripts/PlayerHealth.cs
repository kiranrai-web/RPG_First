using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public TMP_Text healthText;

    private void Start()
    {
        healthText.text = "HP: " + StatsManager.Instance.currentHealth + "/ " + StatsManager.Instance.maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.currentHealth += amount;
        healthText.text = "HP: " + StatsManager.Instance.currentHealth + "/ " + StatsManager.Instance.maxHealth;

        if (StatsManager.Instance.currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }

    }
}
