using System;
using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnClick : MonoBehaviour
{
    private AudioSource audioSource;
    private Button btn;
    

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(Play);
    }

    private void OnDisable()
    {
        btn.onClick.RemoveListener(Play);
    }

    private void Play()
    {
        audioSource.Play();
    }
}
