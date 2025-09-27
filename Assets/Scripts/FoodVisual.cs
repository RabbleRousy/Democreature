using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodVisual : MonoBehaviour
{
    [SerializeField] private string animName;
    [SerializeField] private float timeTillDissolve = 0.5f;
    [SerializeField] private GameObject bacteria;

    private Animator animator;
    private int bacteriaAmount = 4;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayAnimation());
    }

    public void SetBacteriaAmount( int amount)
    {
        bacteriaAmount = amount;
    }

    private IEnumerator PlayAnimation()
    {
        for(int i = 0; i < bacteriaAmount; i++)
        {
            GameObject go = Instantiate(bacteria, transform);
            go.transform.localPosition = Random.insideUnitCircle * 0.75f;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(timeTillDissolve);
        
        animator.SetTrigger(animName);
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
