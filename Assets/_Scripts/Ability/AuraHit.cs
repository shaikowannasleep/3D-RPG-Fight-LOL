using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraHit : MonoBehaviour
{
    [SerializeField] private AbilityConfig config;
    [SerializeField] private GameObject hitEffectPrefab;
   // [SerializeField] private float damage = 20f;
    public virtual void OnInit()
    {
        Destroy(gameObject, 1f);
    }

    private void Start()
    {
        Destroy(gameObject, 1f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Trigger");
            //CharacterHealth characterHealth = other.GetComponent<CharacterHealth>();
            //characterHealth.ReceiveDamage(damage);
            //ApplyDamage(damage);
        }
    }


    public void ApplyDamage(float damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                CharacterHealth target = hit.GetComponent<CharacterHealth>();
                if (target != null)
                {
                    target.ReceiveDamage(damage); 
                }
            }
        }
    }

}