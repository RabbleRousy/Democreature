using System;
using DefaultNamespace;
using TMPro;
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
    [SerializeField] private TMP_Text electionText;

    private Spreader spreader;
    private int electionCounter;
    private Animator animator;
    

    public int ClearMindCost => clearMindCost;

    private void Awake()
    {
        spreader = GetComponent<Spreader>();
        animator = GetComponent<Animator>();
        electionCounter = ticksTillElection;
        electionText.text = electionCounter.ToString();
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
        electionText.text = electionCounter.ToString();
        if (electionCounter <= 0)
        {
            electionCounter = ticksTillElection;
            electionText.text = electionCounter.ToString();
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
            GameManager.Instance.BrainCorruption -= clearMindCurve.Evaluate(GameManager.Instance.HeartCorruption) * clearMindInfluence;
            GameManager.Instance.BrainCorruption = Mathf.Clamp01(GameManager.Instance.BrainCorruption);
        }
    }

    public bool CanClearMind()
    {
        return GameManager.Instance.Blood >= clearMindCost;
    }
}
