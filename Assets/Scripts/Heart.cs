using System;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Heart : MonoBehaviour
{
    [SerializeField] private int ticksTillElection;

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
        GameManager.Instance.HeartCorruption = BeanManager.Instance.GetParasitePercentage();
        spreader.CorruptionChance = GameManager.Instance.HeartCorruption;
    }
}
