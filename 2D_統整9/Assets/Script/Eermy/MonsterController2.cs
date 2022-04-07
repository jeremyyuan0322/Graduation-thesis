using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterController2 : MonoBehaviour
{
    public static Vector2Int mapSize;//地图尺寸
    public bool Movelock = true;

    public GameObject col;

    public Tilemap tilemap;

    public Grid grid_;//把人物移動搬來這邊看看(8/30)

    public int walk;//步數

    public static Vector3Int startPos;//起点
    public static Vector3Int CurrentPos;//怪物移動過後的位置
    public static Vector3Int endPos;//终点

    private bool hasStartPosSet;//是否设置了起点
    private bool hasEndPosSet;//是否设置了终点
    private bool hasobstacle;//有無被障礙物阻擋以至於不能過(範圍內可以 但行走會走出範圍外再回來)

    private Dictionary<Vector3Int, int> search = new Dictionary<Vector3Int, int>();//要进行的查找任务
    private Dictionary<Vector3Int, int> cost = new Dictionary<Vector3Int, int>();//起点到当前点的消耗
    public static Dictionary<Vector3Int, Vector3Int> pathSave = new Dictionary<Vector3Int, Vector3Int>();//保存回溯路径(反)

    public static List<Vector3Int> road = new List<Vector3Int>();//抓出綠色路徑(pathTile)的位置供其他腳本使用(正)

    private List<Vector3Int> hadSearch = new List<Vector3Int>();//已经查找过的坐标
    private List<Vector3Int> AttackSize = new List<Vector3Int>();//攻擊的範圍 

    private bool go = false;     //為true時進行移動
    private bool attack = false; //為true時發動攻擊
    public Animator animator;    //動畫
    private TurnControl turnScript;


    private void Start()
    {
        turnScript = GameObject.FindGameObjectWithTag("System").GetComponent<TurnControl>();
        mapSize.x = 12;
        mapSize.y = 12;
    }

    private void Update()
    {
        endPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(GameObject.Find("Player").GetComponent<Transform>().position));

        if (TurnControl.currentState == TurnControl.GameState.Game)
        {
            if (turnScript.isEnemyAction)
            {
                if (go == true)
                {
                    if (road.Count > 0)
                    { go = MonsterMove(); }

                    else
                    {
                        walk = 0;
                        go = false;
                        Movelock = true;
                    }
                    
                    print(go);
                }
                else if (attack == true)
                {
                    animator.SetBool("Attack", true);
                    CurrentPos = tilemap.WorldToCell(transform.position);
                    attack = MonsterAttack(CurrentPos);
                }
                else
                {
                    animator.SetBool("Attack", false);
                    if (!Movelock)
                    {
                        if (!hasStartPosSet)//第一次点击设置起点
                        {
                            startPos = tilemap.WorldToCell(transform.position);//怪物的位置
                            print("MONSTER" + startPos);
                            hasStartPosSet = true;
                        }

                        if (!hasEndPosSet)//第二次点击设置终点
                        {
                            endPos = tilemap.WorldToCell(GameObject.Find("Player").GetComponent<Transform>().position);
                            print("POTATO:" + endPos);
                            hasEndPosSet = true;
                            AStarSearchPath(endPos);
                            go = true;
                            attack = false;
                        }
                        else
                        {
                            print("reset");
                            hasStartPosSet = false;
                            hasEndPosSet = false;
                            search.Clear();
                            cost.Clear();
                            pathSave.Clear();
                            hadSearch.Clear();
                            road.Clear();

                            AttackSize.Clear();
                        }
                    }

                }
            }
        }

    }
    private bool MonsterAttack(Vector3Int Pos)//玉米的範圍攻擊(偵測)
    {
        for (int x = -2; x < 3; x++)
        {
            for (int y = -2; y < 3; y++)
            {
                AttackSize.Add(Pos + new Vector3Int(x, y, 0));
                //print(Pos + new Vector3Int(x, y, 0));
            }

        }
        while (AttackSize.Count > 0)
        {
            Vector3 obj = grid_.CellToWorld(AttackSize[0]);
            GameObject Obj = Instantiate(col, obj, new Quaternion(0, 0, 0, 0));
            Obj.name += turnScript.ColNum_Monster2;
            turnScript.ColNum_Monster2++;
            AttackSize.RemoveAt(0);
        }
        attack = false;

        return attack;
    }


    public bool MonsterMove() //怪物移動
    {
        if (Mathf.Abs(road[road.Count - 1].x - road[walk].x) > 2 || Mathf.Abs(road[road.Count - 1].y - road[walk].y) > 2)
        {
            Vector3 endPos = grid_.CellToWorld(road[road.Count - 1]);//終點
            if (transform.position != endPos)
            {
                Vector3 EnemyPosition = grid_.CellToWorld(road[walk]);//怪物位置
                if (road.Count != walk + 1)
                {
                    Vector3 NextPosition = grid_.CellToWorld(road[walk + 1]);//下個位置
                    Vector3 gy = Vector3.Lerp(EnemyPosition, NextPosition, 0.5f) - EnemyPosition;//位移距離 
                    transform.position = transform.position + gy;
                    if (transform.position == NextPosition)
                    {
                        walk = walk + 1;
                        //print("walk:"+walk+ "road.Count"+ road.Count);
                    }
                }

                if (walk == 2)
                {
                    go = false;
                    if (Mathf.Abs(road[road.Count - 1].x - road[walk].x) <= 2 && Mathf.Abs(road[road.Count - 1].y - road[walk].y) <= 2)
                    {
                        attack = true;

                    }
                    walk = 0;
                    Movelock = true;
                }
            }
        }
        else if(Mathf.Abs(road[road.Count - 1].x - road[walk].x) <= 2 && Mathf.Abs(road[road.Count - 1].y - road[walk].y) <= 2)
        {
            walk = 0;
            go = false;
            attack = true;
            Movelock = true;
        }

       
        return go;
    }

    //AStar算法查找
    public void AStarSearchPath(Vector3Int _endPos)//加入傳入參數方便其他程式調用
    {
        search.Clear();
        cost.Clear();
        hadSearch.Clear();
        pathSave.Clear();
        search.Add(startPos, GetHeuristic(startPos, _endPos)); //getheuristic是查詢點到終點的消耗的函式
        cost.Add(startPos, 0);//起點到當前點的消耗
        hadSearch.Add(startPos);//查找過的點座標紀錄
        pathSave.Add(startPos, startPos);//保存路徑

        while (search.Count > 0)
        {
            Vector3Int current = GetShortestPos();//获取任务列表里的最少消耗的那个坐标

            if (current.Equals(_endPos))
            {
                break;
            }

            List<Vector3Int> neighbors = GetNeighbors(current);//获取当前坐标的邻居 current→當前

            foreach (var next in neighbors)
            {
                if (!hadSearch.Contains(next))//Contains→包含
                {
                    cost.Add(next, cost[current] + 1);//计算当前格子的消耗，其实就是上一个格子加1步
                    search.Add(next, cost[next] + GetHeuristic(next, _endPos));//添加要查找的任务，消耗值为当前消耗加上当前点到终点的距离
                    pathSave.Add(next, current);//保存路径
                    hadSearch.Add(next);//添加该点为已经查询过

                }

            }
        }

        /*foreach (var item in pathSave.Keys)
        {
            print("有鐘點ㄇ"+item);
        }*/
            foreach (var item in pathSave.Keys)
        {
            if (pathSave.ContainsKey(_endPos))
            {
                hasobstacle = true;
                //print("有路");
            }
            else
            {
                print("No road");
                hasobstacle = false;
                break;
            }

        }
        if (hasobstacle == true)
        { ShowPath(_endPos); }

    }
    //获取周围可用的邻居
    //演算法的實際範圍在鄰居的可用範圍判定上
    private List<Vector3Int> GetNeighbors(Vector3Int target)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        Vector3Int up = target + Vector3Int.up;
        Vector3Int right = target + Vector3Int.right;
        Vector3Int left = target - Vector3Int.right;
        Vector3Int down = target - Vector3Int.up;
        print(" mapSize.x" + mapSize.x + "     mapSize.y" + mapSize.y);

        //Up
        //!obstacle.Contains(up) 檢查obstacle串列中 不包含 "當下的up"
        if (/*up.y < mapSize.y - 1 &&*/ !TileTest.obstacle.Contains(up))
        {
            neighbors.Add(up);
        }
        //Right
        if (/*right.x < mapSize.x - 1 &&*/ !TileTest.obstacle.Contains(right))
        {
            neighbors.Add(target + Vector3Int.right);
        }
        //Left
        if (/*left.x >= 0 - 1 &&*/ !TileTest.obstacle.Contains(left))
        {
            neighbors.Add(target - Vector3Int.right);
        }
        //Down
        if (/*down.y >= 0 - 1 &&*/ !TileTest.obstacle.Contains(down))
        {
            neighbors.Add(target - Vector3Int.up);
        }

        return neighbors;
    }
    //获取当前位置到终点的消耗
    private int GetHeuristic(Vector3Int posA, Vector3Int posB)
    {
        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);//回傳絕對值
    }
    //获取任务字典里面最少消耗的坐标
    private Vector3Int GetShortestPos()
    {
        KeyValuePair<Vector3Int, int> shortest = new KeyValuePair<Vector3Int, int>(Vector3Int.zero, int.MaxValue);

        foreach (var item in search)
        {
            if (item.Value < shortest.Value)
            {
                shortest = item;
            }
        }

        search.Remove(shortest.Key);
        return shortest.Key;
    }

    //显示查找完成的路径
    public void ShowPath(Vector3Int _endPos)
    {
        road.Clear();
        Vector3Int current = _endPos;
        while (current != startPos)
        {
            Vector3Int next = pathSave[current];
            road.Add(current);
            current = next;
        }
        road.Add(startPos);
        road.Reverse();//Reverse倒轉 12345/54321
        foreach (var item in road)
        {
            print("road" + item);
        }
    }

}