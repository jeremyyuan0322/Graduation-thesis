
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BreathFirst : MonoBehaviour
{
    public TileBase baseTile;//藍色tile
    public TileBase non;
    public Tilemap tilemap;
    public static bool isRunning = true;
    public static Vector3Int ExplorArounds = new Vector3Int();
    public static List<Vector3Int> HadWaySave = new List<Vector3Int>();//保存探索路徑 所有藍tile的格子路徑
    private Dictionary<Vector3Int, int> cost = new Dictionary<Vector3Int, int>();//起点到当前点的消耗
    private static readonly List<Vector3Int> tilesOffset = new List<Vector3Int>()//(0,1,0)(1,0,0)(-1,0,0)(0,-1,0)上右下左
    {
        Vector3Int.up,Vector3Int.right,Vector3Int.down,Vector3Int.left
    };
    private static readonly List<Vector3Int> tilesOffset2 = new List<Vector3Int>()//(1,1,0)(1,-1,0)(-1,-1,0)(-1,1,0)右上/右下/左下/左上
    {
        Vector3Int.up+Vector3Int.right,Vector3Int.right+Vector3Int.down,Vector3Int.down+Vector3Int.left,Vector3Int.left+Vector3Int.up
    };
    Queue<Vector3Int> queue = new Queue<Vector3Int>();

    public Vector3Int limit;
    public Tilemap TP;//改成放可行層

    void Start()
    {

    }


    private void ExplorAround(Vector3Int _from)
    {
        if (isRunning == false)
        { return; }
        //print("看看" + _from);
       
        foreach (Vector3Int i in tilesOffset)
        {
            ExplorArounds = _from + i;
            if (HadWaySave.Contains(ExplorArounds))
            {
            }
            else if (!TP.HasTile(ExplorArounds) || TileTest.obstacle.Contains(ExplorArounds))
            {
                //queue.Enqueue(ExplorArounds);//不用傳回queue中當下次的基準點(不會以不通的格子偵測其上右下左)
                //cost.Add(ExplorArounds, cost[_from] + 1);//不通的格子不用有消耗
                HadWaySave.Add(ExplorArounds);//儘管不通也算有調查過
                //print(ExplorArounds);
            }
            else
            {

                //print("要鋪藍色"+ExplorArounds);
                queue.Enqueue(ExplorArounds);
                HadWaySave.Add(ExplorArounds);
                cost.Add(ExplorArounds, cost[_from] + 1);//消耗1傳進來的一定是消耗2
                tilemap.SetTile(ExplorArounds, baseTile);

            }

        }
        
        
    }

    public void BFS(Vector3Int _pos)
    {
        //print("pos有錯嗎" + _pos);
        //foreach (var item in TileTest.obstacle)
        //{ print("障礙座標出問題嗎" + item); }
        isRunning = true;
        cost.Clear();
        HadWaySave.Clear();
        queue.Clear();
        HadWaySave.Add(_pos);
        queue.Enqueue(_pos);
        cost.Add(_pos, 0);
        //print("看看"+ TileController.posMouseOnGrid);
        while (queue.Count > 0 && isRunning == true)
        {

            var searchCenter = queue.Dequeue();//移除並返回queue第一個對象

            StopIfSearchEnd(searchCenter);
            ExplorAround(searchCenter);

        }
    }

    private void StopIfSearchEnd(Vector3Int _searchCenter)//藍色可行動範圍有多大在這設定
    {
        
        if (cost[_searchCenter] == PlayerController.Energy)
        { isRunning = false; }

    }
    public void Clear()
    {
        foreach (var item in HadWaySave)
        {
            tilemap.SetTile(item, non);
            //tilemap.SetTileFlags(item, TileFlags.None);????????????????
            //tilemap.SetColor(item, Color.white);
        }
        HadWaySave.Clear();
        /*
        foreach (var i in MapBehaviour.road)
        {
            tilemap.SetTileFlags(i, TileFlags.None);
            tilemap.SetColor(i, Color.white);
        }
        MapBehaviour.road.Clear();
        */
    }

    public void H(Vector3Int _pos)//傳入的應該是主角腳下的座標
    {
        HadWaySave.Clear();

        HadWaySave.Add(_pos);

        Vector3Int x;
        foreach (Vector3Int i in tilesOffset)//for_each 跑完一次整個list
        {
            x = _pos + i;
            HadWaySave.Add(x);
            if (TP.HasTile(x))
            {
                tilemap.SetTile(x, baseTile);
                tilemap.SetTileFlags(x, TileFlags.None);
            }
        }
        //HadWaySave.RemoveAt(0);

    }
    public void HH(Vector3Int _pos)//傳入的應該是主角腳下的座標
    {
        HadWaySave.Clear();

        HadWaySave.Add(_pos);

        Vector3Int x;
        foreach (Vector3Int i in tilesOffset)//for_each 跑完一次整個list
        {
            x = _pos + i;
            HadWaySave.Add(x);
            if (TP.HasTile(x))
            {
                tilemap.SetTile(x, baseTile);
                tilemap.SetTileFlags(x, TileFlags.None);
            }
        }
        //HadWaySave.RemoveAt(0);
    }
    public void HHH(Vector3Int _pos)//傳入的應該是主角腳下的座標
    {
        HadWaySave.Clear();

        HadWaySave.Add(_pos);

        Vector3Int x;
        foreach (Vector3Int i in tilesOffset)//for_each 跑完一次整個list
        {
            x = _pos + i;
            HadWaySave.Add(x);
            if (TP.HasTile(x))
            {
                tilemap.SetTile(x, baseTile);
                tilemap.SetTileFlags(x, TileFlags.None);
            }
        }
        //HadWaySave.RemoveAt(0);
    }
    public void PPP(Vector3Int _pos)//傳入的應該是主角腳下的座標;這招是第一圈傷害
    {
        HadWaySave.Clear();
        //print("+++"+_pos);
        HadWaySave.Add(_pos);
        Vector3Int x;
        foreach (Vector3Int i in tilesOffset)//for_each 跑完一次整個list(上右下左)
        {
            x = _pos + i;
            HadWaySave.Add(x);
            if (TP.HasTile(x))
            {
                tilemap.SetTile(x, baseTile);
                tilemap.SetTileFlags(x, TileFlags.None);
                //print(x);
            }
        }
        foreach (Vector3Int i in tilesOffset2)//for_each 跑完一次整個list(右上/右下/左下/左上)
        {
            x = _pos + i;
            HadWaySave.Add(x);
            if (TP.HasTile(x))
            {
                tilemap.SetTile(x, baseTile);
                tilemap.SetTileFlags(x, TileFlags.None);
                //print(x);
            }
        }
        //HadWaySave.RemoveAt(0);
    }

    public void HHP(Vector3Int _pos)//傳入的應該是主角腳下的座標
    {
        HadWaySave.Clear();
        //print("+++"+_pos);
        HadWaySave.Add(_pos);
        Vector3Int x;
        foreach (Vector3Int i in tilesOffset)//for_each 跑完一次整個list
        {
            for (int j = 1; j <= 3; j++)
            {
                x = _pos + i * j;
                HadWaySave.Add(x);
                if (TP.HasTile(x))
                {
                    tilemap.SetTile(x, baseTile);
                    tilemap.SetTileFlags(x, TileFlags.None);
                }
            }
        }
        //HadWaySave.RemoveAt(0);
    }
    public void PPH(Vector3Int _pos)//傳入的應該是主角腳下的座標;這招是第二圈傷害
    {
        HadWaySave.Clear();
        //print("+++"+_pos);
        HadWaySave.Add(_pos);
        Vector3Int x;
        foreach (Vector3Int i in tilesOffset)//for_each 跑完一次整個list(上右下左)
        {
            for (int j = 2; j <= 2; j++)
            {
                x = _pos + i * j;
                HadWaySave.Add(x);
                if (i == Vector3Int.up || i == Vector3Int.down)
                {
                    x = x + Vector3Int.left;
                    HadWaySave.Add(x);
                    x = x + Vector3Int.right + Vector3Int.right;
                    HadWaySave.Add(x);
                }
                else if (i == Vector3Int.left || i == Vector3Int.right)
                {
                    x = x + Vector3Int.up;
                    HadWaySave.Add(x);
                    x = x + Vector3Int.down + Vector3Int.down;
                    HadWaySave.Add(x);
                }
                foreach (Vector3Int k in HadWaySave)
                {
                    if (TP.HasTile(k))
                    {
                        tilemap.SetTile(k, baseTile);
                        tilemap.SetTileFlags(k, TileFlags.None);
                    }
                }
            }
        }
        foreach (Vector3Int i in tilesOffset2)//for_each 跑完一次整個list(右上/右下/左下/左上)
        {
            for (int j = 2; j <= 2; j++)
            {
                x = _pos + i * j;
                HadWaySave.Add(x);
                if (TP.HasTile(x))
                {
                    tilemap.SetTile(x, baseTile);
                    tilemap.SetTileFlags(x, TileFlags.None);
                }
            }
        }
        //HadWaySave.RemoveAt(0);
    }
    
    public void V(Vector3Int _pos)//淳元的創地形
    {
        HadWaySave.Clear();
        //print("+++"+_pos);
        HadWaySave.Add(_pos);
        Vector3Int x;
        foreach (Vector3Int i in tilesOffset)//for_each 跑完一次整個list
        {
            for (int j = 1; j <= 10; j++)
            {
                x = _pos + i * j;
                HadWaySave.Add(x);
                if (TP.HasTile(x) && TileTest.obstacle.Contains(x))
                {
                    //print("X"+x);
                    //tilemap.SetTile(x, baseTile);
                    //tilemap.SetTileFlags(x, TileFlags.None);
                }
                else if(TP.HasTile(x) && !TileTest.obstacle.Contains(x) &&  j==1)
                {
                    
                    break; 
                }
                else if (TP.HasTile(x) && !TileTest.obstacle.Contains(x))
                {
                    tilemap.SetTile(x, baseTile);
                    tilemap.SetTileFlags(x, TileFlags.None);

                    break;
                }
            }

        }


    }
    public void PH(Vector3Int _pos)//傳入的應該是主角腳下的座標
    {
        HadWaySave.Clear();
        //print("+++"+_pos);
        HadWaySave.Add(_pos);
        Vector3Int x;
        foreach (Vector3Int i in tilesOffset)//for_each 跑完一次整個list
        {
            for (int j = 1; j <= 3; j++)
            {
                x = _pos + i * j;
                HadWaySave.Add(x);
                if (TP.HasTile(x))
                {
                    tilemap.SetTile(x, baseTile);
                    tilemap.SetTileFlags(x, TileFlags.None);
                }
            }

        }
    }
    public void VH(Vector3Int _pos)//傳入的應該是主角腳下的座標  拉
    {
        HadWaySave.Clear();
        //print("+++"+_pos);
        HadWaySave.Add(_pos);
        Vector3Int x;
        foreach (Vector3Int i in tilesOffset)//for_each 跑完一次整個list
        {
            for (int j = 2; j <= 3; j++)
            {
                x = _pos + i * j;
                HadWaySave.Add(x);
                if (TP.HasTile(x))
                {
                    tilemap.SetTile(x, baseTile);
                    tilemap.SetTileFlags(x, TileFlags.None);
                }
            }

        }
    }
    public void P(Vector3Int _pos)
    {
        HadWaySave.Clear();
    }
    public void PP(Vector3Int _pos)
    {
        HadWaySave.Clear();
    }
}