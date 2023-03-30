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
        InvokeRepeating("SpawnAnimals", 0f, 5f);
    }

    private void SpawnAnimals()
    {
        if (currentWolfCount < maxWolfCount)
        {
            int wolfSpawnIndex = Random.Range(0, wolfSPs.Length);
            Transform wolfSpawnPoint = wolfSPs[wolfSpawnIndex];

            GameObject newWolf = Instantiate(WolfPrefab, wolfSpawnPoint.position, Quaternion.identity);
            wolfList.Add(newWolf);

            currentWolfCount++;
        }

        if (currentBunnyCount < maxBunnyCount)
        {
            int bunnySpawnIndex = Random.Range(0, bunnySPs.Length);
            Transform bunnySpawnPoint = bunnySPs[bunnySpawnIndex];

            GameObject newBunny = Instantiate(BunnyPrefab, bunnySpawnPoint.position, Quaternion.identity);
            bunnyList.Add(newBunny);

            currentBunnyCount++;
        }
    }
}
