using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{

    [SerializeField] private float speed = 250;
    private float tempSpeed = 0;
    [SerializeField] private float acceleration = 10f;

    private GameObject habitat;
    private Rigidbody2D rb;
    [SerializeField] private float detectRange = 4;

    public enum animalType { predator, prey }
    [SerializeField] public animalType type;

    private GameObject prey;
    private GameObject predator;

    private Vector2 moveDir;
    private float timeTemp = 0;
    private float time = 4;
    [SerializeField] private float attackRadius = 2;
    [SerializeField] private float attackDmg = 1;


    private SpriteRenderer sr;
    private bool attacking = false;

    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (moveDir.x > 0) sr.flipX = false;
        if (moveDir.x < 0) sr.flipX = true;

        if (moveDir == Vector2.zero)
        {
            tempSpeed = 0;
        }

        if (tempSpeed < speed)
        {
            tempSpeed += acceleration * Time.deltaTime;

        }

        if (moveDir != Vector2.zero && speed > 0 && !attacking)
        {
            anim.SetInteger("index", 1);
        }

        tempSpeed = Mathf.Clamp(tempSpeed, 0, speed);
    }

    public void TakeHit(GameObject attacker)
    {
        if (type == animalType.prey)
        {
            predator = attacker;

        }
        else
        {
            prey = attacker;
        }

        GetComponentInChildren<Breakable>().DoHit(1);
    }

    public void Hunt()
    {
        if (attacking == false && prey != null)
        {
            moveDir = prey.transform.position - transform.position;

        }

        Attack();
    }

    public void Wander(Transform bounds)
    {
        Vector3 center = bounds.position;
        Vector3 size = bounds.localScale;

        Vector3 minPosition = center - size / 2f;
        Vector3 maxPosition = center + size / 2f;

        if (timeTemp < time)
        {
            timeTemp += Time.deltaTime;

            if (moveDir == Vector2.zero)
            {
                Vector2 newDir = new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y)) - (Vector2)transform.position;
                moveDir = newDir.normalized;
            }
        }
        else
        {
            timeTemp = 0;
            time = Random.Range(0.5f, 3f);

            //make sure we don't get no movement in our wander state
            while (moveDir == Vector2.zero)
            {
                Vector2 newDir = new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y)) - (Vector2)transform.position;
                moveDir = newDir.normalized;
            }

        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDir.normalized.x * tempSpeed * Time.fixedDeltaTime, moveDir.normalized.y * tempSpeed * Time.fixedDeltaTime);
    }

    public void Flee()
    {
        moveDir = transform.position - predator.transform.position;
    }

    public void Idle()
    {
        moveDir = Vector2.zero;
        anim.SetInteger("index", 0);
    }

    public void ReturnHome()
    {
        moveDir = transform.position.normalized - habitat.transform.position.normalized;

    }

    public void Attack()
    {
        if (prey != null)
        {
            if (Vector2.Distance(transform.position, prey.transform.position) < 0.5f && !attacking)
            {
                StartCoroutine(AttackSequence());
            }
        }

    }

    private IEnumerator AttackSequence()
    {
        attacking = true;
        moveDir = Vector2.zero;
        anim.SetInteger("index", 2);

        yield return new WaitForSeconds(0.65f);



        if (Physics2D.OverlapCircle(transform.position, attackRadius, LayerMask.GetMask("Prey")))
        {
            Physics2D.OverlapCircle(transform.position, attackRadius, LayerMask.GetMask("Prey")).GetComponentInParent<Animal>().TakeHit(this.gameObject);
        }
        else if (Physics2D.OverlapCircle(transform.position, attackRadius, LayerMask.GetMask("Vulnerable")))
        {
            Physics2D.OverlapCircle(transform.position, attackRadius, LayerMask.GetMask("Vulnerable")).GetComponent<PlayerController>().healthCount -= attackDmg * 10;
        }



        yield return new WaitForSeconds(0.01f);

        anim.SetInteger("index", 1);

        if (prey != null) moveDir = transform.position - prey.transform.position;



        yield return new WaitForSeconds(0.8f);

        attacking = false;

    }

    public bool GetPrey()
    {
        if (prey == null)
        {
            GameObject temp = null;

            //if theres no prey search for one
            if (Physics2D.OverlapCircle(transform.position, detectRange, LayerMask.GetMask("Prey")))
            {
                temp = Physics2D.OverlapCircle(transform.position, detectRange, LayerMask.GetMask("Prey")).gameObject;
            }
            else
            {
                prey = null;
                return false;
            }


            if (temp != null)
            {

                prey = temp;
                return true;
            }
            else
            {
                prey = null;
                return false;
            }


        }
        else
        {
            if (Vector2.Distance(transform.position, prey.transform.position) > detectRange)
            {
                prey = null;
                return false;
            }

            return true;
        }


    }

    public bool GetPredator()
    {
        if (predator == null)
        {
            GameObject temp = null;

            if (Physics2D.OverlapCircle(transform.position, detectRange, LayerMask.GetMask("Predator")))
            {
                temp = Physics2D.OverlapCircle(transform.position, detectRange, LayerMask.GetMask("Predator")).gameObject;
            }

            if (temp != null)
            {
                predator = temp;
                return true;
            }
            else return false;

        }
        else
        {
            if (Vector2.Distance(transform.position, predator.transform.position) > detectRange)
            {
                predator = null;
                return false;
            }

            return true;
        }
    }


}
