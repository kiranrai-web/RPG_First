using UnityEngine;
using System.Collections;
public class UseItem : MonoBehaviour
{
    public void ApplyItemEffects(ItemSO itemSo)
    {
        if (itemSo.currentHealth > 0)
            StatsManager.Instance.UpdateHealth(itemSo.currentHealth);
        if (itemSo.maxHealth > 0)
            StatsManager.Instance.UpdateMaxHealth(itemSo.maxHealth);
        if (itemSo.speed > 0)
            StatsManager.Instance.UpdateSpeed(itemSo.speed);
        if (itemSo.duration > 0)
            StartCoroutine(EffectTimer(itemSo, itemSo.duration));
    }

    private IEnumerator EffectTimer(ItemSO itemSo, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (itemSo.currentHealth > 0)
            StatsManager.Instance.UpdateHealth(-itemSo.currentHealth);
        if (itemSo.maxHealth > 0)
            StatsManager.Instance.UpdateMaxHealth(-itemSo.maxHealth);
        if (itemSo.speed > 0)
            StatsManager.Instance.UpdateSpeed(-itemSo.speed);
    }

}