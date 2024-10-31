using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillSettings", menuName = "Skill Settings")]
public class SkillSettings : ScriptableObject
{
    public IndicatorController.SkillType skillType;
    public float maxSkillDistance = 10f; 
}