using System;
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

    private List<Transform> availablePointsBrain;
    private List<Transform> availablePointsHeart;
    private List<Transform> availablePointsStomach;

    private List<Spreader> stations;


    private void Awake()
    {
        availablePointsBrain = new List<Transform>(stationPointsBrain);
        availablePointsHeart = new List<Transform>(stationPointsHeart);
        availablePointsStomach = new List<Transform>(stationPointsStomach);
        stations = new List<Spreader>();
    }


    private void CreatStation(ref List<Transform> points)
    {
        int index = Random.Range(0, points.Count);
        Vector3 position = points[index].position;
        points.RemoveAt(index);

        Spreader spreader = Instantiate(stationPrefab, position, Quaternion.identity);
        stations.Add(spreader);
    }


}
