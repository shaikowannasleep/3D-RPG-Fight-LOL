using UnityEngine;

[CreateAssetMenu(fileName = "AbilityConfig", menuName = "ScriptableObjects/AbilityConfig", order = 1)]
public class AbilityConfig : ScriptableObject
{
    public float healthToSubtract;
    public float additionalDamage;
    public float additionalAttackSpeed;
    public float skillActiveTime;
    public float slowFactor;
    public float slowDuration;
}
