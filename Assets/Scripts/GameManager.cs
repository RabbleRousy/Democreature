using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float tickInterval;

    public float BrainCorruption { get; set; }
    public float HeartCorruption  { get; set; }
    public float ImmuneSystemCorruption  { get; set; }
    public float BeanCorruption  { get; set; }
    public bool Paused  { get; set; }

    public event Action OnTick;

    private float timer;
    private void Start()
    {
        timer = tickInterval;
    }
    
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = tickInterval;
            OnTick?.Invoke();
        }
    }
}
