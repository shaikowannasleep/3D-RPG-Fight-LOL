using Game.Core;
using RPG.Saving;
using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] float healthRegenPercentage = 5f;
    [SerializeField] float selfRegenTime = 3f;
    [SerializeField] TakeDamageEvent takeDamage;
    [SerializeField] UnityEvent onDie;
    [SerializeField]
    private float maxHealth = 100f;
    [System.Serializable]
    public class TakeDamageEvent : UnityEvent<float>
    {

    }

    // LazyValue<float> healthPoints;

    private bool isDying = false;
    private bool isRestored = false;
    
    float timeSinceSelfRegen = 0;

    private void Awake()
    {
        health = maxHealth;
        
    }

    private void UpdateHealth()
    {
        health = Mathf.Min(health, maxHealth);
    }

    private void Start()
    {
        if (health <= 0)
        {
            isDying = true;
            SetCharacterActive(!isDying);
            return;
        }

        if (!isRestored)
        {
            health = maxHealth;
        }
    }

    private void SetCharacterActive(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
    public bool IsDead()
    {
        return isDying;
    }

    public void TakeDamage(GameObject instigator, float damage)
    {
       health = Mathf.Max(health - damage, 0);
        if (health == 0)
        {
            onDie.Invoke();
            Die();
        }
        else
        {
            takeDamage.Invoke(damage);
        }
    }


    public void ReceiveDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        if (health == 0)
        {
            onDie.Invoke();
            Die();
        }
        else
        {
            takeDamage.Invoke(damage);
        }
    }



    public float GetHealthPoints()
    {
        return health;
    }

    public float GetMaxHealhPoints()
    {
        return maxHealth;
    }

    public float GetPercentage()
    {
        return health / maxHealth;
    }

    private void Die()
    {
        if (isDying) return;
        isDying = true;
        GetComponent<ActionScheduler>().CancelAllActions();
        GetComponent<Animator>().SetTrigger("die");
        gameObject.SetActive(false);
        
    }
    public void SelfRegen()
    {
        if (health < maxHealth && timeSinceSelfRegen > selfRegenTime)
        {
            health = MathF.Min(health + maxHealth * healthRegenPercentage / 100, maxHealth);
            timeSinceSelfRegen = 0;
        }
        else
        {
            timeSinceSelfRegen += Time.deltaTime;
        }
    }
    private void RegenerateHealth()
    {
        maxHealth = health;
    }

    public void Heal(float healthToRestore)
    {
        health = Mathf.Min(health + healthToRestore, GetMaxHealhPoints());
    }


    private void OnReset()
    {
        gameObject.SetActive(true);
        health = maxHealth;
        timeSinceSelfRegen = 0;

    }
}
