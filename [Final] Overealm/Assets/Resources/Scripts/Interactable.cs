using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public bool active = true;

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {
        UpdateHighlight();
    }

    public virtual void UpdateHighlight()
    {
        if (PlayerManager.instance.playerClosestObjectInRange == this.gameObject)
        {
            GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 1));
        }
        else if (PlayerManager.instance.playerClosestObjectInRange != this.gameObject && GetComponent<Renderer>().material.GetColor("_Color").a != 0)
        {
            GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 0));
        }
    }

    public virtual void OnInteractedWith()
    {
        Debug.Log("Player interacted with {" + gameObject.name + "}");
    }
}
