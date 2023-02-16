using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float timeBeforeDeath = 120;

    [SerializeField]
    float maxSpeed = 0.1f;
    [SerializeField]
    float minSpeed = 0.05f;

    float lastFrame;
    [SerializeField]
    float animationFPS = 12;
    int animTic = 0;

    [SerializeField]
    Sprite[] sprites;

    SpriteRenderer sr;
    Rigidbody2D rb;

    [SerializeField]
    float chopDuration = 0.5f;
    float chopStart = -100;
    bool hasChopped = true;

    bool facingRight = false;

    Transform toolPivot;
    SpriteRenderer[] toolSprites;

    Vector2 input;

    [SerializeField]
    float moneyGain = 100;
    [SerializeField]
    float moneySpeed = 9;

    public TextMeshProUGUI rockCountText;
    public TextMeshProUGUI woodCountText;
    public TextMeshProUGUI healthCountText;
    public TextMeshProUGUI foodCountText;
    public TextMeshProUGUI moneyCountText;

    public GameObject moneyIcon;

    [SerializeField]
    float maxItems = 50;

    float rockCount = 0;
    float woodCount = 0;
    float healthCount = 100;
    float foodCount = 0;
    float moneyCount = 0;

    float moneyCountOld = 0;

    [SerializeField]
    Sprite[] resourceItemSprites;

    [SerializeField]
    GameObject foodItem;

    bool repeatHit = false;

    bool gameOver = false;

    bool freezeAnim = false;
    private Pond currentPond;

    //differentiate between fishing action (hold) and chopping action (hit)
    public enum actionType { none, machine, chop, fish }
    [SerializeField]
    public actionType action = actionType.none;

    // Start is called before the first frame update
    void Start()
    {
        animationFPS = 1.0f / animationFPS;
        sr = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        toolPivot = transform.Find("ToolPivot");

        toolSprites = new SpriteRenderer[2];
        toolSprites[0] = toolPivot.Find("ToolUP").GetComponent<SpriteRenderer>();
        toolSprites[1] = toolPivot.Find("ToolDOWN").GetComponent<SpriteRenderer>();

        InvokeRepeating(nameof(DoAnimation), 0, 0.08333f);
    }

    // Update is called once per frame
    void Update()
    {
        if (moneyCountOld != moneyCount)
        {
            moneyCountOld += moneySpeed;
            moneyIcon.GetComponent<Interactable>().DoHit();

            if (moneyCount - moneyCountOld < moneySpeed)
            {
                moneyCountOld = moneyCount;
            }
        }

        if (gameOver)
        {

            if(Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            return;
        }

        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2.ClampMagnitude(input, 1);


        if (Time.time - chopStart > chopDuration && !freezeAnim)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                chopStart = Time.time;
                hasChopped = false;
                repeatHit = true;

                GetTool();
            }
            else if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.E))
            {
                if (GetTool() && repeatHit)
                {
                    chopStart = Time.time;
                    hasChopped = false;
                }
                else repeatHit = false;
            }
            else repeatHit = false;

            //transform.Translate(speed * Time.deltaTime * input);
            //rb.MovePosition(rb.position + speed * Time.deltaTime * input);

            if (input.x > 0.2f) facingRight = true;
            else if (input.x < -0.2f) facingRight = false;
        }

        if(Input.GetKeyDown(KeyCode.Q) && foodCount > 0)
        {
            foodCount -= 1;

            toolPivot.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

            GameObject item = Instantiate(foodItem);

            item.transform.position = toolPivot.Find("hitpoint").position;

            item.SendMessage("SetVelocity", transform.position - toolPivot.Find("hitpoint").position);
        }


        if (freezeAnim)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                
                currentPond.Reel();
                currentPond = null;

                freezeAnim = false;
                action = actionType.none;

            }
        }
        //DoAnimation();

    }

    private void FixedUpdate()
    {
        //float d = rb.position.y - Camera.main.transform.position.y;
        //rb.position = new Vector3(rb.position.x, rb.position.y, d);

        if (Time.time - chopStart > chopDuration)
        {
            float speed = minSpeed + (maxSpeed - minSpeed) * healthCount / 100;

            rb.MovePosition(rb.position + (speed * input * Time.fixedDeltaTime) * 50);
        }
    }

    void DoChop()
    {
        if(!hasChopped)
        {
            RaycastHit2D hit = Physics2D.CircleCast(toolPivot.Find("hitpoint").position, 0.2f, Vector2.up, 0.2f, LayerMask.GetMask("Breakable"));
            if(hit)
            {
                if(hit.transform.CompareTag("Machine"))
                {

                    if (woodCount > rockCount && woodCount >= 5)
                    {
                        woodCount -= 5;
                        hit.transform.SendMessage("DoHit");
                        moneyCount += moneyGain;
                    }
                    else if (rockCount >= 5)
                    {
                        rockCount -= 5;
                        hit.transform.SendMessage("DoHit");
                        moneyCount += moneyGain;
                    }
                }
                else if (hit.transform.CompareTag("Pond"))
                {
                    
                    
                    currentPond = hit.transform.GetComponent<Pond>();
                    currentPond.Cast();

                }
                else hit.transform.SendMessage("DoHit");
            }



            hasChopped = true;
        }
    }

    
    bool GetTool()
    {
        toolPivot.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

        // find correct tool for resource
        RaycastHit2D hit = Physics2D.CircleCast(toolPivot.Find("hitpoint").position, 0.2f, Vector2.up, 0.2f, LayerMask.GetMask("Breakable"));
        if (hit)
        {
            if (hit.transform.CompareTag("Machine"))
            {
                action = actionType.machine;
                if (woodCount > rockCount && woodCount >= 5)
                {
                    toolSprites[0].sprite = resourceItemSprites[0];
                    toolSprites[1].sprite = null;
                }
                else if (rockCount >= 5)
                {
                    toolSprites[0].sprite = resourceItemSprites[1];
                    toolSprites[1].sprite = null;
                }
                else
                {
                    toolSprites[0].sprite = null;
                    toolSprites[1].sprite = null;
                    return false;
                }
            }
            else if (hit.transform.CompareTag("Pond"))
            {
                toolSprites[0].sprite = hit.transform.GetComponent<Interactable>().tool;
                toolSprites[1].sprite = hit.transform.GetComponent<Interactable>().tool;
                action = actionType.fish;
            }
            else
            {
                // hit.transform.SendMessage("DoHit");
                toolSprites[0].sprite = hit.transform.GetComponent<Breakable>().tool;
                toolSprites[1].sprite = hit.transform.GetComponent<Breakable>().tool;
                action = actionType.chop;
            }

            //Debug.Log(hit.transform.name);

            return true;
        }

        toolSprites[0].sprite = null;
        toolSprites[1].sprite = null;
        action = actionType.none;

        return false;
    }

    void DoAnimation()
    {
        Vector2 vel = input;

        // putting this here because i don't like Time.deltaTime
        healthCount -= 100f / timeBeforeDeath / 12f;

        if(healthCount < 0)
        {
            healthCount = 0;
            gameOver = true;
        }

        if(healthCountText.text != Mathf.Floor(healthCount) + "%")
        {
            healthCountText.color = new Color(1, 0, 0);
        }

        UpdateUI();

        if(gameOver)
        {
            sr.sprite = sprites[4];
            return;
        }

        //if (Time.time - lastFrame < animationFPS) return;
        //lastFrame = Time.time;

        animTic++;

        // this is here so it syncs with the animation
        if (Time.time - chopStart > chopDuration / 2) DoChop();

        if (vel.magnitude < 0.2)
        {
            sr.sprite = sprites[0];
            sr.flipX = animTic % 8 < 4;
        } else
        {
            sr.sprite = sprites[1];
            sr.flipX = animTic % 4 < 2;
        }

        //animation should be active
        if(Time.time - chopStart < chopDuration)
        {
            toolPivot.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);

            sr.flipX = !facingRight;

            if (!freezeAnim)
            {
                //upwards motion
                if (Time.time - chopStart < chopDuration / 2)
                {
                    sr.sprite = sprites[2];
                    // enable up tool
                    toolSprites[0].enabled = true;
                    toolSprites[1].enabled = false;

                }
                else //downwards motion
                {
                    sr.sprite = sprites[3];
                    toolSprites[0].enabled = false;
                    toolSprites[1].enabled = true;

                    if (action == actionType.fish)
                    {
                        freezeAnim = true;
                    }
                    
                }
            }
            else
            {
                chopStart = Time.time;

                sr.sprite = sprites[3];
                toolSprites[0].enabled = false;
                toolSprites[1].enabled = true;

            }
           
        } else
        {
            toolSprites[0].enabled = false;
            toolSprites[1].enabled = false;
        }
    }

    void UpdateUI()
    {
        rockCountText.text = rockCount + "";
        woodCountText.text = woodCount + "";
        foodCountText.text = "(q) " + foodCount;
        moneyCountText.text = ((int)moneyCountOld) + "$";

        healthCountText.text = Mathf.Floor(healthCount) + "%";
        if (gameOver) healthCountText.text += " (Click anywhere to restart)";

        LerpColor(rockCountText);
        LerpColor(woodCountText);
        LerpColor(healthCountText);

        

        moneyCountOld = Mathf.Clamp(moneyCountOld, 0f, moneyCount);
    }

    void LerpColor(TextMeshProUGUI tm)
    {
        tm.color = new Color(
            tm.color.r + (1 - tm.color.r) / 8,
            tm.color.g + (1 - tm.color.g) / 8,
            tm.color.b + (1 - tm.color.b) / 8);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pickup"))
        {

            if (collision.gameObject.CompareTag("Wood")) {
                woodCount++;
                //woodCount = Mathf.Min(woodCount, maxItems);
                if(woodCount > maxItems)
                {
                    woodCount = maxItems;
                    woodCountText.color = new Color(1, 0, 0);

                    return; // dont destroy item 
                }
            }
            else if(collision.gameObject.CompareTag("Rock"))
            {
                rockCount++;
                if (rockCount > maxItems)
                {
                    rockCount = maxItems;
                    rockCountText.color = new Color(1, 0, 0);

                    return; // dont destroy item 
                }
            }
            else if (collision.gameObject.CompareTag("Health"))
            {
                healthCount += 5;

                if(healthCount > 100)
                {
                    healthCount -= 5;
                    foodCount++;
                }
            }

            Destroy(collision.gameObject);

        }

        //Debug.Log(collision.gameObject.layer + "," + LayerMask.GetMask("Pickup"));
    }
}
