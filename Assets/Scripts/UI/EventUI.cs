using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour
{
    [SerializeField] private TMP_Text title, description;
    [SerializeField] private Image img;
    private Animator animator;
    private AudioSource source;
    private bool waitingForClose;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public void Show(string eventName, string eventDescription, Sprite icon, float time = 0f)
    {
        title.text = eventName;
        description.text = eventDescription;
        img.sprite = icon;
        animator.SetTrigger("Open");
        source.Play();

        if (time > 0f)
            StartCoroutine(HideAfterSeconds(time));
    }

    private IEnumerator HideAfterSeconds(float time)
    {
        waitingForClose = true;
        yield return new WaitForSeconds(time);
        waitingForClose = false;
        Hide();
    }
    
    public void Hide()
    {
        if (waitingForClose) return;
        animator.SetTrigger("Close");
    }
}
