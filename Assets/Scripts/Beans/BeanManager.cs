using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeanManager : MonoBehaviour
{
    public static BeanManager Instance;

    [SerializeField] private List<Bean> allBeans, sweetBeans, sourBeans, policeBeans;

    public int PoliceBeans => policeBeans.Count;
    public int SourBeans => sourBeans.Count;
    public int AllBeans => allBeans.Count;
    public int SweetBeans => sweetBeans.Count;

    private void Awake()
    {
        Instance = this;
        allBeans = new List<Bean>();
        sweetBeans = new List<Bean>();
        sourBeans = new List<Bean>();
        policeBeans = new List<Bean>();
    }

    public float GetParasitePercentage()
    {
        return (float)sourBeans.Count / (sweetBeans.Count + sourBeans.Count);
    }

    public void AddSweetBean(Bean bean) => sweetBeans.Add(bean);
    public void RemoveSweetBean(Bean bean) => sweetBeans.Remove(bean);
    public void AddSourBean(Bean bean) => sourBeans.Add(bean);
    public void RemoveSourBean(Bean bean) => sourBeans.Remove(bean);
    public void AddPolice(Bean bean) => policeBeans.Add(bean);
    public void RemovePolice(Bean bean) => policeBeans.Remove(bean);

    public void SweetToSour(Bean bean)
    {
        sweetBeans.Remove(bean);
        sourBeans.Add(bean);
    }

    public void SourToSweet(Bean bean)
    {
        sourBeans.Remove(bean);
        sweetBeans.Add(bean);
    }

    public bool TryCreatePoliceBean(bool shouldBeSourBean)
    {
        Bean bean;
        if (shouldBeSourBean)
        {
            bean = sourBeans.FirstOrDefault(bean => !policeBeans.Contains(bean));
        }
        else
        {
            bean = sourBeans.FirstOrDefault(bean => !policeBeans.Contains(bean));
        }

        if (bean == null)
        {
            bean = allBeans.FirstOrDefault(bean => !policeBeans.Contains(bean));
        }

        if (bean != null)
        {
            return bean.BecomePolice();
        }

        return false;
    }
}
