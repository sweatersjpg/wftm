using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLink : MonoBehaviour
{
    GameObject linked;
    bool hasBeenLinked;

    // Update is called once per frame
    void Update()
    {
        if (hasBeenLinked && linked == null) Destroy(gameObject);
    }

    public void MakeLink(GameObject g)
    {
        linked = g;
        hasBeenLinked = true;
    }
}
