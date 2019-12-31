using UnityEngine;

[ExecuteInEditMode]
public class ItemDepth : MonoBehaviour
{
    public float depthSortOffset;

    void Update()
    {
        transform.GetChild(0).GetComponent<Renderer>().sortingOrder = (int)((transform.position.y + depthSortOffset) * -100);
        transform.GetChild(1).GetComponent<Renderer>().sortingOrder = (int)((transform.position.y + depthSortOffset) * -100);
    }


}