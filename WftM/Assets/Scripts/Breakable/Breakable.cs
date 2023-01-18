using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField]
    GameObject itemDrop;

    public Sprite tool;

    public float itemsDropChance = 0.8f;
    public int itemsOnHit = 1;
    public int itemsOnDestroy = 3;
    public int health = 3;

    [SerializeField]
    AnimationCurve Wobble;

    [SerializeField]
    float WobbleDuration = 0.5f;
    float WobbleStart = 0;

    [SerializeField]
    bool ignoreFlip = false;

    private void Awake()
    {
        if(!ignoreFlip) GetComponentInChildren<SpriteRenderer>().flipX = Random.value > 0.5;
    }

    private void Update()
    {
        transform.localScale = Vector3.LerpUnclamped(new Vector3(1, 0, 1), new Vector3(1, 1, 1), Wobble.Evaluate((Time.time-WobbleStart)/WobbleDuration));
    }

    // -- stright up does not work >:(

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision.gameObject.name);

    //    if (collision.transform.CompareTag("ChopZone"))
    //    {
    //        DoHit();
    //    }
    //}

    public void DoHit()
    {
        WobbleStart = Time.time;

        health--;

        if (health < 0)
        {
            DropItems(itemsOnDestroy);
            Destroy(gameObject);
        } else
        {
            DropItems(itemsOnHit);
        }
    }

    void DropItems(int n)
    {
        if (itemDrop == null) return;
        for (int i = 0; i < n; i++)
        {
            if (Random.value > itemsDropChance) continue;

            GameObject item = Instantiate(itemDrop);

            // this is super stupid but it works fow now
            Transform player = Camera.main.GetComponent<Follow>().toFollow.parent;

            item.transform.position = transform.position + (transform.position - player.position).normalized;

            item.SendMessage("SetVelocity", player.position - transform.position);
        }
    }
}
