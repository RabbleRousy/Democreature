using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float tickInterval;
    [SerializeField] private int bloodPerBean = 1;
    [SerializeField] private TMP_Text bloodCount;

    public float BrainCorruption { get; set; }
    public float HeartCorruption  { get; set; }
    public float BeanCorruption  { get; set; }

    private int blood;
    public int Blood
    {
        get => blood;
        set
        {
            blood = value;
            bloodCount.text = blood.ToString();
        }
    }

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
