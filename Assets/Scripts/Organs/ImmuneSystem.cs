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

        [Header("Patrol Order"), SerializeField]
        private List<BeanMovement> patrollingBeans;
        [SerializeField] private int patrolsWanted;
        [SerializeField] private PatrolTarget patrolStartTarget;


        private float currentCorruptionChance;
        private int currentUpgrades = 1;

        public static ImmuneSystem Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        
        public void AddPatrollingBean(BeanMovement bean) => patrollingBeans.Add(bean);
        public void RemovePatrollingBean(BeanMovement bean) => patrollingBeans.Remove(bean);


        private void Update()
        {
            currentCorruptionChance -= decayPerSecond * Time.deltaTime;
            currentCorruptionChance = Mathf.Clamp(currentCorruptionChance, baseCorruptionChance, 1);

            if (patrolsWanted != patrollingBeans.Count)
                UpdatePatrols();
        }

        private void UpdatePatrols()
        {
            if (patrolsWanted > patrollingBeans.Count)
            {
                BeanManager.Instance.IncreasePatrol(patrolStartTarget);
            }
            else
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