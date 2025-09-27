using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DotColorizer : MonoBehaviour
{
    private SpriteRenderer[] dots;
    [SerializeField] private Color sourColor, sweetColor, clearColor;
    public static DotColorizer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        dots = GetComponentsInChildren<SpriteRenderer>();
    }

    [ContextMenu("UpdateDots")]
    public void UpdateDots()
    {
        float sourPerc = GameManager.Instance.HeartCorruption;
        int limit = (int)(dots.Length * sourPerc);

        StartCoroutine(Repaint(limit));
    }

    private IEnumerator Repaint(int limit)
    {
        yield return ClearToLeft();
        yield return new WaitForSeconds(0.5f);
        yield return FillFromLeft(limit);
    }
    
    // Turns everything sour
    private IEnumerator ClearToLeft()
    {
        for (int i = dots.Length - 1; i >= 0; i--)
        {
            dots[i].color = clearColor;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator FillFromLeft(int limit)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].color = i < limit ? sourColor : sweetColor;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
