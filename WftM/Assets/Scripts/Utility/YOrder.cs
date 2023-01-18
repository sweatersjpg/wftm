using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YOrder : MonoBehaviour
{
    SpriteRenderer[] srs;

    private void Start()
    {
        srs = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float d = transform.position.y - Camera.main.transform.position.y;

        foreach(SpriteRenderer sr in srs)
        {
            sr.sortingOrder = (int) (-d * 100);
        }

        //transform.position = new Vector3(transform.position.x, transform.position.y, d);
    }
}
