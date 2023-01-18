using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    public Vector3 velocity;
    float height;
    float heightVelocity = 3;

    float ground;

    [SerializeField]
    float startingSpeed = 4;

    [SerializeField]
    float gravity = 10;

    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.flipX = Random.value > 0.5;
        ground = sprite.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        heightVelocity -= gravity * Time.deltaTime;
        height += heightVelocity * Time.deltaTime;

        if (height < ground) height = ground;

        sprite.transform.localPosition = new Vector3(0, height);

        transform.Translate(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        velocity *= 0.95f;
    }

    public void SetVelocity(Vector3 dir)
    {
        velocity = dir * -1;
        velocity.Normalize();

        velocity = Quaternion.Euler(0, 0, Random.Range(-45, 45)) * velocity;

        velocity *= Random.Range(0, startingSpeed);
    }
}
