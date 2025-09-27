using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stomach : MonoBehaviour
{
    [SerializeField] private Spreader stationPrefab;
    [SerializeField] private Transform[] stationPointsBrain;
    [SerializeField] private Transform[] stationPointsHeart;
    [SerializeField] private Transform[] stationPointsStomach;
    
    [SerializeField] private int maxFactChecker;
    
    
    [SerializeField] private int minFood;
    [SerializeField] private int maxFood;
    [SerializeField] private int minFoodSpawnInterval;
    [SerializeField] private int maxFoodSpawnInterval;
    [SerializeField] private GameObject[] foodPrefabs;
    [SerializeField] private Transform[] foodSpawns;

    private List<Transform> availablePointsBrain;
    private List<Transform> availablePointsHeart;
    private List<Transform> availablePointsStomach;
    private List<Transform> availableFoodSpawns;

    private List<Spreader> stations;

    private int currentFactChecker;
    private int foodSpawnCounter;


    private void Awake()
    {
        availablePointsBrain = new List<Transform>(stationPointsBrain);
        availablePointsHeart = new List<Transform>(stationPointsHeart);
        availablePointsStomach = new List<Transform>(stationPointsStomach);
        availableFoodSpawns = new List<Transform>(foodSpawns);
        stations = new List<Spreader>();
    }

    private void Start()
    {
        GameManager.Instance.OnTick += Tick;
    }

    private void Tick()
    {
        foodSpawnCounter--;
        if (foodSpawnCounter <= 0)
        {
            foodSpawnCounter = Random.Range(minFoodSpawnInterval, maxFoodSpawnInterval+1);
            StartCoroutine(SpawnFood());
        }
    }

    public void BuyFactChecker()
    {
        currentFactChecker++;
    }

    public void CreateStomachStation()
    {
        CreatStation(ref availablePointsStomach);
    }
    
    public void CreateHeartStation()
    {
        CreatStation(ref availablePointsHeart);
    }

    public void CreateBrainStation()
    {
        CreatStation(ref availablePointsBrain);
    }

    private void CreatStation(ref List<Transform> points)
    {
        if(points.Count <= 0) return;
        int index = Random.Range(0, points.Count);
        Vector3 position = points[index].position;
        points.RemoveAt(index);

        Spreader spreader = Instantiate(stationPrefab, position, Quaternion.identity);
        stations.Add(spreader);
    }

    private IEnumerator SpawnFood()
    {
        int foodAmount = Random.Range(minFood, maxFood + 1);
        availableFoodSpawns = new List<Transform>(foodSpawns);
        float totalCorruption = 0;
        for (int i = 0; i < foodAmount; i++)
        {
            int index = Random.Range(0, availableFoodSpawns.Count);
            Instantiate(foodPrefabs[Random.Range(0,foodPrefabs.Length)], availableFoodSpawns[index].position, Quaternion.identity);
            availableFoodSpawns.RemoveAt(index);

            totalCorruption += Random.Range(-1f, 1f);
            
            yield return new WaitForSeconds(0.5f);
        }

        totalCorruption /= foodAmount;

        if (totalCorruption > 0)
        {
            totalCorruption *= 1 - (currentFactChecker / (float)maxFactChecker);
        }
        
        UpdateStations(totalCorruption);
    }

    private void UpdateStations(float value)
    {
        foreach (Spreader spreader in stations)
        {
            spreader.Value = value;
        }
    }
}
