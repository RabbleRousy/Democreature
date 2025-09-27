using System;
using UnityEngine;

public class BeanCorruption : MonoBehaviour, ICorruptible
{
    [SerializeField] private float corruption;

    private static float parasiteThreshold = 0.9f;
    
    public bool IsParasite => corruption >= parasiteThreshold;

    public Action<BeanCorruption> OnBecomeParasite, OnBecomeSymbiont;

    private void Awake()
    {
        OnBecomeParasite += BeanManager.Instance.SourToSweet;
        OnBecomeSymbiont += BeanManager.Instance.SweetToSour;
    }

    public void Start()
    {
        if (IsParasite)
            BeanManager.Instance.AddSourBean(this);
        else BeanManager.Instance.AddSweetBean(this);
    }

    public void OnDestroy()
    {
        if (IsParasite)
            BeanManager.Instance.RemoveSourBean(this);
        else BeanManager.Instance.RemoveSweetBean(this);
    }

    public void Corrupt(float value)
    {
        bool wasParasite = IsParasite;
        corruption += value;
        corruption = Mathf.Clamp01(corruption);
        
        if (wasParasite && !IsParasite) OnBecomeSymbiont?.Invoke(this);
        else if (!wasParasite && IsParasite) OnBecomeParasite?.Invoke(this);
    }
}
