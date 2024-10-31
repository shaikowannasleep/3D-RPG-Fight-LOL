using Game.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class AutoAttack : MonoBehaviour
{
    public float attackSpeed = 1.0f;
    protected Animator animator;
    [SerializeField]
    protected Avatar swordAvatar;
    protected float nextTimeToFire = 0f;
    protected bool isAttacking;
    public float AttackDamage = 50f;
    public float Range = 5f;
    public GameObject damageText;
    [SerializeField] private Button buttonAa;
    [SerializeField]
    private GameObject targetEnemy; 

    void Start()
    {
        animator = GetComponent<Animator>();
        buttonAa.OnClickAsObservable()
            .Subscribe(_ => BasicAttack("button"))
            .AddTo(this);
        animator.avatar = swordAvatar;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BasicAttack("mouse");
        }

        if (Input.GetMouseButton(1))
        {
            isAttacking = false;
        }
    }

    private void BasicAttack(string source)
    {
        if (source == "mouse")
        {
            Ray intersectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit interactionHitInfo;

            if (Physics.Raycast(intersectionRay, out interactionHitInfo, Mathf.Infinity))
            {
                if (interactionHitInfo.collider.CompareTag("Enemy"))
                {
                    targetEnemy = interactionHitInfo.collider.gameObject;

                    if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= Range)
                    {
                        isAttacking = true;
                        StartCoroutine(ApplyBasicAttack(targetEnemy, (int)AttackDamage));
                    }
                }
                else
                {
                    isAttacking = false;
                }
            }
        }
        else if (source == "button")
        {
            if (targetEnemy != null && Vector3.Distance(transform.position, targetEnemy.transform.position) <= Range)
            {
                isAttacking = true;
                StartCoroutine(ApplyBasicAttack(targetEnemy, (int)AttackDamage));
            }
        }
    }

    protected virtual IEnumerator ApplyBasicAttack(GameObject other, int damage)
    {
        Avatar currentAvatar = animator.avatar;
        //animator.avatar = swordAvatar;
        while (isAttacking)
        {
            if (Time.time >= nextTimeToFire)
            {
                if (other == null || Vector3.Distance(transform.position, other.transform.position) > Range)
                {
                    isAttacking = false;
                    break;
                }

                nextTimeToFire = Time.time + 1f / attackSpeed;
                transform.LookAt(other.transform);
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                {
                    animator.SetTrigger("attack");
                }
                Damage(other, damage);
            }
            yield return null;
        }
        //animator.avatar = currentAvatar;
    }

    protected void Damage(GameObject other, int damage)
    {
        if (other.TryGetComponent<CharacterHealth>(out CharacterHealth health))
        {
            health.TakeDamage(other, damage);
        }
    }

    private void Animation()
    {

    }

}