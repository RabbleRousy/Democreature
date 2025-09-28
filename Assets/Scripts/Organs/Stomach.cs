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
    [SerializeField] private float minInfluence = -0.5f;
    [SerializeField] private float maxInfluence = 0.5f;
    [SerializeField] private GameObject[] foodPrefabsPositive;
    [SerializeField] private GameObject[] foodPrefabsNegative;
    [SerializeField] private Transform[] foodSpawns;

    [SerializeField] private int factCheckerCost;
    [SerializeField] private int stationCost;

    private List<Transform> availablePointsBrain;
    private List<Transform> availablePointsHeart;
    private List<Transform> availablePointsStomach;
    private List<Transform> availableFoodSpawns;

    private List<Spreader> stations;

    private int currentFactChecker;
    private int foodSpawnCounter;

    public bool CanBuyHeartStation => GameManager.Instance.Blood >= stationCost && availablePointsHeart.Count > 0;
    public bool CanBuyBrainStation => GameManager.Instance.Blood >= stationCost && availablePointsBrain.Count > 0;
    public bool CanBuyStomachStation => GameManager.Instance.Blood >= stationCost && availablePointsStomach.Count > 0;

    public bool CanBuyFactChecker =>
        GameManager.Instance.Blood >= factCheckerCost && currentFactChecker < maxFactChecker;

    public int FactCheckerCost => factCheckerCost;
    public int StationCost => stationCost;
    
    public int MaxFactCheckers => maxFactChecker;
    public int FactCheckerCount => currentFactChecker;


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
        CreatStation(ref availablePointsStomach,true);
        CreatStation(ref availablePointsStomach,true);
    }

    private void Tick()
    {
        foodSpawnCounter--;
        if (foodSpawnCounter <= 0)
        {
            foodSpawnCounter = Random.Range(minFoodSpawnInterval, maxFoodSpawnInterval + 1);
            StartCoroutine(SpawnFood());
        }
    }

    public void BuyFactChecker()
    {
        if (CanBuyFactChecker)
        {
            currentFactChecker++;
            GameManager.Instance.Blood -= factCheckerCost;
        }
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

    private void CreatStation(ref List<Transform> points, bool ignorecost = false)
    {
        if (points.Count <= 0 || (GameManager.Instance.Blood < stationCost && !ignorecost)) return;
        int index = Random.Range(0, points.Count);
        Vector3 position = points[index].position;
        points.RemoveAt(index);

        Spreader spreader = Instantiate(stationPrefab, position, Quaternion.identity);
        stations.Add(spreader);
        if(!ignorecost) GameManager.Instance.Blood -= stationCost;
    }

    private IEnumerator SpawnFood()
    {
        int foodAmount = Random.Range(minFood, maxFood + 1);
        availableFoodSpawns = new List<Transform>(foodSpawns);
        float totalInfluence = 0;
        int bacteriaPerFood = Mathf.Max(1, Mathf.CeilToInt(currentFactChecker / (float)foodAmount)); 
        for (int i = 0; i < foodAmount; i++)
        {
            if (availableFoodSpawns.Count <= 0)
            {
                availableFoodSpawns = new List<Transform>(foodSpawns);
            }

            int index = Random.Range(0, availableFoodSpawns.Count);

            float currentInfluence = Random.Range(minInfluence, maxInfluence);
            GameObject[] prefabs = currentInfluence < 0 ? foodPrefabsPositive : foodPrefabsNegative;

            Instantiate(prefabs[Random.Range(0, prefabs.Length)], availableFoodSpawns[index].position,
                Quaternion.identity).GetComponent<FoodVisual>().SetBacteriaAmount(bacteriaPerFood);
            availableFoodSpawns.RemoveAt(index);

            totalInfluence += currentInfluence;

            yield return new WaitForSeconds(0.5f);
        }

        totalInfluence /= foodAmount;

        if (totalInfluence > 0)
        {
            totalInfluence *= 1 - (currentFactChecker / (float)maxFactChecker);
        }

        UpdateStations(totalInfluence);
    }

    private void UpdateStations(float value)
    {
        foreach (Spreader spreader in stations)
        {
            spreader.Value = value;
        }
    }
}