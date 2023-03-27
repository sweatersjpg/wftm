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



    private void Start()
    {
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
        switch (bState)
        {
            case BunStates.idle:
                animalControl.Idle();
                actionTime = Random.Range(1f, 3f);
                currentTime += Time.deltaTime;
                break;
            case BunStates.wander:

                animalControl.Wander();
                actionTime = Random.Range(0.6f, 1.2f);
                currentTime += Time.deltaTime;
                break;
            case BunStates.flee:
                animalControl.Flee();
                break;
        }

        if (animalControl.GetPredator())
        {
            bState = BunStates.flee;
        }
        else RandomizeState();


    
    }




}
