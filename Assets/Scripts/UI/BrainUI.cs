using System;
using DefaultNamespace.Organs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrainUI : MonoBehaviour
{
    [SerializeField] private Button unpoliceButton;
    [SerializeField] private TMP_Text unpoliceCost;
    
    private Brain brain;

    private void Awake()
    {
        brain = FindFirstObjectByType<Brain>();
    }

    // Update is called once per frame
    void Update()
    {
        unpoliceButton.interactable = brain.CanUnPolice();
        unpoliceCost.text = "" + brain.UnPoliceCost;
    }
}
