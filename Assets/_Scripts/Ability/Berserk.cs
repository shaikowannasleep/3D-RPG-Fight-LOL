using System;
using UniRx;
using UnityEngine;

[CreateAssetMenu]
[RequireComponent(typeof(AutoAttack))]
public class Berserk : Ability
{
    [SerializeField] private AbilityConfig config;

    public override void Activate(GameObject parent)
    {
        CharacterHealth playerHealth = parent.GetComponent<CharacterHealth>();
        AutoAttack autoAttack = parent.GetComponent<AutoAttack>();
        if (playerHealth != null && autoAttack != null)
        {
            playerHealth.TakeDamage(parent, config.healthToSubtract);

            autoAttack.AttackDamage += config.additionalDamage;
            autoAttack.attackSpeed += config.additionalAttackSpeed;
       
            Observable.Timer(TimeSpan.FromSeconds(config.skillActiveTime))
                .Subscribe(_ =>
                {
                    autoAttack.AttackDamage -= config.additionalDamage;
                    autoAttack.attackSpeed -= config.additionalAttackSpeed;
                })
                .AddTo(parent);
            playerHealth.Heal(autoAttack.AttackDamage/10);
        }
    }
}
