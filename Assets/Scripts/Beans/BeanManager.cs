using System;
using UnityEngine;

public class BeanManager : MonoBehaviour
{
    public static BeanManager Instance;

    [SerializeField] private uint symbiontAmount, parasiteAmount;

    private void Awake()
    {
        Instance = this;
    }

    public float GetParasitePercentage()
    {
        return (float)parasiteAmount / symbiontAmount;
    }

    public void AddSymbiont() => symbiontAmount++;
    public void RemoveSymbiont() => symbiontAmount--;
    public void AddParasite() => parasiteAmount++;
    public void RemoveParasite() => parasiteAmount--;

    public void ConvertToSymbiont()
    {
        symbiontAmount++;
        parasiteAmount--;
    }

    public void ConvertToParasite()
    {
        parasiteAmount++;
        symbiontAmount--;
    }
}
