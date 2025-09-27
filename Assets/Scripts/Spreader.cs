using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Spreader : MonoBehaviour
    {
        [SerializeField] private float range;
        [SerializeField] private float value;

        public float Value
        {
            get => value;
            set => this.value = value;
        }

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
                   corruptible.Corrupt(value);
               }
           }
        }
    }
}