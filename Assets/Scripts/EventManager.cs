using System;
using DefaultNamespace.Organs;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    [Header("Virus"),SerializeField] private float weakVirusAttack = 0.2f;
    [SerializeField] private float mediumVirusAttack = 0.3f;
    [SerializeField] private float strongVirusAttack = 0.4f;
    [SerializeField] private int durationActiveVirusAttack = 10;
    private const string IDVirusAttack = "Virus";
    [SerializeField] private string virusDescription;
    [SerializeField] private Sprite virusIcon;

    [Header("Freeze"), SerializeField] private int brainFreezeDuration = 5;
    private const string IDFreeze = "Freeze";
    [SerializeField] private string brainFreezeDescription;
    [SerializeField] private Sprite brainFreezeIcon;

    [Header("Brain Corruption"),SerializeField] private float weakBrainCorruption = 0.2f;
    [SerializeField] private float mediumBrainCorruption = 0.4f;
    [SerializeField] private float strongBrainCorruption = 0.5f;
    [SerializeField] private string brainFogDescription;
    [SerializeField] private Sprite brainFogIcon;
    [SerializeField] private float brainFogInfoDuration = 3.0f;
    
    

    [Header("General"),SerializeField] private Heart heart;
    [SerializeField] private Brain brain;
    [SerializeField] private int minTicksTillNext;
    [SerializeField] private int maxTicksTillNext;

    private string currentEvent = "";
    private int currentEventDuration;
    private int ticksTillNext;
    
    private EventUI eventUI;

    private void Start()
    {
        GameManager.Instance.OnEarlyTick += Tick;
        eventUI = FindFirstObjectByType<EventUI>();
    }

    private void Tick()
    {
        if (currentEvent.Length <= 0)
        {
            ticksTillNext--;
        }
        else
        {
            currentEventDuration--;
        }
        

        if (ticksTillNext <= 0)
        {
            TriggerRandomEvent();
            ticksTillNext = Random.Range(minTicksTillNext, maxTicksTillNext);
        }

        if (currentEventDuration <= 0)
        {
            StopCurrentEvent();
        }
    }

    private void TriggerRandomEvent()
    {
        switch (Random.Range(0,3))
        {
            case 0:
                TriggerVirusAttack();
                break;
            case 1:
                TriggerBrainFreeze();
                break;
            case 2:
                TriggerBrainCorruption();
                break;
        }
    }

    private void TriggerBrainCorruption()
    {
        switch (Random.Range(0,2))
        {
            case 0:
                GameManager.Instance.BrainCorruption += weakBrainCorruption;
                break;
            case 1:
                GameManager.Instance.BrainCorruption += mediumBrainCorruption;
                break;
            case 2:
                GameManager.Instance.BrainCorruption += strongBrainCorruption;
                break;
        }
        
        Debug.Log("Triggered: Brain Corruption" );
        eventUI.Show("Brain Fog!", brainFogDescription, brainFogIcon, brainFogInfoDuration);
    }


    private void TriggerBrainFreeze()
    {
        brain.Freeze = true;
        currentEvent = IDFreeze;
        currentEventDuration = brainFreezeDuration;
        Debug.Log("Triggered: " + IDFreeze);
        eventUI.Show("Brain Freeze!", brainFreezeDescription, brainFreezeIcon);
    }
    
    private void TriggerVirusAttack()
    {
        switch (Random.Range(0,3))
        {
            case 0:
                heart.StartVirusAttack(weakVirusAttack);
                break;
            case 1:
                heart.StartVirusAttack(mediumVirusAttack);
                break;
            case 2:
                heart.StartVirusAttack(strongVirusAttack);
                break;
        }

        currentEvent = IDVirusAttack;
        currentEventDuration = durationActiveVirusAttack;
        Debug.Log("Triggered: " + IDVirusAttack);
        eventUI.Show("Virus Attack!", virusDescription, virusIcon);
    }

    private void StopCurrentEvent()
    {
        switch (currentEvent)
        {
            case IDVirusAttack:
                heart.EndVirusAttack();
                Debug.Log("Stopped: " + IDVirusAttack);
                break;
            case IDFreeze:
                brain.Freeze = false;
                Debug.Log("Stopped: " + IDFreeze);
                break;
        }

        currentEvent = "";
        currentEventDuration = 0;
        eventUI.Hide();
    }
}
