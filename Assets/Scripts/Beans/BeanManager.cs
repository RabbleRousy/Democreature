using System;
using System.Collections.Generic;
using UnityEngine;

public class BeanManager : MonoBehaviour
{
    public static BeanManager Instance;

    private List<BeanCorruption> allBeans, sweetBeans, sourBeans, policeBeans;

    private void Awake()
    {
        Instance = this;
    }

    public float GetParasitePercentage()
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
    }

    public void SourToSweet(BeanCorruption bean)
    {
        sourBeans.Remove(bean);
        sweetBeans.Add(bean);
    }
}
