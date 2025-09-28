using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private Button clearMindButton;
    [SerializeField] private TMP_Text clearMindCost, brainCorruption;
    
    private Heart heart;

    private void Awake()
    {
        heart = FindFirstObjectByType<Heart>();
    }

    // Update is called once per frame
    void Update()
    {
        clearMindButton.interactable = heart.CanClearMind();
        clearMindCost.text = heart.ClearMindCost.ToString();
        brainCorruption.text = "Brain Fog: " + (int)(GameManager.Instance.BrainCorruption * 100) + "%";
    }
}
