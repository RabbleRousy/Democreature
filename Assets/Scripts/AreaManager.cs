using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AreaManager : MonoBehaviour
{
    private BoxCollider2D[] areas;

    private void Awake()
    {
        areas = GetComponentsInChildren<BoxCollider2D>();
    }

    public Vector3 GetRandomPoint()
    {
        var area = areas[Random.Range(0, areas.Length)];
        return area.bounds.min + new Vector3(Random.Range(0, area.bounds.size.x), Random.Range(0, area.bounds.size.y), 0f);
    }
}
