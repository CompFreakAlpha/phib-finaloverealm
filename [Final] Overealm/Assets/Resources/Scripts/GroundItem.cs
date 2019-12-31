using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : Interactable
{
    public ItemStack itemData;
    SpriteRenderer iconSpr;

    public override void Start()
    {
        iconSpr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        iconSpr.sprite = itemData.item.icon;
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = itemData.item.icon;
    }

    public override void OnInteractedWith()
    {
        base.OnInteractedWith();
        var aSource = GameManager.instance.PlayClipAt(itemData.item.pickupSound, transform.position);
        aSource.spatialize = true;
        aSource.spatialBlend = 1f;
        aSource.rolloffMode = AudioRolloffMode.Linear;
        aSource.minDistance = 1;
        aSource.maxDistance = 25;
        aSource.dopplerLevel = 0;
        Destroy(gameObject);
    }

    public override void UpdateHighlight()
    {
        if (PlayerManager.instance.playerClosestObjectInRange == this.gameObject && iconSpr.materials[0].GetColor("_Color").a != 1)
        {
            iconSpr.materials[0].SetColor("_Color", new Color(1, 1, 1, 1));
        }
        else if (PlayerManager.instance.playerClosestObjectInRange != this.gameObject && iconSpr.materials[0].GetColor("_Color").a != 0)
        {
            iconSpr.materials[0].SetColor("_Color", new Color(1, 1, 1, 0));
        }
    }

}
