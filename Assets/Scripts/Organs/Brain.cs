using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Organs
{
    public class Brain : MonoBehaviour
    {
        [SerializeField] private int bloodCost;
        [SerializeField] private int minAmount;
        [SerializeField] private int maxAmount;
        

        public void UnPolice()
        {
            if(GameManager.Instance.Blood < bloodCost) return;
            
            int amount = Mathf.FloorToInt(Random.Range(minAmount, maxAmount) * 1 - GameManager.Instance.BrainCorruption);
            for (int i = 0; i < amount; i++)
            {
                BeanManager.Instance.TryUnPolice(Random.Range(0f,1f) < GameManager.Instance.BrainCorruption);
            }
        }
    }
}