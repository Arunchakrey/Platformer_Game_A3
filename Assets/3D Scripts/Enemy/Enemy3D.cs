using UnityEngine;
using UnityEngine.AI;

public class Enemy3D : MonoBehaviour
{
    [Header("Refs")]
    public NavMeshAgent agent;
    public Transform player;

    [Header("Layers")]
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    [Header("Stats")]
    public float health = 50f;

    [Header("Patrol")]
    public float walkPointRange = 10f;
    Vector3 walkPoint;
    bool walkPointSet;

    [Header("Combat")]
    public float sightRange = 15f;
    public float attackRange = 2.0f;              // melee reach
    public float timeBetweenAttacks = 1.0f;       // attack cooldown
    public int damage = 10;                       // damage per swing
    public Transform attackOrigin;                // optional: a child at chest/hand height
    public float attackRadius = 1.25f;            // sphere radius for hit detection

    bool playerInSightRange, playerInAttackRange, alreadyAttacked;

    void Awake()
    {
        if (player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        // Keep the agent from clipping into the player; he’ll stop just short of attack range.
        if (agent != null) agent.stoppingDistance = Mathf.Max(attackRange * 0.9f, 0.1f);

        if (attackOrigin == null) attackOrigin = transform; // fall back if not assigned
    }

    void Update()
    {
        if (player == null || agent == null) return;

        // Perception
        playerInSightRange  = Physics.CheckSphere(transform.position, sightRange,  whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // State machine
        if (!playerInSightRange && !playerInAttackRange)
            Patrol();
        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInSightRange && playerInAttackRange)
            AttackPlayer();

            if (agent)
    {
        Debug.Log($"[Enemy] onNav:{agent.isOnNavMesh} stopped:{agent.isStopped} " +
                $"hasPath:{agent.hasPath} rem:{agent.remainingDistance:F2} " +
                $"spd:{agent.speed} walkSet:{walkPointSet} wp:{walkPoint}");
    }
    }

    //STATES
    void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.isStopped = false;
            agent.SetDestination(walkPoint);

            var distance = Vector3.Distance(transform.position, walkPoint);
            if (distance < 1f)
            {
                walkPointSet = false; // <- important fix: pick a NEW point once reached
            }
        }
    }

    void SearchWalkPoint()
    {
        // Try a few random points near the current position
        const int attempts = 6;
        for (int i = 0; i < attempts; i++)
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            Vector3 candidate = new Vector3(transform.position.x + randomX, transform.position.y + 5f, transform.position.z + randomZ);

            // Raycast down to find ground
            if (Physics.Raycast(candidate, Vector3.down, out RaycastHit hit, 20f, whatIsGround, QueryTriggerInteraction.Ignore))
            {
                // Optionally ensure it's on the NavMesh
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit nmHit, 1.0f, NavMesh.AllAreas))
                {
                    walkPoint = nmHit.position;
                    walkPointSet = true;
                    return;
                }
            }
        }
        // If no point found, we'll try again next frame.
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        // Smooth face the player while moving
        FaceTarget(player.position);
    }

    void AttackPlayer()
    {
        // Stop moving, face target
        agent.isStopped = true;
        FaceTarget(player.position);

        if (alreadyAttacked) return;

        // Do a melee swing using an overlap sphere at the attack origin
        Collider[] hits = Physics.OverlapSphere(
            attackOrigin.position,
            attackRadius,
            whatIsPlayer,
            QueryTriggerInteraction.Ignore
        );

        // foreach (var h in hits)
        // {
        //     // Try to damage something with a health component
        //     var hp = h.GetComponent<PlayerHealth>(); // example health script (below)
        //     if (hp != null)
        //     {
        //         hp.TakeDamage(damage);
        //     }
        // }

        // Cooldown
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    void ResetAttack() => alreadyAttacked = false;

    void FaceTarget(Vector3 worldPos)
    {
        Vector3 to = (worldPos - transform.position);
        to.y = 0f;
        if (to.sqrMagnitude < 0.0001f) return;
        Quaternion look = Quaternion.LookRotation(to);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 10f);
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0f) Die();
    }

    void Die()
    {
        // TODO: play death anim/sfx
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (attackOrigin != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
        }
    }
}
