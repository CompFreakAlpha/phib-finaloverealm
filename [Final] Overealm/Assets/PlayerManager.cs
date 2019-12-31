using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;

    public GameObject activePlayer;

    public static int health = 5;
    public static int maxHealth = 10;
    public static float dashPower = 5;

    public float speed;

    public enum PlayerState
    {
        Idle,
        Moving,
        Dashing,
    }

    public PlayerState currentPlayerState = PlayerState.Idle;


    public GameObject playerClosestObjectInRange;

    public static bool active = true;

    public float maxInteractRange = 3;

    public float dashCooldown = 0;

    public void ChangeHealth(int change)
    {
        if (health + change > maxHealth)
        {
            health = maxHealth;
        }
        else if (health + change < 1)
        {
            Debug.Log("Player has died!");
            health = 1;
        }
        else
        {
            health += change;
        }
    }

    void Start()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if(activePlayer == null)
        {
            activePlayer = GameObject.FindGameObjectWithTag("Player");
        }

    }
}
