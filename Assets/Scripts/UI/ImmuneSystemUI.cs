using System;
using DefaultNamespace.Organs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImmuneSystemUI : MonoBehaviour
{
    private ImmuneSystem immuneSystem;
    [SerializeField] private Button recruitButton, upgradeButton, decreaseButton, increaseButton;
    [SerializeField] private TMP_Text recruitPrice, upgradePrice, policeCount, patrolCount;

    private void Awake()
    {
        immuneSystem = FindFirstObjectByType<ImmuneSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRecruitingInfo();
        UpdateUpgradeInfo();
        UpdatePoliceCounters();
    }

    public void UpdateRecruitingInfo()
    {
        recruitButton.interactable = immuneSystem.CanBuyPolice();
        recruitPrice.text = "" + immuneSystem.PoliceCost;
    }
    
    public void UpdateUpgradeInfo()
    {
        upgradeButton.interactable = immuneSystem.CanBuyUpgrade();
        upgradePrice.text = "" + immuneSystem.UpgradeCost;
    }

    public void UpdatePoliceCounters()
    {
        increaseButton.interactable = immuneSystem.CanIncreasePatrol();
        decreaseButton.interactable = immuneSystem.CanDecreasePatrol();
        policeCount.text = "" + BeanManager.Instance.PoliceBeans + "/" + immuneSystem.MaxPolice;
        patrolCount.text = "" + BeanManager.Instance.PatrollingBeans + "/" + BeanManager.Instance.PoliceBeans;
    }
}
