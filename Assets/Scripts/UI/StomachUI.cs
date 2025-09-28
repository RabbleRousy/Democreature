using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StomachUI : MonoBehaviour
{
    [SerializeField] private Button factCheckerButton;
    [SerializeField] private Button stomachButton, brainButton, heartButton;
    [SerializeField] private TMP_Text factCheckerCost, stationCost, factCheckerCount;
    
    private Stomach stomach;

    private void Awake()
    {
        stomach = FindFirstObjectByType<Stomach>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFactCheckerInfo();
        UpdateStationInfo();
    }

    public void UpdateFactCheckerInfo()
    {
        factCheckerButton.interactable = stomach.CanBuyFactChecker;
        factCheckerCost.text = stomach.FactCheckerCost.ToString();
        factCheckerCount.text = "" + stomach.FactCheckerCount + "/" + stomach.MaxFactCheckers;
    }

    public void UpdateStationInfo()
    {
        stationCost.text = stomach.StationCost.ToString();
        stomachButton.interactable = stomach.CanBuyStomachStation;
        brainButton.interactable = stomach.CanBuyBrainStation;
        heartButton.interactable = stomach.CanBuyHeartStation;
    }
}
