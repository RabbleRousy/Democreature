using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float tickInterval;
    [SerializeField] private int bloodPerBean = 1;
    [SerializeField] private TMP_Text bloodCount;
    
    [SerializeField]private float looseThreshold = 0.8f;
    [SerializeField] private int ticksTillLost = 10;

    [SerializeField] private GameObject countdownUI;
    [SerializeField] private TMP_Text countdownText;

    public float BrainCorruption { get; set; }
    public float HeartCorruption  { get; set; }
    public float BeanCorruption  { get; set; }

    private int blood = 10;
    private int looseCountdown;
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

    public event Action OnEarlyTick;
    public event Action OnTick;
    public event Action OnLateTick;

    private float timer;

    private void Awake()
    {
        timer = tickInterval;
        looseCountdown = ticksTillLost;
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
        
        if(looseCountdown <= 0) return;
        
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = tickInterval;
            OnEarlyTick?.Invoke();
            OnTick?.Invoke();
            OnLateTick?.Invoke();
            Blood += bloodPerBean * BeanManager.Instance.SweetBeans  + Mathf.FloorToInt(bloodPerBean * 0.25f * BeanManager.Instance.SourBeans) ;
            if (HeartCorruption > looseThreshold)
            {
                looseCountdown--;
                if(looseCountdown <= 0) Lost();
                countdownUI.SetActive(true);
                countdownText.text = looseCountdown.ToString();
            }
            else
            {
                countdownUI.SetActive(false);
                looseCountdown = ticksTillLost;
            }
        }
    }

    private void Lost()
    {
        SceneManager.LoadScene("Lost");
    }
}
