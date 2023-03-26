using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer grassObj;
    public Transform resSourceCount;
    public static float resourcesGathered=0;

    public AudioSource wind;
    public ParticleSystem windParticles;

    public int minParticles = 2;
    public int maxParticles = 20;

    public Color liveGrass;
    public Color driedGrass;

    int totalRes;

    // Start is called before the first frame update
    void Start()
    {
        totalRes = GetResCount();
    }

    // Update is called once per frame
    void Update()
    {
        resourcesGathered = 1 - ((float) GetResCount() / (float) totalRes);

        grassObj.color = Color.Lerp(liveGrass, driedGrass, resourcesGathered);

        wind.volume = resourcesGathered;

        ParticleSystem.EmissionModule emission = windParticles.emission;
        emission.rateOverTime = Mathf.Lerp(minParticles, maxParticles, resourcesGathered);
    }

    int GetResCount()
    {
        return resSourceCount.childCount;
    }
}
