using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Phibian.Utilities
{
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "New Random Tile", order = 1)]
#endif
    public class RandomTile : TileBase
    {
        [SerializeField]
        public Sprite RawTileImage;

        public SprWithChance[] sprites;



        public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            base.GetTileData(location, tileMap, ref tileData);

            //    Change Sprite
            float totalChance = 0;
            foreach(SprWithChance s in sprites)
            {
                totalChance += s.relativeChance;
            }
            float rand = UnityEngine.Random.Range(0f, totalChance);
            float run = 0f;
            foreach (SprWithChance s in sprites)
            {
                run += s.relativeChance;
                if(run >= rand)
                {
                    tileData.sprite = s.spr;
                    break;
                }
            }

            
        }

    }

}

[System.Serializable]
public class SprWithChance
{
    public Sprite spr;
    public float relativeChance = 100f;
}