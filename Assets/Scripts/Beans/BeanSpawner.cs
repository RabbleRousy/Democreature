using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeanSpawner : MonoBehaviour
{
    private AreaManager areaManager;

    [SerializeField] private BeanMovement beanToSpawn;
    [SerializeField] private int minAmountPerTick, maxAmountPerTick;
    [SerializeField] private AreaManager[] possibleTargets;

    private void Awake()
    {
        areaManager = GetComponent<AreaManager>();
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
            Vector3 pos = areaManager.GetRandomPoint();
            var bean = Instantiate(beanToSpawn, pos, Quaternion.identity);
            bean.InitializeTargets(possibleTargets.ToList());
        }
    }
}
