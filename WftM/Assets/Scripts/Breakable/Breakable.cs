using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField]
    public GameObject itemDrop;

    public Sprite tool;

    public float itemsDropChance = 0.8f;
    public int itemsOnHit = 1;
    public int itemsOnDestroy = 3;
    public float health = 3;

    [SerializeField]
    AnimationCurve Wobble;

    [SerializeField]
    float WobbleDuration = 0.5f;
    float WobbleStart = 0;

    [SerializeField]
    bool ignoreFlip = false;

    [SerializeField]
    bool breakable = true;

    [SerializeField]
    bool hasSFX = false;

    public PlayerController.actionType toolType;

    private void Awake()
    {
        if(!ignoreFlip) GetComponentInChildren<SpriteRenderer>().flipX = Random.value > 0.5;
    }

    private void Update()
    {
        transform.localScale = Vector3.LerpUnclamped(new Vector3(1, 0, 1), new Vector3(1, 1, 1), Wobble.Evaluate((Time.time-WobbleStart)/WobbleDuration));
    }

    public void DoHit(float damage)
    {
        WobbleStart = Time.time;

        if (breakable) health -= damage;

        float chance = (damage * itemsDropChance) / Mathf.Ceil(damage);
        DropItems((int)Mathf.Ceil(damage) * itemsOnHit, chance);
        if (hasSFX) gameObject.SendMessage("PlaySFX");

        if (health < 0)
        {
            DropItems(itemsOnDestroy, itemsDropChance);

            if (hasSFX)
            {
                enabled = false;
                gameObject.layer = LayerMask.GetMask("Default");
                gameObject.SendMessage("FinalSFX", true);
            } else Destroy(gameObject);
        }
    }

    void DropItems(int n, float dropChance)
    {
        if (itemDrop == null) return;
        for (int i = 0; i < n; i++)
        {
            if (Random.value > dropChance) continue;

            GameObject item = Instantiate(itemDrop);

            // this is super stupid but it works fow now
            Transform player = Camera.main.GetComponent<Follow>().toFollow.parent;

            item.transform.position = transform.position + (transform.position - player.position).normalized;

            item.SendMessage("SetVelocity", player.position - transform.position);
        }
    }
}
