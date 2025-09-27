using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BeanMovement : MonoBehaviour
{
    private List<AreaManager> possibleTargets, usedTargets;
    private Vector3 currentTarget;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void InitializeTargets(List<AreaManager> possibleTargets)
    {
        this.possibleTargets = possibleTargets;
        usedTargets = new List<AreaManager>();
        FindNewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, currentTarget) < 1f)
        {
            FindNewTarget();
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
        agent.SetDestination(currentTarget);
    }
}
