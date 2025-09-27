using System;
using UnityEngine;

public class BeanCorruption : MonoBehaviour, ICorruptible
{
    [SerializeField] private float corruption;

    private static float parasiteThreshold = 0.9f;
    
    public bool IsParasite => corruption >= parasiteThreshold;

    public Action OnBecomeParasite, OnBecomeSymbiont;

    private void Awake()
    {
        OnBecomeParasite += BeanManager.Instance.ConvertToParasite;
        OnBecomeSymbiont += BeanManager.Instance.ConvertToSymbiont;
    }

    public void Start()
    {
        if (IsParasite)
            BeanManager.Instance.AddParasite();
        else BeanManager.Instance.AddSymbiont();
    }

    public void OnDestroy()
    {
        if (IsParasite)
            BeanManager.Instance.RemoveParasite();
        else BeanManager.Instance.RemoveSymbiont();
    }

    public void Corrupt(float value)
    {
        bool wasParasite = IsParasite;
        corruption += value;
        corruption = Mathf.Clamp01(corruption);
        
        if (wasParasite && !IsParasite) OnBecomeSymbiont?.Invoke();
        else if (!wasParasite && IsParasite) OnBecomeParasite?.Invoke();
    }
}
