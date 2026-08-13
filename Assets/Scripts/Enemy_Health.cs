using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int expReward = 3;
    public delegate void MonsterDefeated(int exp);
    public static event MonsterDefeated onMonsterDefeated;
    public int currentHealth;
    public int maxHealth;

    private void start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        else if(currentHealth <= 0)
        {
            onMonsterDefeated(expReward);
            Destroy(gameObject);
        }
    }
}
