using System;
using DefaultNamespace;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Bean : MonoBehaviour, ICorruptible
{
    [SerializeField] private float corruption;
    [SerializeField] private int lifeTime;
    [SerializeField] private int lifeTimeVariance;
    [SerializeField] private GameObject policeVisuals, sourVisuals;
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private float sourThreshold = 0.9f;
    [SerializeField] private float spreaderRange = 2f;
    [SerializeField] private float policeForce = 0.2f;
    [SerializeField] private float minCorruption = 0f;
    [SerializeField] private float maxCorruption = 0.75f;
    [SerializeField] private AnimatorController[] possibleAnimators;

    public Action<Bean> OnBecomeSour, OnBecomeSweet, OnBecomePolice, OnUnPolice;
    
    private SpriteRenderer renderer;
    private Spreader spreader;
    private Animator animator;
    
    public bool IsSour => corruption >= sourThreshold;
    public bool IsPolice { get; private set; }


 
    private void Awake()
    {
        OnBecomeSour += BeanManager.Instance.SweetToSour;
        OnBecomeSweet += BeanManager.Instance.SourToSweet;
        OnBecomePolice += BeanManager.Instance.AddPolice;
        OnUnPolice += BeanManager.Instance.RemovePolice;

        GameManager.Instance.OnLateTick += ReduceLifeTime;
        
        renderer = GetComponentInChildren<SpriteRenderer>();
        spreader = GetComponent<Spreader>();
        // Pick random animator
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = possibleAnimators[Random.Range(0, possibleAnimators.Length)];
        
        AddToLists();
    }

    private void Start()
    {
        Corrupt(Random.Range(minCorruption,maxCorruption));
        UpdateVisuals();
        lifeTime += Random.Range(-lifeTimeVariance, lifeTimeVariance+1);
    }

    private void ReduceLifeTime()
    {
        lifeTime--;
        if (lifeTime == 0)
        {
            RemoveFromLists();
            animator.SetTrigger("Death");
        }
    }

    public void DestroyBean()
    {
        Destroy(gameObject);
    }

    public void AddToLists()
    {
        BeanManager.Instance.AddBean(this);
        
        if (IsSour)
            BeanManager.Instance.AddSourBean(this);
        else BeanManager.Instance.AddSweetBean(this);

        if (IsPolice)
            BeanManager.Instance.AddPolice(this);
    }

    public void RemoveFromLists()
    {
        BeanManager.Instance.RemoveBean(this);
        
        if (IsSour)
            BeanManager.Instance.RemoveSourBean(this);
        else BeanManager.Instance.RemoveSweetBean(this);

        if (IsPolice)
            BeanManager.Instance.RemovePolice(this);
    }

    [ContextMenu("Corrupt")]
    public void Corrupt() => Corrupt(1f);
    
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
        sourVisuals.SetActive(IsSour);
    }


    [ContextMenu("Become Police")]
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
        spreader.enabled = true;
        spreader.Range = spreaderRange;
        spreader.Value = IsSour ? policeForce : -policeForce;
        OnBecomeSour += UpdateSpreadValue;
        OnBecomeSweet += UpdateSpreadValue;
        return true;
    }

    private void UpdateSpreadValue(Bean b)
    {
        Debug.Log("Update Value");
        var spreader = b.GetComponent<Spreader>();
        Debug.Log("Old Value " + spreader.Value);
        spreader.Value = IsSour ? policeForce : -policeForce;
        Debug.Log("New Value " + spreader.Value);
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
        spreader.enabled = false;
        
        OnBecomeSour -= UpdateSpreadValue;
        OnBecomeSweet -= UpdateSpreadValue;
    }
}
