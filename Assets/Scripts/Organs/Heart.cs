using System;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Heart : MonoBehaviour
{
    [SerializeField] private int ticksTillElection;
    [SerializeField] private float maxVirusBias = 0.2f;
    [SerializeField] private int maxPatrollingBeansForProtection = 5;

    [SerializeField] private AnimationCurve clearMindCurve;
    [SerializeField] private float clearMindInfluence;
    [SerializeField] private int clearMindCost;
    
    [SerializeField]private AudioSource audioBeat;
    [SerializeField]private AudioSource audioElection;

    private Spreader spreader;
    private int electionCounter;
    private Animator animator;
    

    public int ClearMindCost => clearMindCost;

    private void Awake()
    {
        spreader = GetComponent<Spreader>();
        animator = GetComponent<Animator>();
        electionCounter = ticksTillElection;
    }

    private void Start()
    {
        GameManager.Instance.OnTick += Tick;
        spreader.UseCorruptionChance = true;
    }

    private void Tick()
    {
        electionCounter--;
        animator.SetTrigger("Beat");
        audioBeat.Play();
        if (electionCounter <= 0)
        {
            electionCounter = ticksTillElection;
            UpdateCorruption();
        }

    }

    public void StartVirusAttack(float influence)
    {
        maxVirusBias = influence;
    }
    
    public void EndVirusAttack()
    {
        maxVirusBias = 0;
    }


    private void UpdateCorruption()
    {
        float protection = 1 - Mathf.Clamp01(BeanManager.Instance.PatrollingBeans / (float)maxPatrollingBeansForProtection);
        GameManager.Instance.HeartCorruption = BeanManager.Instance.GetSourPercentage() + maxVirusBias * protection;
        spreader.CorruptionChance = GameManager.Instance.HeartCorruption;
        Debug.Log("Protection: " + protection);
        DotColorizer.Instance.UpdateDots();
        audioElection.Play();
    }

    public void ClearMind()
    {
        if (CanClearMind())
        {
            GameManager.Instance.BrainCorruption = clearMindCurve.Evaluate(GameManager.Instance.BrainCorruption) * clearMindInfluence;
        }
    }

    public bool CanClearMind()
    {
        return GameManager.Instance.Blood >= clearMindCost;
    }
}
