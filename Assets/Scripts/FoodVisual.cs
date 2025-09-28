using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodVisual : MonoBehaviour
{
    [SerializeField] private string animName;
    [SerializeField] private float timeTillDissolve = 0.5f;
    [SerializeField] private GameObject bacteria;
    [SerializeField] private AudioClip[] clips;

    private Animator animator;
    private int bacteriaAmount = 4;
    private AudioSource source;

    private void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
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
        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
