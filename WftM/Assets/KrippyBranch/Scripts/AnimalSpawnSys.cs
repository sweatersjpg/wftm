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

    public int currentWolfCount;
    public int currentBunnyCount;

    private List<GameObject> wolfList = new List<GameObject>();
    private List<GameObject> bunnyList = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating("SpawnAnimals", 5f, 20f);
    }

    private void SpawnAnimals()
    {
        if (currentWolfCount < maxWolfCount)
        {
            int wolfSpawnIndex = Random.Range(0, wolfSPs.Length);
            Transform wolfSpawnPoint = wolfSPs[wolfSpawnIndex];

            GameObject newWolf = Instantiate(WolfPrefab, wolfSpawnPoint.position, Quaternion.identity);
            newWolf.transform.parent = wolfSpawnPoint.transform;
            wolfList.Add(newWolf);

            currentWolfCount++;
        }

        if (currentBunnyCount < maxBunnyCount)
        {
            int bunnySpawnIndex = Random.Range(0, bunnySPs.Length);
            Transform bunnySpawnPoint = bunnySPs[bunnySpawnIndex];

            GameObject newBunny = Instantiate(BunnyPrefab, bunnySpawnPoint.position, Quaternion.identity);
            newBunny.transform.parent = bunnySpawnPoint.transform;
            bunnyList.Add(newBunny);

            currentBunnyCount++;
        }
    }
}
