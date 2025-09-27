using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeanSpawner : MonoBehaviour
{
    private BoxCollider2D[] spawnAreas;

    [SerializeField] private BeanCorruption beanToSpawn;
    [SerializeField] private int minAmountPerTick, maxAmountPerTick;

    private void Awake()
    {
        spawnAreas = GetComponentsInChildren<BoxCollider2D>();
    }

    public void OnEnable()
    {
        GameManager.Instance.OnTick += Spawn;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnTick -= Spawn;
    }

    private void Spawn()
    {
        int amount = Random.Range(minAmountPerTick, maxAmountPerTick + 1);
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = GetRandomSpawnPos();
            Instantiate(beanToSpawn, pos, Quaternion.identity);
        }
    }

    private Vector3 GetRandomSpawnPos()
    {
        var area = spawnAreas[Random.Range(0, spawnAreas.Length)];
        return area.bounds.min + new Vector3(Random.Range(0, area.bounds.size.x), Random.Range(0, area.bounds.size.y), 0f);
    }
}
