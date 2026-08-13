using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class SkillSlot : MonoBehaviour
{
    public List<SkillSlot> prerequisiteSkillSlots = new List<SkillSlot>();
    public SkillSo SkillSo;

    public int currentLevel;
    public bool isUnlocked;

    public Image skillIcon;
    public Button skillButton;
    public TMP_Text skillLevelText;

    public static event Action<SkillSlot> OnAbilityPointSpent;
    public static event Action<SkillSlot> OnSkillMaxed;

    private void OnValidate()
    {
        if(SkillSo != null && skillLevelText != null)
        {
            UpdateUI();
        }
    }

    public void TryUpgradeSkill()
    {
        if (SkillSo == null)
            return;

        if(isUnlocked && currentLevel < SkillSo.maxLevel)
        {
            currentLevel++;
            OnAbilityPointSpent?.Invoke(this);
            if(currentLevel >= SkillSo.maxLevel)
            {
                OnSkillMaxed?.Invoke(this);
            }
            UpdateUI();
        }
    }

    public bool CanUnlockSkill()
    {
        if (prerequisiteSkillSlots == null || prerequisiteSkillSlots.Count == 0)
            return true;

        foreach (SkillSlot slot in prerequisiteSkillSlots)
        {
            if (slot == null || slot.SkillSo == null)
                return false;

            if (!slot.isUnlocked || slot.currentLevel < slot.SkillSo.maxLevel)
            {
                return false;
            }
        }

        return true;
    }

    public void Unlock()
    {
        isUnlocked = true;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Defensive null checks to avoid NullReferenceExceptions at runtime
        if (SkillSo == null)
        {
            if (skillButton != null) skillButton.interactable = false;
            if (skillLevelText != null) skillLevelText.text = "No Skill";
            if (skillIcon != null) skillIcon.color = Color.clear;
            return;
        }

        if (skillIcon != null)
            skillIcon.sprite = SkillSo.skillIcon;

        if (isUnlocked)
        {
            if (skillButton != null) skillButton.interactable = true;
            if (skillLevelText != null) skillLevelText.text = currentLevel.ToString() + "/" + SkillSo.maxLevel.ToString();
            if (skillIcon != null) skillIcon.color = Color.white;
        }
        else
        {
            if (skillButton != null) skillButton.interactable = false;
            if (skillLevelText != null) skillLevelText.text = "Locked";
            if (skillIcon != null) skillIcon.color = Color.grey;
        }
    }
}
