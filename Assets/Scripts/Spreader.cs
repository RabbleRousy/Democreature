using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Spreader : MonoBehaviour
    {
        [SerializeField] private float range;
        [SerializeField] private float value;
        [SerializeField] private GameObject vfxPrefab;

        private GameObject vfxInstance;
        private SpriteRenderer vfxRenderer;

        public float Value
        {
            get => value;
            set => this.value = value;
        }

        public float Range
        {
            get => range;
            set => range = value;
        }

        public float CorruptionChance { get; set; }
        public bool UseCorruptionChance { get; set; }


        private void OnEnable()
        {
            GameManager.Instance.OnTick += Spread;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnTick -= Spread;
        }

        private void Spread()
        {
           Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
           StartVfx();
           foreach (Collider2D collider in colliders)
           {
               if (collider.gameObject.TryGetComponent(out ICorruptible corruptible))
               {
                   if (UseCorruptionChance)
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

        private void StartVfx()
        {
            if (vfxInstance == null)
            {
                vfxInstance = Instantiate(vfxPrefab, transform.position,Quaternion.Euler(Vector3.zero),transform);
                vfxRenderer = vfxInstance.GetComponent<SpriteRenderer>();
                vfxInstance.transform.localScale = Vector3.zero;
                
            }

            Sequence seq = DOTween.Sequence();
            seq.Append(vfxInstance.transform.DOScale(Vector3.one * range, 0.25f));
            seq.Join(vfxRenderer.DOFade(1, 0.5f));
            seq.Append(vfxInstance.transform.DOScale(Vector3.zero, 0.25f));
            seq.Join(vfxRenderer.DOFade(0f, 0.25f));


        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.coral;
            Gizmos.DrawWireSphere(transform.position,range);
        }
    }
}