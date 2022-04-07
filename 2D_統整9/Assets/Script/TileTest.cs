using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
public class TileTest : MonoBehaviour
{

    public static List<Vector3Int> obstacle = new List<Vector3Int>();//障碍物坐标
    public static List<Vector3Int> ground = new List<Vector3Int>();//有地板的坐标
    public Tilemap obstaclemap;
    public Tilemap groundmap;
    public Tile a;//檢測覆蓋用(用綠色小方塊)
    public void obstacle_tiletest(int Y)
    {
        //obstaclemap = GetComponent<Tilemap>();
        BoundsInt bounds = obstaclemap.cellBounds;
        TileBase[] allTiles = obstaclemap.GetTilesBlock(bounds);
        //print("1. " + tilemap + " 2. " + bounds + " 3." + allTiles);
        //會將最下方的點設為(0,0) 要以最下方的點調整
        obstacle.Clear();
        //print("起點 " + obstaclemap.origin + " 大小 " + obstaclemap.size );
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);

                    Vector3Int obstaclePos = Vector3Int.up;
                    obstaclePos.x = x + obstaclemap.origin.x;        //直接將跑掉的位置座標調整
                    obstaclePos.y = y + Y;        //直接將跑掉的位置座標調整
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
        //return obstacle;
    }
    public void ground_tiletest(int Y)
    {
        //groundmap = GetComponent<Tilemap>();
        BoundsInt bounds = groundmap.cellBounds;
        TileBase[] allTiles = groundmap.GetTilesBlock(bounds);
        //print("1. " + tilemap + " 2. " + bounds + " 3." + allTiles);
        //會將最下方的點設為(0,0) 要以最下方的點調整
        ground.Clear();
        //print("起點 " + groundmap.origin + " 大小 " + groundmap.size);
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);

                    Vector3Int obstaclePos = Vector3Int.up;
                    obstaclePos.x = x + groundmap.origin.x;        //直接將跑掉的位置座標調整
                    obstaclePos.y = y + Y;        //直接將跑掉的位置座標調整
                                                  //每次重新設值就會跑掉(因最左下方基準點變換)
                    obstaclePos.z = 0;
                    //print("obstaclePos" + obstaclePos);
                    ground.Add(obstaclePos);
                    //groundmap.SetTile(obstaclePos, a);
                }
                else
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }
        //return obstacle;
    }
    void Start()
    {
        /*
        Tilemap tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        print("1. " + tilemap + " 2. " + bounds + " 3." + allTiles);
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
                    obstaclePos.x = x -2;        //直接將跑掉的位置座標調整
                    obstaclePos.y = y -2;        //直接將跑掉的位置座標調整
                                                  //每次重新設值就會跑掉(因最左下方基準點變換)
                    obstaclePos.z = 0;
                    print("obstaclePos" + obstaclePos);
                    obstacle.Add(obstaclePos);
                }
                else
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }*/
    }
}