using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Spreader : MonoBehaviour
    {
        [SerializeField] private float range;
        [SerializeField] private float value;

        private bool useCorruptionChance;

        public float Value
        {
            get => value;
            set => this.value = value;
        }
        
        public float CorruptionChance { get; set; }
        public bool UseCorruptionChance { get; set; }


        private void Start()
        {
            GameManager.Instance.OnTick += Spread;
        }

        private void Spread()
        {
           Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);

           foreach (Collider2D collider in colliders)
           {
               if (collider.gameObject.TryGetComponent(out ICorruptible corruptible))
               {
                   if (useCorruptionChance)
                   {
                      int direction = Random.Range(0f, 1f) < CorruptionChance ? 1 : -1;
                      corruptible.Corrupt(value * direction);
                   }
                   else
                   {
                       corruptible.Corrupt(value);
                   }
                  
               }
           }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.coral;
            Gizmos.DrawWireSphere(transform.position,range);
        }
    }
}