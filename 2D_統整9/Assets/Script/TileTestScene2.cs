using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
public class TileTestScene2 : MonoBehaviour
{

    public static List<Vector3Int> obstacle = new List<Vector3Int>();//障碍物坐标
    
    void Start()
    {
        
        Tilemap tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        //print("1. " + tilemap + " 2. " + bounds + " 3." + allTiles);
        //會將最下方的點設為(0,0) 要以最下方的點調整
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    
                    Vector3Int obstaclePos = Vector3Int.up;
                    obstaclePos.x = x -7;        //直接將跑掉的位置座標調整
                    obstaclePos.y = y -13;        //直接將跑掉的位置座標調整
                                                  //每次重新設值就會跑掉(因最左下方基準點變換)
                    obstaclePos.z = 0;
                    //print("obstaclePos" + obstaclePos);
                    obstacle.Add(obstaclePos);
                }
                else
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }
    }
}