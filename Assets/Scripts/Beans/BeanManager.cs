using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeanManager : MonoBehaviour
{
    public static BeanManager Instance;

    [SerializeField] private List<Bean> allBeans, sweetBeans, sourBeans, policeBeans;
    [SerializeField]private List<BeanMovement> patrollingBeans;

    public int PoliceBeans => policeBeans.Count;
    public int SourBeans => sourBeans.Count;
    public int AllBeans => allBeans.Count;
    public int SweetBeans => sweetBeans.Count;

    public int PatrollingBeans => patrollingBeans.Count;

    private void Awake()
    {
        Instance = this;
        allBeans = new List<Bean>();
        sweetBeans = new List<Bean>();
        sourBeans = new List<Bean>();
        policeBeans = new List<Bean>();
    }

    public float GetSourPercentage()
    {
        return (float)sourBeans.Count / (allBeans.Count);
    }

    public void AddBean(Bean bean) => allBeans.Add(bean);
    public void RemoveBean(Bean bean) => allBeans.Remove(bean);
    public void AddSweetBean(Bean bean) => sweetBeans.Add(bean);
    public void RemoveSweetBean(Bean bean) => sweetBeans.Remove(bean);
    public void AddSourBean(Bean bean) => sourBeans.Add(bean);
    public void RemoveSourBean(Bean bean) => sourBeans.Remove(bean);
    public void AddPolice(Bean bean) => policeBeans.Add(bean);
    public void RemovePolice(Bean bean) => policeBeans.Remove(bean);
    
    public void AddPatrollingBean(BeanMovement bean) => patrollingBeans.Add(bean);
    public void RemovePatrollingBean(BeanMovement bean) => patrollingBeans.Remove(bean);

    public void SweetToSour(Bean bean)
    {
        sweetBeans.Remove(bean);
        if(!sourBeans.Contains(bean))sourBeans.Add(bean);
        GameManager.Instance.BeanCorruption = GetSourPercentage();
    }

    public void SourToSweet(Bean bean)
    {
        sourBeans.Remove(bean);
        if(!sweetBeans.Contains(bean)) sweetBeans.Add(bean);
        GameManager.Instance.BeanCorruption = GetSourPercentage();
    }

    public bool TryCreatePoliceBean(bool shouldBeSourBean)
    {
        Bean bean;
        if (shouldBeSourBean)
        {
            bean = sourBeans.LastOrDefault(bean => !policeBeans.Contains(bean));
        }
        else
        {
            bean = sweetBeans.LastOrDefault(bean => !policeBeans.Contains(bean));
        }

        if (bean == null)
        {
            bean = allBeans.LastOrDefault(bean => !policeBeans.Contains(bean));
        }

        if (bean != null)
        {
            return bean.BecomePolice();
        }

        return false;
    }

    public void TryUnPolice()
    {
        Bean[] badBeans = sourBeans.Where(bean => policeBeans.Contains(bean)).ToArray();
        if(badBeans.Length <= 0) return;
        
        badBeans[Random.Range(0, badBeans.Count())].UnPolice();
    }

    public bool TryIncreasePatrol(PatrolTarget target)
    {
        // Find non-patrolling bean and send it to patrol
        foreach (var b in policeBeans)
        {
            var movement = b.GetComponent<BeanMovement>();
            if (movement.Patrolling) continue;
            
            movement.StartPatrolling(target);
            return true;
        }

        return false;
    }

    public void DecreasePatrol()
    {
        // Find patrolling bean and stop it
        foreach (var b in policeBeans)
        {
            var movement = b.GetComponent<BeanMovement>();
            if (!movement.Patrolling) continue;
            
            movement.StopPatrolling();
            return;
        }
    }
}
