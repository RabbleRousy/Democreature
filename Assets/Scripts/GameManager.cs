using System;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float tickInterval;
    [SerializeField] private int bloodPerBean = 1;

    public float BrainCorruption { get; set; }
    public float HeartCorruption  { get; set; }
    public float BeanCorruption  { get; set; }
    
    public int Blood  { get; set; }
    public bool Paused  { get; set; }

    public static GameManager Instance;

    public event Action OnTick;
    public event Action OnLateTick;

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
            OnLateTick?.Invoke();
            Blood += bloodPerBean * BeanManager.Instance.SweetBeans;
        }
    }
}
