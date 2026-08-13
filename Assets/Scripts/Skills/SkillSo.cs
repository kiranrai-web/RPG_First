using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillTree/Skill")] 

public class SkillSo : ScriptableObject
{
    public string skillName;
    public int maxLevel;
    public Sprite skillIcon;
}
