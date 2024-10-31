using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class DashDamageArea : MonoBehaviour
{
    [SerializeField] private AbilityConfig config;
    public bool applySlowOnly = true;

    private void Start()
    {
        Destroy(gameObject, config.slowDuration + 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Detect");
            CharacterHealth target = other.GetComponent<CharacterHealth>();
            if (target != null)
            {
                if (applySlowOnly)
                {
                    var enemyAgent = other.GetComponent<NavMeshAgent>();
                    if (enemyAgent != null)
                    {
                        ApplySlow(enemyAgent, config.slowDuration);
                    }
                }
            }
        }
    }

    private async void ApplySlow(NavMeshAgent enemyAgent, float duration)
    {
        float originalSpeed = enemyAgent.speed;
        enemyAgent.speed *= config.slowFactor;

        await Task.Delay((int)(duration * 1000));

        enemyAgent.speed = originalSpeed;
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
                    target.TakeDamage(null, damage);
                }
            }
        }
    }
}
