using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Organs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BeanMovement : MonoBehaviour
{
    private List<AreaManager> possibleTargets, usedTargets;
    private Vector3 currentTarget;
    private NavMeshAgent agent;
    
    public bool Patrolling { get; private set; }
    private PatrolTarget patrolTarget;

    private float defaultStoppingDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        defaultStoppingDistance = agent.stoppingDistance;
        Patrolling = false;
    }

    public void InitializeTargets(List<AreaManager> possibleTargets)
    {
        this.possibleTargets = possibleTargets;
        usedTargets = new List<AreaManager>();
        FindNewTarget();
        agent.SetDestination(currentTarget);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (Patrolling)
            {
                patrolTarget = patrolTarget.Next;
                currentTarget = patrolTarget.transform.position;
            }
            else
                FindNewTarget();
            
            agent.SetDestination(currentTarget);
        }
    }

    private void LateUpdate()
    {
        // Fix x rotation
        var rot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(-90f, rot.y, rot.z);
    }

    private void FindNewTarget()
    {
        if (possibleTargets.Count == 0)
        {
            possibleTargets = usedTargets;
        }
        
        var newTarget = possibleTargets[Random.Range(0, possibleTargets.Count)];
        possibleTargets.Remove(newTarget);
        usedTargets.Add(newTarget);
        currentTarget = newTarget.GetRandomPoint();
    }

    public void StartPatrolling(PatrolTarget startTarget)
    {
        Patrolling = true;
        patrolTarget = startTarget;
        currentTarget = patrolTarget.transform.position;
        agent.SetDestination(currentTarget);
        BeanManager.Instance.AddPatrollingBean(this);
        GetComponent<Spreader>().enabled = false;
        agent.stoppingDistance = 0.5f;
        agent.avoidancePriority = 90;
    }

    public void StopPatrolling()
    {
        Patrolling = false;
        FindNewTarget();
        agent.SetDestination(currentTarget);
        BeanManager.Instance.RemovePatrollingBean(this);
        GetComponent<Spreader>().enabled = true;
        agent.stoppingDistance = defaultStoppingDistance;
        agent.avoidancePriority = 90;
    }

    private void OnDestroy()
    {
        if (Patrolling)
        {
            
        }
    }
}
