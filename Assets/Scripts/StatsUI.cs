using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas;

    private bool statsOpen = false;
    public void Start()
    {
        UpdateAllStats();
    }

    private void Update()
    {
        if (Input.GetButtonDown("ToggleStats"))
            if (statsOpen)
            {
                Time.timeScale = 1;
                statsCanvas.alpha = 0;
                statsCanvas.blocksRaycasts = false;
                statsOpen = false;
            }
            else
            {
                Time.timeScale = 0;
                statsCanvas.alpha = 1;
                statsCanvas.blocksRaycasts = true;
                statsOpen = true;
            }
    }

    private void SetSlotText(int index, string text)
    {
        if (statsSlots == null)
            return;

        if (index < 0 || index >= statsSlots.Length)
            return;

        GameObject slot = statsSlots[index];
        if (slot == null)
            return;

        TMP_Text txt = slot.GetComponentInChildren<TMP_Text>();
        if (txt == null)
            return;

        txt.text = text;
    }

    public void UpdateDamage()
    {
        SetSlotText(0, "Damage: " + StatsManager.Instance.damage);
    }

    public void UpdateSpeed()
    {
        statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.speed;
    }

    public void UpdateMaxHealth()
    {
        SetSlotText(2, "Max Health: " + StatsManager.Instance.maxHealth);
    }

    public void UpdateStun()
    {
        SetSlotText(3, "Stun Time: " + StatsManager.Instance.stunTime);
    }

    public void UpdateStrength()
    {
        SetSlotText(4, "Strength: 20");
    }

    public void UpdateMagic()
    {
        SetSlotText(5, "Magic: 05");
    }

    public void UpdateRange()
    {
        SetSlotText(6, "Range: " + StatsManager.Instance.weaponRange);
    }

    public void UpdateAccuracy()
    {
        SetSlotText(7, "Accuracy: 100");
    }

    public void UpdateEvasion()
    {
        SetSlotText(8, "Evasion: 50");
    }

    public void UpdateHealth()
    {
        SetSlotText(2, "Health: " + StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth);
    }

    public void UpdateCharisma()
    {
        SetSlotText(9, "Charisma: 10");
    }

    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
        UpdateAccuracy();
        UpdateEvasion();
        UpdateMagic();  
        UpdateRange();
        UpdateStun();
        UpdateStrength();
        UpdateMaxHealth();
        UpdateCharisma();
    }
}
