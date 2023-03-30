using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    private enum WolfStates { idle, wander, hunt, flee }
    private WolfStates wState;

    private Animal animalControl;

    private GameObject home;

    private float actionTime = 3;
    private float currentTime = 0;

    Transform floorSprite;

    private void Start()
    {
        floorSprite = GameObject.FindGameObjectWithTag("Floor").transform;

        animalControl = GetComponent<Animal>();
  

        if (animalControl == null)
        {
            Debug.LogError("Missing Dependancy Script: Animal");
            return;
        }

        wState = WolfStates.idle;
    }

    private void Update()
    {
        StateManage();
        

        if (currentTime >= actionTime)
        {
            RandomizeState();
        }
    }

    private void RandomizeState()
    {
        int rnd = Random.Range(0, 2);
        if (rnd == 0) wState = WolfStates.idle;
        else wState = WolfStates.wander;

        currentTime = 0;
    }

    private void AlternateState()
    {
        if (wState == WolfStates.idle)
        {
            wState = WolfStates.wander;
        }
        else if (wState == WolfStates.wander)
        {
            wState = WolfStates.idle;
        }
    }

    private void StateManage()
    {
        switch(wState)
        {
            case WolfStates.idle:
                animalControl.Idle();
                actionTime = Random.Range(0.2f, 0.5f);
                currentTime += Time.deltaTime;
                break;
            case WolfStates.wander:

                animalControl.Wander(floorSprite);
                actionTime = Random.Range(0.5f, 0.75f);
                currentTime += Time.deltaTime;
                break;
            case WolfStates.hunt:
                animalControl.Hunt();
                break;
        }

        if (animalControl.GetPrey())
        {
            wState = WolfStates.hunt;
        }
        else
        {
            if (wState == WolfStates.hunt)
            {
                RandomizeState();
            }
        }
    }

  
   
  

   
}
