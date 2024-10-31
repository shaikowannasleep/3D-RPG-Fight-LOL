using Game.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject damageTextPrefab;
  
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            CharacterHealth target = other.gameObject.GetComponent<CharacterHealth>();
            if (target != null)
            {
                target.TakeDamage(null, damage);

                if (hitEffectPrefab != null)
                {
                    Instantiate(hitEffectPrefab, startPosition, Quaternion.identity);
                }

                ShowDamage(damage, transform.position);
                Destroy(gameObject);
            }
        }
    }
    private void Update()
    {
        MoveBullet();
    }

    private void MoveBullet()
    {
        transform.Translate(Vector3.forward * 10f * Time.deltaTime);
    }

    private void CheckDistanceTravelled()
    {
        if (Vector3.Distance(transform.position, startPosition) >= 100f)
        {
            Destroy(gameObject);
        }
    }
    public void ShowDamage(float damageAmount, Vector3 hitPosition)
    {
        if (damageTextPrefab != null)
        {
            GameObject dmgTextInstance = Instantiate(damageTextPrefab, hitPosition, Quaternion.identity);
            DamageText dmgTextComponent = dmgTextInstance.GetComponent<DamageText>();
            if (dmgTextComponent != null)
            {
                dmgTextComponent.SetValue(damageAmount);

            }
        }
    }

    public void Initialize(float damage, GameObject hitEffectPrefab)
    {
        this.damage = damage;
        this.hitEffectPrefab = hitEffectPrefab;
    }
}
