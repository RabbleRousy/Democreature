using System;
using DefaultNamespace;
using UnityEngine;

public class Bean : MonoBehaviour, ICorruptible
{
    [SerializeField] private float corruption;
    [SerializeField] private uint lifeTime;
    [SerializeField] private GameObject policeVisuals;
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private float sourThreshold = 0.9f;

    public Action<Bean> OnBecomeSour, OnBecomeSweet, OnBecomePolice, OnUnPolice;
    
    private SpriteRenderer renderer;
    
    public bool IsSour => corruption >= sourThreshold;
    public bool IsPolice { get; private set; }


 
    private void Awake()
    {
        OnBecomeSour += BeanManager.Instance.SourToSweet;
        OnBecomeSweet += BeanManager.Instance.SweetToSour;
        OnBecomePolice += BeanManager.Instance.AddPolice;
        OnUnPolice += BeanManager.Instance.RemovePolice;

        GameManager.Instance.OnTick += ReduceLifeTime;
        
        renderer = GetComponentInChildren<SpriteRenderer>();
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
        BeanManager.Instance.AddBean(this);
        
        if (IsSour)
            BeanManager.Instance.AddSourBean(this);
        else BeanManager.Instance.AddSweetBean(this);

        if (IsPolice)
            BeanManager.Instance.AddPolice(this);
    }

    public void OnDestroy()
    {
        BeanManager.Instance.RemoveBean(this);
        
        if (IsSour)
            BeanManager.Instance.RemoveSourBean(this);
        else BeanManager.Instance.RemoveSweetBean(this);

        if (IsPolice)
            BeanManager.Instance.RemovePolice(this);
    }

    public void Corrupt(float value)
    {
        bool wasSour = IsSour;
        corruption += value;
        corruption = Mathf.Clamp01(corruption);
        UpdateVisuals();
        
        if (wasSour && !IsSour) OnBecomeSweet?.Invoke(this);
        else if (!wasSour && IsSour) OnBecomeSour?.Invoke(this);
    }

    private void UpdateVisuals()
    {
        renderer.color = colorGradient.Evaluate(corruption / sourThreshold);
    }

    [ContextMenu("Become Police")]
    private bool BecomePolice() => BecomePolice(0.1f);
    
    public bool BecomePolice(float policeForce)
    {
        if (IsPolice)
        {
            Debug.LogError("Trying to promote Bean to police, but is already police!");
            return false;
        }
        
        IsPolice = true;
        OnBecomePolice(this);
        policeVisuals.SetActive(true);
        var spreader = gameObject.AddComponent<Spreader>();
        spreader.Value = IsSour ? policeForce : -policeForce;
        OnBecomeSour += InvertSpreadValue;
        OnBecomeSweet += InvertSpreadValue;
        return true;
    }

    private void InvertSpreadValue(Bean b)
    {
        var spreader = b.GetComponent<Spreader>();
        spreader.Value = -spreader.Value;
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
        Destroy(gameObject.GetComponent<Spreader>());
        OnBecomeSour -= InvertSpreadValue;
        OnBecomeSweet -= InvertSpreadValue;
    }
}
