using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using System.Threading; //sleep用的

public class MapBehaviour : MonoBehaviour
{
    public int V_energy = 1;
    public static Vector2Int mapSize;//地图尺寸

    public Tilemap tilemap;
    public Tilemap tilemap2;

    public Tile normalTile;//白色tile(陸地)
    public Tile obstacleTile;//黑色tile(障礙物)
    public Tile pathTile;//绿色tile(人物)
    public Tile pathTile2;//紅色tile(人物)

    public GameObject g;
    public Grid grid_;//把人物移動搬來這邊看看(8/30)

    public int obstacleCount;//要生成的障碍物数量

    public int work;//步數
    public int x;//已經走的步數

    public static Vector3Int startPos;//起点
    public static Vector3Int endPos;//终点

    public static Vector3Int checkPos;//確認路徑時的點

    public static bool hasStartPosSet;//是否设置了起点
    public static bool hasEndPosSet;//是否设置了终点

    private bool hasobstacle;//有無被障礙物阻擋以至於不能過(範圍內可以 但行走會走出範圍外再回來) PS:應該是有沒有路 有就是true

    private Dictionary<Vector3Int, int> search = new Dictionary<Vector3Int, int>();//要进行的查找任务
    private Dictionary<Vector3Int, int> cost = new Dictionary<Vector3Int, int>();//起点到当前点的消耗
    public static Dictionary<Vector3Int, Vector3Int> pathSave = new Dictionary<Vector3Int, Vector3Int>();//保存回溯路径(反)

    public static List<Vector3Int> road = new List<Vector3Int>();//抓出綠色路徑(pathTile)的位置供其他腳本使用(正)

    private List<Vector3Int> hadSearch = new List<Vector3Int>();//已经查找过的坐标

    private List<Vector3Int> obstacle = new List<Vector3Int>();//障碍物坐标



    public static bool go = false;

    //回合控制脚本(從期鈞那抓的 配合start抓取物件)
    private TurnControl turnScript;


    private void Awake()
    {
        //endPos.x = 0;
        //endPos.y = 3;
        //endPos.z = 0;
        
    }

    private void Start()
    {
        turnScript = GameObject.FindGameObjectWithTag("System").GetComponent<TurnControl>();
        mapSize.x = 12;
        mapSize.y = 12;

        Scene1(Scenemapdata.obstacle, Scenemapdata.ground);
        GameObject.Find("Main Camera").GetComponent<CameraController>().SetCamToPlayer();//開場時把鏡頭對到玩家身上
        //print("有跑?");
        //GameObject.Find("碰撞層").GetComponent<TileTest>().obstacle_tiletest(-4);
        //GameObject.Find("碰撞層").GetComponent<TileTest>().ground_tiletest(-12);
        
        //CreateNormalTiles();
        //print("work" + work);
        //CreateObstacleTiles();

        //print(TileTest.someGlobal);
        //↑樓上有調用成功
        //print(TileTest.obstacle);//調用成功好爽o
    }

    public void Gotrue()//整合了攻擊時的確定跟移動的確定
    {
      
        if (turnScript.isWaitForPlayerMove)
        {
            if (hasEndPosSet)
            {
                go = true;
                GameObject.Find("Player").GetComponent<PlayerController>().CalculatePlayerEnergy();
            }

        }
        else if (turnScript.isWaitForPlayerAttack)
        {
            if (PlayerController.Energy < PlayerController.EnergyCosume)

            {
                //如果玩家剩餘的體力<技能需要的體力消耗
                //什麼都不做
            }
            else
            {

                /*foreach (var i in road)
                {
                    print(i);
                }*/
                if (road.Count == 0&&AttackController.combo!="P"&&AttackController.combo!="PP" && AttackController.combo != "PPP" && AttackController.combo != "PPH")
                {
                    print("空的");
                }
                else
                {
                    if (road.Contains(checkPos))
                    {
                        road.Remove(checkPos);
                    }

                    if (AttackController.combo == "PPP" || AttackController.combo == "PPH")
                    {
                        //BreathFirst.HadWaySave.RemoveAt(0);
                        print("有洞?");
                        foreach (var i in BreathFirst.HadWaySave)
                        {

                            //print("ROAD" + i);

                            //tilemap.SetTileFlags(i, TileFlags.None);
                            //tilemap.SetColor(i, Color.gray);


                            GameObject.Find("Player").GetComponent<AttackController>().Create(i);
                            GameObject.Find("Player").GetComponent<AttackController>().Destroy_bullet();

                        }
                    }
                    else if (AttackController.combo == "P")
                    {
                        GameObject.Find("Player").GetComponent<PlayerController>().Hp += 3;
                        GameObject.Find("Player").GetComponent<PlayerController>().CalculatePlayerEnergy();
                        GameObject.Find("System").GetComponent<TurnControl>().TurnChange();
                        //GameObject.Find("Player").GetComponent<PlayerController>().CalculatePlayerEnergy();
                    }
                    else if (AttackController.combo == "PP")
                    {
                        PlayerController.Energy += 3;
                        GameObject.Find("Player").GetComponent<PlayerController>().CalculatePlayerEnergy();
                        GameObject.Find("System").GetComponent<TurnControl>().TurnChange();
                        //GameObject.Find("Player").GetComponent<PlayerController>().CalculatePlayerEnergy();
                    }
                    else if ((AttackController.combo == "V"))
                    {
                        road.RemoveAt(road.Count - 1);

                        foreach (var i in road)
                        {
                            //road.RemoveAt(0);
                            //tilemap.SetTileFlags(i, TileFlags.None);
                            //tilemap.SetColor(i, Color.gray);
                            GameObject.Find("Player").GetComponent<AttackController>().Create(i);

                            //print("VVV"+V_energy);
                            GameObject.Find("Player").GetComponent<AttackController>().Destroy_bullet();
                            //ShowPath(endPos);
                            //road.Add(startPos);
                            //print("rrr" + i + startPos);
                            //print("kkk" + road[0] + road[1]);
                        }
                        V_energy = 1;

                    }
                    else if (AttackController.combo == "VH")
                    {
                        road.RemoveAt(0);
                        {
                           
                                
                                print("ENDPOS :" + road[road.Count-1]);
                                GameObject.Find("Player").GetComponent<AttackController>().Create(road[road.Count - 1]);
                                GameObject.Find("Player").GetComponent<AttackController>().Destroy_bullet();
                                //ShowPath(endPos);
                                //road.Add(startPos);
                                //print("rrr" + i + startPos);
                                //print("kkk" + road[0] + road[1]);
                            
                        }
                    }
                    else if (AttackController.combo == "PH")
                    {
                        
                        {


                            print("ENDPOS :" + road[road.Count - 1]);
                            GameObject.Find("Player").GetComponent<AttackController>().Create(road[road.Count - 1]);
                            GameObject.Find("Player").GetComponent<AttackController>().Destroy_bullet();
                            //ShowPath(endPos);
                            //road.Add(startPos);
                            //print("rrr" + i + startPos);
                            //print("kkk" + road[0] + road[1]);

                        }
                    }
                    else
                    {

                        foreach (var i in road)
                        {
                            //road.RemoveAt(0);
                            //tilemap.SetTileFlags(i, TileFlags.None);
                            //tilemap.SetColor(i, Color.gray);
                            GameObject.Find("Player").GetComponent<AttackController>().Create(i);
                            GameObject.Find("Player").GetComponent<AttackController>().Destroy_bullet();
                            //ShowPath(endPos);
                            //road.Add(startPos);
                            //print("rrr" + i + startPos);
                            //print("kkk" + road[0] + road[1]);
                        }
                    }
                    GameObject.Find("Player").GetComponent<PlayerController>().CalculatePlayerEnergy();
                    GameObject.Find("Player").GetComponent<AttackController>().AllCancelCoin();
                }
            }
        }

    }
    private void Update()
    {
        
         
        
        if (TurnControl.currentState == TurnControl.GameState.Game)
        {


            checkPos = tilemap.WorldToCell(g.transform.position);
            if (turnScript.isWaitForPlayerMove)
            {
                if (go == true)
                {
                    go = GameObject.Find("Player").GetComponent<MouseDetector2>().Goo();
                }
                else
                {
                    if (!hasStartPosSet)//第一次点击设置起点
                    {
                        //print("5k41u0 ");
                        //StartPos = tilemap.WorldToCell(g.transform.position);
                        //StartPos.y = StartPos.y - 0.5f;
                        startPos = tilemap.WorldToCell(g.transform.position);//紀錄第一次點擊位置 避免同次點擊時出錯
                        //print("startPos" + startPos);
                        tilemap2.SetTile(startPos, pathTile2);
                        hasStartPosSet = true;
                        //GameObject.Find("Player").GetComponent<MouseDetector2>().assd();
                        //GameObject.Find("BFS").GetComponent<BreathFirst>().Clear();
                        //if (TurnControl.xx == 1)
                        //{ }
                        //else
                        GameObject.Find("BFS").GetComponent<BreathFirst>().BFS(startPos);
                    }


                    if (Input.GetMouseButtonDown(0))
                    {
                        if (GameObject.Find("System").GetComponent<ClickEvent>().check_UI_click() == false)
                        { 
                        if (!hasEndPosSet)//第二次点击设置终点
                        {
                            Thread.Sleep(500);
                            if (tilemap.HasTile(TileController.posMouseOnGrid) && TileController.posMouseOnGrid != startPos && TileTest.obstacle.Contains(TileController.posMouseOnGrid) == false)
                            {
                                endPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                                endPos.z = 0;
                                //print("座標在哪" + endPos);
                                if (BreathFirst.HadWaySave.Contains(endPos) == true)
                                {
                                    hasEndPosSet = true;

                                    //print("endpos" + endPos);
                                    AStarSearchPath(endPos);
                                    //go = true;
                                    GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("move", road.Count - 1);
                                    //GameObject.Find("Player").GetComponent<MouseDetector2>().assd();

                                    //startPos = endPos;

                                    // GameObject.Find("Player").GetComponent<MouseDetector2>().assd();
                                    startPos = endPos;//調整位置的判定時機讓其他腳本抓取正確
                                }

                                //print("endPos" + endPos);
                                //tilemap2.SetTile(endPos, pathTile);

                            }
                            else if (TileController.posMouseOnGrid == startPos)
                            {

                                print("TileController.posMouseOnGrid " + TileController.posMouseOnGrid + "StartPos" + startPos);
                            }
                        }
                        else if (tilemap.HasTile(TileController.posMouseOnGrid))//重置
                        {
                            Thread.Sleep(500);
                            //print("on");
                            hasStartPosSet = false;
                            hasEndPosSet = false;

                            startPos = endPos;//調整位置的判定時機讓其他腳本抓取正確
                            GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("move", 0);
                            GameObject.Find("BFS").GetComponent<BreathFirst>().Clear();
                            Clear();

                        }
                        }
                    }
                }
            }
        }
    }
    //创建白色地图
    public void CreateNormalTiles()
    {

        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                Vector3Int position = new Vector3Int(i - 6, j - 6, 0);
                tilemap.SetTile(position, normalTile);
            }
        }
    }
    //创建黑色障碍
    public void CreateObstacleTiles()
    {
        List<Vector3Int> blankTiles = new List<Vector3Int>();

        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                blankTiles.Add(new Vector3Int(i - 6, j - 6, 0));
            }
        }

        for (int i = 0; i < obstacleCount; i++)
        {
            int index = Random.Range(0, blankTiles.Count);
            Vector3Int obstaclePos = blankTiles[index];
            blankTiles.RemoveAt(index);
            obstacle.Add(obstaclePos);

            tilemap.SetTile(obstaclePos, obstacleTile);
        }
    }
    //AStar算法查找
    public void AStarSearchPath(Vector3Int _endPos)//加入傳入參數方便其他程式調用
    {
        //初始化
        //print("Start" + startPos + "End" + _endPos);
        Clear();
        ///print(startPos);
        search.Add(startPos, GetHeuristic(startPos, _endPos)); //getheuristic是查詢點到終點的消耗的函式
        cost.Add(startPos, 0);//起點到當前點的消耗
        hadSearch.Add(startPos);//查找過的點座標紀錄
        pathSave.Add(startPos, startPos);//保存路徑

        while (search.Count > 0)
        {
            Vector3Int current = GetShortestPos();//获取任务列表里的最少消耗的那个坐标

            if (current.Equals(_endPos))
            {
                //road.Add(current);
                /*foreach (var next in road)
                {
                    print("內容正確嗎"+next);
                }*/
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
                    //print("NEXT" + next);
                    //print("current" + current);
                    //tilemap.SetTile(next, pathTile);//顯示查找過的位置(用pathtile) 
                    //if (!road.Contains(current))
                    //{ road.Add(current); }

                }

            }
        }

        //foreach (var item in pathSave.Keys)   //沒用到呢先註解
        //{
            if (BreathFirst.HadWaySave.Contains(_endPos) == true && pathSave.ContainsKey(_endPos))
            {
                hasobstacle = true;

            }
            else
            {
                print("No road");
                hasobstacle = false;
                //break;
            }
        //}

        if (hasobstacle == true)
        {
            ShowPath(_endPos);
        }

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

        if (turnScript.isWaitForPlayerMove)
        {
            //Up
            //!obstacle.Contains(up) 檢查obstacle串列中 不包含 "當下的up"
            if (tilemap.HasTile(up)&&!TileTest.obstacle.Contains(up))
            {
                neighbors.Add(up);
            }
            //Right
            if (tilemap.HasTile(right) && !TileTest.obstacle.Contains(right))
            {
                neighbors.Add(target + Vector3Int.right);
            }
            //Left
            if (tilemap.HasTile(left) && !TileTest.obstacle.Contains(left))
            {
                neighbors.Add(target - Vector3Int.right);
            }
            //Down
            if (tilemap.HasTile(down) && !TileTest.obstacle.Contains(down))
            {
                neighbors.Add(target - Vector3Int.up);
            }

        }
        else if (turnScript.isWaitForPlayerAttack)
        {
            //Up
            //!obstacle.Contains(up) 檢查obstacle串列中 不包含 "當下的up"
            if (tilemap.HasTile(up))
            {
                neighbors.Add(up);
            }
            //Right
            if (tilemap.HasTile(right))
            {
                neighbors.Add(target + Vector3Int.right);
            }
            //Left
            if (tilemap.HasTile(left))
            {
                neighbors.Add(target - Vector3Int.right);
            }
            //Down
            if (tilemap.HasTile(down))
            {
                neighbors.Add(target - Vector3Int.up);
            }
        }
        

        return neighbors;
    }
    //获取当前位置到终点的消耗
    private int GetHeuristic(Vector3Int posA, Vector3Int posB)
    {
        //print("Heuristic" + Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y));
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
        //print(pathSave.Count);
        Vector3Int current = _endPos;//current終點
        
        while (current != startPos)
        {
            //x = x+1;
            Vector3Int next = pathSave[current];
            if (turnScript.isWaitForPlayerMove)
            {
                tilemap2.SetTile(current, pathTile);//pathtile 淺綠格子
            }
            else if (turnScript.isWaitForPlayerAttack && AttackController.combo=="V")
            {
                V_energy++;
                //print("VVV" + V_energy);
            }
            road.Add(current);
            current = next;
            //if (x == work)
            //{ break; }
        }
        road.Add(startPos);
        road.Reverse();//Reverse倒轉 12345/54321
        /*foreach (var item in road)
            print(item);*/
        
            V_energy = V_energy - 1;
        
    }
    public void Clear()
    {
        search.Clear();
        cost.Clear();
        pathSave.Clear();
        hadSearch.Clear();
        road.Clear();
        V_energy = -1;
    }
    public void turnfalse()
    {
        hasStartPosSet = false;
        hasEndPosSet = false;
    }
    public void Scene1(int _from,int _from2)
    {
        //print("_from"+_from);
        GameObject.Find("碰撞層").GetComponent<TileTest>().obstacle_tiletest(_from);
        GameObject.Find("碰撞層").GetComponent<TileTest>().ground_tiletest(_from2);
    }
}