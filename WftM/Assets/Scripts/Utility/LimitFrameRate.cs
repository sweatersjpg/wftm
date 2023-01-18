using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFrameRate : MonoBehaviour
{
    [SerializeField]
    int frameRate;

    void Start()
    {
        Application.targetFrameRate = frameRate;
    }
}
