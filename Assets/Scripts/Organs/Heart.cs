using System;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Heart : MonoBehaviour
{
    [SerializeField] private int ticksTillElection;
    [SerializeField] private float maxVirusBias = 0.2f;
    [SerializeField] private int maxPatrollingBeansForProtection;

    private Spreader spreader;
    private int electionCounter;

    private void Awake()
    {
        spreader = GetComponent<Spreader>();
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

        if (electionCounter <= 0)
        {
            electionCounter = ticksTillElection;
            UpdateCorruption();
        }

    }

    private void UpdateCorruption()
    {
        float protection = 1 - Mathf.Clamp01(BeanManager.Instance.PatrollingBeans / (float)maxPatrollingBeansForProtection);
        GameManager.Instance.HeartCorruption = BeanManager.Instance.GetSourPercentage() + maxVirusBias * protection;
        spreader.CorruptionChance = GameManager.Instance.HeartCorruption;
        Debug.Log("Protection: " + protection);
        DotColorizer.Instance.UpdateDots();
    }
}
