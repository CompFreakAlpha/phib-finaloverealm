using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{

    public bool entered = false;
    float fadeDelay = 2;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            entered = true;
            Debug.Log("Player entered Nexus!");
            PlayerManager.instance.activePlayer.GetComponent<Player>().EnterNexus(gameObject);
            transform.GetChild(4).GetComponent<ParticleSystem>().Play();
            
        }
    }

    public void Update()
    {
        if(entered)
        {
            transform.GetChild(4).localScale *= 1.0025f;
            if(fadeDelay > 0)
            {
                fadeDelay -= Time.deltaTime;
            } else
            {
                GameManager.instance.FadeToScene("Next");
            }
        }
    }

}
