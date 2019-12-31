using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFadeEffect : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, GetComponent<SpriteRenderer>().sprite);
    }
}
