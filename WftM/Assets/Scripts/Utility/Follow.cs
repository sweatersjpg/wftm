using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform toFollow;
    [SerializeField]
    float followSpeed;

    private void FixedUpdate()
    {
        float oldz = transform.position.z;
        transform.Translate((toFollow.position - transform.position) / followSpeed);

        transform.position = new Vector3(transform.position.x, transform.position.y, oldz);
    }
}
