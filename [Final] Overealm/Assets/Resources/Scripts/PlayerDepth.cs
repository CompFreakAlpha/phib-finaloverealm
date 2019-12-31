using UnityEngine;

[ExecuteInEditMode]
public class PlayerDepth : MonoBehaviour
{

    void Update()
    {
        GetComponent<Renderer>().sortingOrder = (int)(transform.position.y * -100 - 3);
        transform.parent.GetComponent<Renderer>().sortingOrder = (int)(transform.position.y * -100 - 2);
        transform.parent.parent.GetChild(0).GetComponent<Renderer>().sortingOrder = (int)(transform.position.y * -100 - 1);
    }


}