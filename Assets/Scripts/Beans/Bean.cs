using System;
using UnityEngine;

public class Bean : MonoBehaviour, ICorruptible
{
    [SerializeField] private float corruption;

    private static float sourThreshold = 0.9f;
    
    public bool IsSour => corruption >= sourThreshold;
    public bool IsPolice { get; private set; }

    public Action<Bean> OnBecomeSour, OnBecomeSweet, OnBecomePolice, OnUnPolice;

    [SerializeField] private uint lifeTime;
    [SerializeField] private GameObject policeVisuals;
 
    private void Awake()
    {
        OnBecomeSour += BeanManager.Instance.SourToSweet;
        OnBecomeSweet += BeanManager.Instance.SweetToSour;
        OnBecomePolice += BeanManager.Instance.AddPolice;
        OnUnPolice += BeanManager.Instance.RemovePolice;

        GameManager.Instance.OnTick += ReduceLifeTime;
    }

    private void ReduceLifeTime()
    {
        lifeTime--;
        if (lifeTime == 0)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        if (IsSour)
            BeanManager.Instance.AddSourBean(this);
        else BeanManager.Instance.AddSweetBean(this);

        if (IsPolice)
            BeanManager.Instance.AddPolice(this);
    }

    public void OnDestroy()
    {
        if (IsSour)
            BeanManager.Instance.RemoveSourBean(this);
        else BeanManager.Instance.RemoveSweetBean(this);

        if (IsPolice)
            BeanManager.Instance.RemovePolice(this);
    }

    public void Corrupt(float value)
    {
        bool wasParasite = IsSour;
        corruption += value;
        corruption = Mathf.Clamp01(corruption);
        
        if (wasParasite && !IsSour) OnBecomeSweet?.Invoke(this);
        else if (!wasParasite && IsSour) OnBecomeSour?.Invoke(this);
    }

    public bool BecomePolice()
    {
        if (IsPolice)
        {
            Debug.LogError("Trying to promote Bean to police, but is already police!");
            return false;
        }
        
        IsPolice = true;
        OnBecomePolice(this);
        policeVisuals.SetActive(true);
        return true;
    }

    public void UnPolice()
    {
        if (!IsPolice)
        {
            Debug.LogError("Trying to remove police from bean, but is not police!");
            return;
        }
        IsPolice = false;
        OnUnPolice(this);
        policeVisuals.SetActive(false);
    }
}
