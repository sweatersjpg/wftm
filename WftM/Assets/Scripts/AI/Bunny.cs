using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    private enum BunStates { idle, wander, flee }
    private BunStates bState;

    private Animal animalControl;

    private GameObject home;

    private float actionTime = 3;
    private float currentTime = 0;

    Transform floorSprite;
    private float stateTimer = 0f;

    bool flee;

    private void Start()
    {
        floorSprite = GameObject.FindGameObjectWithTag("Floor").transform;

        animalControl = GetComponent<Animal>();


        if (animalControl == null)
        {
            Debug.LogError("Missing Dependancy Script: Animal");
            return;
        }

        bState = BunStates.idle;


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
        if (rnd == 0) bState = BunStates.idle;
        else bState = BunStates.wander;

        currentTime = 0;
    }

    private void AlternateState()
    {
        if (bState == BunStates.idle)
        {
            bState = BunStates.wander;
        }
        else if (bState == BunStates.wander)
        {
            bState = BunStates.idle;
        }
    }

    private void StateManage()
    {
        if (animalControl.GetPredator() && !flee)
        {
            bState = BunStates.flee;
            flee = true;
        }
        else if (!animalControl.GetPredator() && flee)
        {
            RandomizeState();
            flee = false;
        }

        switch (bState)
        {
            case BunStates.idle:
                animalControl.Idle();
                if (stateTimer >= actionTime)
                {
                    RandomizeState();
                    stateTimer = 0f;
                }
                break;
            case BunStates.wander:
                animalControl.Wander(floorSprite);
                if (stateTimer >= actionTime)
                {
                    RandomizeState();
                    stateTimer = 0f;
                }
                break;
            case BunStates.flee:
                animalControl.Flee();
                break;
        }

        stateTimer += Time.deltaTime;
    }

}
