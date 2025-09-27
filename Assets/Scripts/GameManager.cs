using System;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float tickInterval;

    public float BrainCorruption { get; set; }
    public float HeartCorruption  { get; set; }
    public float ImmuneSystemCorruption  { get; set; }
    public float BeanCorruption  { get; set; }
    public bool Paused  { get; set; }

    public static GameManager Instance;

    public event Action OnTick;

    private float timer;

    private void Awake()
    {
        timer = tickInterval;
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
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
