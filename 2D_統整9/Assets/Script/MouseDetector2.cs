using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading; //sleep用的
using UnityEngine.Tilemaps;
using System;
//這份東西是以期鈞的觸碰方格變色腳本(TileController)為基礎拿資料做成 不可單獨存在(無設線判定)
public class MouseDetector2 : MonoBehaviour
{

    public GameObject g;
    public Grid grid_;
    public Tilemap tilemap;
    private bool hasStartPosSet;//是否设置了起点
    private bool hasEndPosSet;//是否设置了终点
    private Vector3Int StartPos;//紀錄第一次點擊位置 避免同次點擊時出錯

    private bool go ;//是否设置了终点

    int mg = 0;//go的次數
    Vector3 b;

    //回合控制脚本(從期鈞那抓 配合start抓取物件)
    private TurnControl turnScript;
    // Start is called before the first frame update
    void Start()
    {
        hasStartPosSet = false;
        hasEndPosSet = false;
        //回合控制脚本
        turnScript = GameObject.FindGameObjectWithTag("System").GetComponent<TurnControl>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(turnScript.isWaitForPlayerMove)//在移動回合才進行內容
        {
            if (go == true)
            {
                goo(mg);
            }
            //assd();
        }*/
        
        //print("歸零嗎"+mg);
        
        //print("看看" + TileController.posMouseOnGrid);
    }
    public Vector3 assd()
    {
        if (Input.GetMouseButton(0))
        {

            if (hasStartPosSet == false)
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().Clear();
                //Thread.Sleep(500);
                //GameObject.Find("BFS").GetComponent<BreathFirst>().BFS();
                if (tilemap.HasTile(TileController.posMouseOnGrid))
                {
                    StartPos = TileController.posMouseOnGrid;//紀錄第一次點擊位置 避免同次點擊時出錯
                    Vector3 x = grid_.CellToWorld(TileController.posMouseOnGrid);
                    //x.x = x.x - 1;
                    x.y = x.y + 0.5f;
                    //g.transform.position = x;
                    hasStartPosSet = true;
                    //print("看看" + g.transform.position);

                }
            }
            else if (hasEndPosSet == false)
            {
                //Thread.Sleep(500);
                if (BreathFirst.HadWaySave.Contains(TileController.posMouseOnGrid) && TileController.posMouseOnGrid != StartPos)
                {
                    Vector3 x = grid_.CellToWorld(TileController.posMouseOnGrid);
                    //x.x = x.x - 1;
                    x.y = x.y + 0.5f;


                    hasEndPosSet = true;
                    //print("看看" + g.transform.position);
                    go = true;
                }
                else if (TileController.posMouseOnGrid == StartPos)
                { 
                
                }
            }
            else if (hasStartPosSet == true && hasEndPosSet == true)
            {
                /*foreach (var next in MapBehaviour.road)
                {
                    print("有跑?");
                    Thread.Sleep(50);
                    while (g.transform.position != grid_.CellToWorld(next))
                    {
                        print("有跑?");
                        g.transform.position = next;
                    }
                }*/
                //Thread.Sleep(500);
                hasStartPosSet = false;
                hasEndPosSet = false;
                print("有整回false嗎");
                GameObject.Find("BFS").GetComponent<BreathFirst>().Clear();
            }
        }
        return g.transform.position;
    }
    public bool Goo()
    {

       
        GGoo();
        return go;
    }
    int GGootime = 1;
    Vector3 fix;
    public void GGoo()
    {

        if (GGootime == 1)
        {
            fix = g.transform.position - grid_.CellToWorld(MapBehaviour.road[mg]);
            //print("fix :" + fix);
            GGootime = 2;
        }

        Vector3 xx = grid_.CellToWorld(MapBehaviour.road[MapBehaviour.road.Count - 1]);//終點



        if (g.transform.position != xx + fix)
        {
            Vector3 x = grid_.CellToWorld(MapBehaviour.road[mg]) + fix;//現在的位置
            Vector3 xxx = grid_.CellToWorld(MapBehaviour.road[mg + 1]) + fix;//下個位置
            Vector3 gy = Vector3.Lerp(x, xxx, 0.5f) - x;//位移距離 
            //PS: 這邊gy用0.1f，在地圖上方會出現錯誤(原因不明)
            g.transform.position = g.transform.position + gy;

            go = true;
            if (g.transform.position == xx + fix)
            {

                //print("多少呢" + grid_.WorldToCell(g.transform.position) + "YYY則是" + yyyy + "終點則是" + MapBehaviour.road[MapBehaviour.road.Count - 1]);

                //print("g :" + g.transform.position + " xxx :" + xxx + " xx :" + xx+" mg :"+mg);
                //print("到指定位置了");
                mg = 0;
                go = false;
                GGootime = 1;

                MapBehaviour.hasStartPosSet = false;
                MapBehaviour.hasEndPosSet = false;
                MapBehaviour.startPos = MapBehaviour.endPos;//調整位置的判定時機讓其他腳本抓取正確
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("move", 0);
                GameObject.Find("BFS").GetComponent<BreathFirst>().Clear();
                GameObject.Find("System").GetComponent<MapBehaviour>().Clear();
            }
            else if (g.transform.position == xxx)
            {
                mg = mg + 1;
                //print("MG" + mg);
                //print("x" + x + "xxx" + xxx);
            }
            else if (g.transform.position != xxx)
            {
                //print("g :" + g.transform.position + " xxx :" + xxx + " xx :" + xx + " mg :" + mg + "else");
            }
        }
        else
        {

            print("else!!");
        }
    }
}




