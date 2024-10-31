using Game.Attributes;
using Game.Combat;
using Game.Control;
using Game.Core;
using Game.Movement;
using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour , ISaveable , IAction 
{
    [SerializeField] float chaseDistance = 5f;
    [SerializeField] float suspicionTIme = 2f;
    //[SerializeField] float shoutDistance = 5f;
    [SerializeField] float agroCooldownTime = 10f;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1f;
    [SerializeField] float waypointDwellTime = 2f;

    [SerializeField] Transform target;
    [SerializeField] float maxSpeed = 6f;
    [SerializeField] float maxNavPathLength = 40f;
    [SerializeField]NavMeshAgent navMeshAgent;



    GameObject player;
     [SerializeField] Vector3 guardPosition;

    bool isAggrevated = false;
    float timeSinceLastSawPlayer = Mathf.Infinity;
    float timeSinceArrivedAtWaypoint = Mathf.Infinity;
    float timeSinceAggrevated = Mathf.Infinity;
    int currentWaypointIndex = 0;

    Health health;
    bool isAttacking = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        guardPosition = transform.position;
    }

    private void Update()
    {
        if (IsAggrevated() && CanAttack(player))
        {
            AttackBehaviour();
        }
        else if (timeSinceLastSawPlayer < suspicionTIme)
        {
            SuspicionBehaviour();
        }
        else
        {
            PatrolBehaviour();
        }

        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceArrivedAtWaypoint += Time.deltaTime;
        timeSinceAggrevated += Time.deltaTime;
    }

    public void Aggrevate()
    {
        isAggrevated = true;
        timeSinceAggrevated = 0;
    }

    private bool IsAggrevated()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (isAggrevated)
        {
            return timeSinceAggrevated < agroCooldownTime;
        }
        else
        {
            return distance < chaseDistance;
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    private void PatrolBehaviour()
    {
        Vector3 nextPosition = guardPosition;
        if (patrolPath != null)
        {
            if (AtWayPoint())
            {
                timeSinceArrivedAtWaypoint = 0;
                CycleWaypoint();
            }
            nextPosition = GetCurrentWayPoint();
        }
        if (timeSinceArrivedAtWaypoint > waypointDwellTime)
        {
            StartMoveAction(nextPosition, 1f);
        }
        isAggrevated = false;
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    public void StartMoveAction(Vector3 des, float speedFraction)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        MoveTo(des,1f);
    }
    public void MoveTo(Vector3 des, float speedFraction)
    {
        navMeshAgent.destination = des;
        navMeshAgent.speed = maxSpeed * speedFraction;
        navMeshAgent.isStopped = false;
    }
    private bool AtWayPoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
        return distanceToWaypoint < waypointTolerance;
    }
    private Vector3 GetCurrentWayPoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void SuspicionBehaviour()
    {
        GetComponent<ActionScheduler>().CancelAction();
    }

    public bool CanAttack(GameObject combatTarget)
    {
        if (combatTarget == null) return false;
        if (!CanMoveTo(combatTarget.transform.position)) return false;
        Health target = combatTarget.GetComponent<Health>();
        return target != null && !target.IsDead();
    }

    public bool CanMoveTo(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
        if (!hasPath) return false;
        if (path.status != NavMeshPathStatus.PathComplete) return false;
        if (GetPathLength(path) > maxNavPathLength) return false;

        return true;
    }

    private float GetPathLength(NavMeshPath path)
    {
        float total = 0;
        if (path.corners.Length < 2) return total;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return total;
    }

    private void AttackBehaviour()
    {
        timeSinceLastSawPlayer = 0;
        this.Attack(player);
        transform.LookAt(target);
    }
    public void Attack(GameObject combatTarget)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        Debug.Log("Attack");
    }

    public void Cancel()
    {
        //GetComponent<Animator>().ResetTrigger("attack");
        //GetComponent<Animator>().SetTrigger("stopAttack");
        target = null;
        SetAttacking(false);
    }
    public void SetAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }

    public object CaptureState()
    {
        return new SerializableVector3(guardPosition);
    }

    public void RestoreState(object state)
    {
        guardPosition = ((SerializableVector3)state).ToVector();
        isAggrevated = false;
        timeSinceLastSawPlayer = Mathf.Infinity;
        timeSinceArrivedAtWaypoint = Mathf.Infinity;
        timeSinceAggrevated = Mathf.Infinity;
        currentWaypointIndex = 0;
    }


}
