using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeanManager : MonoBehaviour
{
    public static BeanManager Instance;

    [SerializeField] private List<BeanCorruption> allBeans, sweetBeans, sourBeans, policeBeans;

    public int PoliceBeans => policeBeans.Count;

    private void Awake()
    {
        Instance = this;
        allBeans = new List<BeanCorruption>();
        sweetBeans = new List<BeanCorruption>();
        sourBeans = new List<BeanCorruption>();
        policeBeans = new List<BeanCorruption>();
    }

    public float GetSourPercentage()
    {
        return (float)sourBeans.Count / (sweetBeans.Count + sourBeans.Count);
    }

    public void AddSweetBean(BeanCorruption bean) => sweetBeans.Add(bean);
    public void RemoveSweetBean(BeanCorruption bean) => sweetBeans.Remove(bean);
    public void AddSourBean(BeanCorruption bean) => sourBeans.Add(bean);
    public void RemoveSourBean(BeanCorruption bean) => sourBeans.Remove(bean);

    public void SweetToSour(BeanCorruption bean)
    {
        sweetBeans.Remove(bean);
        sourBeans.Add(bean);
        GameManager.Instance.BeanCorruption = GetSourPercentage();
    }

    public void SourToSweet(BeanCorruption bean)
    {
        sourBeans.Remove(bean);
        sweetBeans.Add(bean);
        GameManager.Instance.BeanCorruption = GetSourPercentage();
    }

    public bool TryCreatePoliceBean(bool shouldBeSourBean)
    {
        BeanCorruption bean;
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
            policeBeans.Add(bean);
            return true;
        }

        return false;
    }
}
