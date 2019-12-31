using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector2 roomSize;


    public GameObject player;
    public bool playable = true;
    public int cellX, cellY;
    public int minCellX, maxCellX, minCellY, maxCellY;

    

    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void LateUpdate()
    {
        cellX = (int)Mathf.Round(player.transform.position.x / roomSize.x);
        cellX = Mathf.Clamp(cellX, minCellX, maxCellX);
        cellY = (int)Mathf.Round(player.transform.position.y / roomSize.y);
        cellY = Mathf.Clamp(cellY, minCellY, maxCellY);
        if (playable)
        {
            iTween.MoveTo(gameObject, new Vector3(cellX * roomSize.x, (cellY * roomSize.y), -10), 1f);
        }

    }

    public void OnDrawGizmos()
    {

        // ===={ DRAW PLAYABLE BOX }====

        Vector2 cornerBL = new Vector2(minCellX * (roomSize.x) - (roomSize.x / 2), minCellY * (roomSize.y) - (roomSize.y / 2));
        Vector2 cornerTL = new Vector2(minCellX * (roomSize.x) - (roomSize.x / 2), maxCellY * (roomSize.y) + (roomSize.y / 2));
        Vector2 cornerBR = new Vector2(maxCellX * (roomSize.x) + (roomSize.x / 2), minCellY * (roomSize.y) - (roomSize.y / 2));
        Vector2 cornerTR = new Vector2(maxCellX * (roomSize.x) + (roomSize.x / 2), maxCellY * (roomSize.y) + (roomSize.y / 2));



        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(cornerBL, cornerTL);
        Gizmos.DrawLine(cornerBL, cornerBR);
        Gizmos.DrawLine(cornerTR, cornerTL);
        Gizmos.DrawLine(cornerTR, cornerBR);

        for (float i = minCellX * (roomSize.x) - (roomSize.x / 2); i < maxCellX * (roomSize.x) + (roomSize.x / 2); i+= roomSize.x)
        {
            Gizmos.DrawLine(new Vector2(i, minCellY * (roomSize.y) - (roomSize.y / 2)), new Vector2(i, maxCellY * (roomSize.y) + (roomSize.y / 2)));
        }
        for (float i = minCellY * (roomSize.y) - (roomSize.y / 2); i < maxCellY * (roomSize.y) + (roomSize.y / 2); i += roomSize.y)
        {
            Gizmos.DrawLine(new Vector2(minCellX * (roomSize.x) - (roomSize.x / 2), i), new Vector2(maxCellX * (roomSize.x) + (roomSize.x / 2), i));
        }

    }

}
