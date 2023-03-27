using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pond : MonoBehaviour
{
    [SerializeField] private int fishCount = 8;
    private int currentCount;

    [SerializeField] private Vector2 timeRange = new Vector2(0.5f, 5f);
    private float timeToCatch = 0;

    [SerializeField] private float catchWindow = 1f;

    private bool active = false;

    private float time = 0;
    private bool canCatch = false;

    
    private Breakable breakable;
    private SpriteRenderer sr;

    //player casts their line then is in fishing mode
    //player can interact with this script by casting or pulling only
    private void Start()
    {
        currentCount = fishCount;
        breakable = GetComponent<Breakable>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (active)
        {
            time += Time.deltaTime;

            if (time >= timeToCatch)
            {
                StartCoroutine(FishHooked());
                active = false;
            }
        }

        if (canCatch)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.blue;
        }
    }

    public void Cast()
    {
        active = true;
        time = 0;
        timeToCatch = Random.Range(timeRange.x, timeRange.y);
    }

    public void Reel()
    {
        active = false;
        time = 0;

        if (canCatch)
        {
            breakable.DoHit(1);
            fishCount--;
        }

    }


    private IEnumerator FishHooked()
    {
        canCatch = true;

        yield return new WaitForSeconds(catchWindow);

        canCatch = false;

        time = 0;
        timeToCatch = Random.Range(timeRange.x, timeRange.y);

    }
}

