using UnityEngine;

[ExecuteInEditMode]
public class Depth : MonoBehaviour
{
    public float depthSortOffset;

    void Update()
    {
        GetComponent<Renderer>().sortingOrder = (int)((transform.position.y + depthSortOffset) * -100);
    }


}