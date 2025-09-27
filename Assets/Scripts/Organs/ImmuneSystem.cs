using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Organs
{
    public class ImmuneSystem : MonoBehaviour
    {
        [SerializeField] private int maxUpgrades;
        [SerializeField] private int policePerUpgrade;
        [SerializeField] private float baseCorruptionChance;
        [SerializeField] private float decayPerSecond;
        [SerializeField] private float corruptionPerBuy;
        [SerializeField] private int bloodCostPolice;
        [SerializeField] private int bloodCostUpgrade;
        
        [SerializeField] private PatrolTarget patrolStartTarget;


        private float currentCorruptionChance;
        private int currentUpgrades = 1;


        private void Update()
        {
            currentCorruptionChance -= decayPerSecond * Time.deltaTime;
            currentCorruptionChance = Mathf.Clamp(currentCorruptionChance, baseCorruptionChance, 1);
            
        }


        public void IncreasePatrol()
        {
            BeanManager.Instance.TryIncreasePatrol(patrolStartTarget);
        }

        public void DecreasePatrol()
        {
            BeanManager.Instance.DecreasePatrol();
        }

        public void Buy()
        {
            if(GameManager.Instance.Blood < bloodCostPolice || BeanManager.Instance.PoliceBeans >= policePerUpgrade * currentUpgrades) return;
            
            bool ShouldBeCorrupted = Random.Range(0f, 1f) <= currentCorruptionChance;
            if (BeanManager.Instance.TryCreatePoliceBean(ShouldBeCorrupted))
            {
                GameManager.Instance.Blood -= bloodCostPolice;
            }
            currentCorruptionChance += corruptionPerBuy;
        }

        public void UpgradeLimit()
        {
            if (GameManager.Instance.Blood >= bloodCostUpgrade && currentUpgrades < maxUpgrades)
            {
                currentUpgrades++;
                GameManager.Instance.Blood -= bloodCostUpgrade;
            }
        }
    }
}