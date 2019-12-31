using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    
    public float moveX, moveY;
    public float lastMoveX = 0, lastMoveY = -1;
    public bool moving;
    Vector2 moveVector;

    

    Animator anim;

    

    [HideInInspector]
    public bool _Anim_;

    

    


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("LastMoveX", 0);
        anim.SetFloat("LastMoveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        BasicRules();
        UpdateMovement();
        UpdateControls();
        UpdateClosestObjectInRange(PlayerManager.instance.maxInteractRange);
    }

    void FixedUpdate()
    {
        if(PlayerManager.active)
        {
            GetComponent<Rigidbody2D>().AddForce(moveVector * 750 * PlayerManager.instance.speed * Time.deltaTime, ForceMode2D.Impulse);
        }
        
    }







    void BasicRules()
    {
        if (PlayerManager.health > PlayerManager.maxHealth)
        {
            PlayerManager.health = PlayerManager.maxHealth;
        }
    }

    public void UpdateMovement()
    {
        if(PlayerManager.active)
        {
            if(PlayerManager.instance.currentPlayerState == PlayerManager.PlayerState.Idle || PlayerManager.instance.currentPlayerState == PlayerManager.PlayerState.Moving)
            {
                if(PlayerManager.instance.dashCooldown > 0)
                {
                    PlayerManager.instance.dashCooldown -= Time.deltaTime;
                } else
                {
                    PlayerManager.instance.dashCooldown = 0;
                }

                moveX = Input.GetAxisRaw("Horizontal");
                moveY = Input.GetAxisRaw("Vertical");
                anim.SetFloat("MoveX", moveX);
                anim.SetFloat("MoveY", moveY);
                if (moveX != 0 || moveY != 0)
                {
                    moving = true;
                    PlayerManager.instance.currentPlayerState = PlayerManager.PlayerState.Moving;
                }
                else
                {
                    moving = false;
                    PlayerManager.instance.currentPlayerState = PlayerManager.PlayerState.Idle;
                }
                anim.SetBool("Moving", moving);
                if (moving)
                {
                    lastMoveX = moveX;
                    lastMoveY = moveY;
                    anim.SetFloat("LastMoveX", lastMoveX);
                    anim.SetFloat("LastMoveY", lastMoveY);
                }

                moveVector = new Vector2(moveX, moveY).normalized;
            } else if(PlayerManager.instance.currentPlayerState == PlayerManager.PlayerState.Dashing && GetComponent<Rigidbody2D>().velocity == Vector2.zero)
            {
                PlayerManager.instance.currentPlayerState = PlayerManager.PlayerState.Idle;
            }
        }
        
    }

    public void UpdateClosestObjectInRange(float _range)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 selectorPos = Vector2.Lerp(transform.position, mousePos, 0.5f);
        GameObject closestObjectInRange = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Interactable"))
        {
            if (obj.GetComponent<Interactable>() != null)
            {
                if (closestObjectInRange == null)
                {
                    if (Vector2.Distance(transform.position, obj.transform.position) < _range)
                    {
                        closestObjectInRange = obj;
                    }
                }
                else if (Vector2.Distance(selectorPos, obj.transform.position) < Vector2.Distance(selectorPos, closestObjectInRange.transform.position) && Vector2.Distance(transform.position, obj.transform.position) < _range)
                {
                    closestObjectInRange = obj;
                }
            }
        }

        PlayerManager.instance.playerClosestObjectInRange = closestObjectInRange;
    }


    public void UpdateControls()
    {
        if (PlayerManager.active)
        {
            if (Input.GetButtonDown("Use"))
            {
                if (PlayerManager.instance.playerClosestObjectInRange != null)
                {
                    if (PlayerManager.instance.playerClosestObjectInRange.GetComponent<Interactable>() != null)
                    {
                        if (PlayerManager.instance.playerClosestObjectInRange.GetComponent<Interactable>().active)
                        {
                            PlayerManager.instance.playerClosestObjectInRange.GetComponent<Interactable>().OnInteractedWith();
                        }

                    }

                }

            }

            if(Input.GetMouseButtonDown(1))
            {
                if(PlayerManager.instance.dashCooldown <= 0)
                {
                    StartCoroutine(Dash(new Vector2(lastMoveX, lastMoveY), PlayerManager.dashPower));
                }
               
            }

        }
            
    }


    public IEnumerator Dash(Vector3 _normedDir, float _power)
    {
        PlayerManager.instance.dashCooldown = 1.5f;
        PlayerManager.instance.currentPlayerState = PlayerManager.PlayerState.Dashing;
        GetComponent<Rigidbody2D>().drag = 25;
        GetComponent<Rigidbody2D>().AddForce(_normedDir * _power * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.02f * _power);
        GetComponent<Rigidbody2D>().drag = 100;
        PlayerManager.instance.currentPlayerState = PlayerManager.PlayerState.Idle;
        GameManager.instance.SlowGame(0.2f * _power, 0.1f);
    }


    public void EnterNexus(GameObject _gameObject)
    {
        PlayerManager.active = false;
        GetComponent<BoxCollider2D>().enabled = false;
        anim.SetTrigger("EnterNexus");
        iTween.MoveTo(gameObject, _gameObject.transform.position, 2.0f);
    }

}
