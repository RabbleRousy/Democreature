using System;
using UnityEngine;

public class PlaySoundAfterSeconds : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource toStop;
    [SerializeField] private float time;

    private bool played;
    private void Update()
    {
        if(played)return;
        
        time -= Time.deltaTime;
        if (time <= 0)
        {
            source.Play();
            played = true;
            toStop.Stop();
        }
    }
}
