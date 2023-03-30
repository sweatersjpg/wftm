using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawnSys : MonoBehaviour
{
    [SerializeField] GameObject WolfPrefab;
    [SerializeField] GameObject BunnyPrefab;

    [SerializeField] Transform[] wolfSPs;
    [SerializeField] Transform[] bunnySPs;

    [SerializeField] int maxWolfCount;
    [SerializeField] int maxBunnyCount;

    private int currentWolfCount;
    private int currentBunnyCount;

    private List<GameObject> wolfList = new List<GameObject>();
    private List<GameObject> bunnyList = new List<GameObject>();

    private void Start()
    {
        SpawnAnimals();
    }

    private void Update()
    {
        SpawnAnimals();
    }

    private void SpawnAnimals()
    {
        
    }
}
