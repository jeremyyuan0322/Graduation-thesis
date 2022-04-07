using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class AttackController : MonoBehaviour
{

    private string root = "UI/Coin/";
    static public string combo;//改成static
    public static int count;

    public Color transparent = new Color(1, 1, 1, 0);//透明看不到
    public Color Non_transparent = new Color(1, 1, 1, 1);//非透明看得到

    public GameObject Coinobj;
    List<string> Coinlist = new List<string>(); //記錄玩家每個所案的按鈕
    public GameObject[] Bomb;
    private GameObject[] Bomb_C;
    public GameObject BA, A;//Bullet_A
    //public GameObject BB, B;//Bullet_B
    private GameObject[] BA_C, BA_CC;
    //private GameObject[] BB_C, BB_CC;
    public GameObject PL;
    public Grid grid_;
    public Tilemap tilemap;
    public Tilemap tilemap2;
    public GameObject ControlBox;//控制盒

    public Tilemap tilemap3;//抓碰撞層
    public Tilemap tilemap4;
    public Tile vine;//空的

    private bool hasStartPosSet;//是否设置了起点
    private bool hasEndPosSet;//是否设置了终点
    public bool ConfirmAttack = true;//確認輸入攻擊
    bool checkrange;//顯示攻擊範圍
    private bool go = false;//是否设置了终点
    private Vector3 gy;
    int mg = 0;//go的次數
    private Vector3 b;
    private List<Vector3Int> ATKroad = new List<Vector3Int>();
    //回合控制脚本(從期鈞那抓 配合start抓取物件)
    private TurnControl turnScript;
    // Start is called before the first frame update

    // Start is called before the first frame update
    public TileBase tile;


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
        if (turnScript.isWaitForPlayerAttack)
        {
            if (checkrange == true)
            {
                Compute_Energy();
               checkrange = false;//不能無限跑 不然會找不到路 很莫名
            }

        }
    }

    public void PotatoBtClick()//當點擊馬鈴薯的按鈕
    {
        if (count < 5)
        {
            count++;
            ShowCoin("PotatoCoin", count);
            AttackCombine("P");
        }
    }

    public void HumanBtClick()//當點擊人類的按鈕
    {
        if (count < 5)
        {
            count++;
            ShowCoin("HumanCoin", count);
            AttackCombine("H");
        }
    }

    public void VineBtClick()//當點擊藤蔓的按鈕
    {
        if (count < 5)
        {
            count++;
            ShowCoin("VineCoin", count);
            AttackCombine("V");
        }
    }

    public void AttackCombine(string coin)//攻擊組合
    {
        combo += coin;
        switch (combo)
        {
            
            case "H"://單格1點傷害
                AllSkillCar_Transparent();
                GameObject.Find("Card_NormalAttack").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("NormalAttackIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
            case "HH"://單格2點傷害
                AllSkillCar_Transparent();
                GameObject.Find("Card_Song").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("SongIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
            case "HHH"://單格3點傷害
                AllSkillCar_Transparent();
                GameObject.Find("Card_Punch").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("PunchIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
            case "P"://生命+3，本回合結束
                AllSkillCar_Transparent();
                GameObject.Find("Card_AddHP").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("AddHpIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
            case "PP"://能量+3，本回合結束
                AllSkillCar_Transparent();
                GameObject.Find("Card_AddEnergy").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("AddEnergyIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
            case "PPP"://周圍一圈2點傷害
                AllSkillCar_Transparent();
                GameObject.Find("Card_Rotation").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("RotationIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
            case "HHP"://三格2點傷害
                AllSkillCar_Transparent();
                GameObject.Find("Card_Boom").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("BoomIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
            case "PPH"://周圍第二圈2點傷害
                AllSkillCar_Transparent();
                GameObject.Find("Card_Cow").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("CowIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
            case "V"://創造地形
                AllSkillCar_Transparent();
                GameObject.Find("Card_Build").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("BuildIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
            
            case "VH"://拉
                AllSkillCar_Transparent();
                GameObject.Find("Card_Pull").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("PullIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
            case "PH"://推
                AllSkillCar_Transparent();
                GameObject.Find("Card_Push").GetComponent<Image>().color = Non_transparent;
                GameObject.Find("PushIF").GetComponent<Image>().color = Non_transparent;
                checkrange = true;
                break;
        }


    }


    void ShowCoin(string coin, int num)//顯示按鈕
    {
        if (num < 6)
        {
            if (coin == "PotatoCoin")
            {
                Object obj = Resources.Load(root + coin, typeof(GameObject));
                Coinobj = Instantiate(obj, ControlBox.transform.position, ControlBox.transform.rotation, ControlBox.transform) as GameObject;
                Coinobj.name += num;
                Coinobj.transform.localPosition = new Vector3((96f * (num - 1)) - 218f, 93f, 0);
                Coinlist.Add("PotatoCoin(Clone)" + num);//將按鈕名稱加入list

            }
            if (coin == "HumanCoin")
            {
                Object obj = Resources.Load(root + coin, typeof(GameObject));
                Coinobj = Instantiate(obj, ControlBox.transform.position, ControlBox.transform.rotation, ControlBox.transform) as GameObject;
                Coinobj.name += num;
                Coinobj.transform.localPosition = new Vector3((96f * (num - 1)) - 218f, 93f, 0);
                Coinlist.Add("HumanCoin(Clone)" + num);//將按鈕名稱加入list

            }
            if (coin == "VineCoin")
            {
                Object obj = Resources.Load(root + coin, typeof(GameObject));
                Coinobj = Instantiate(obj, ControlBox.transform.position, ControlBox.transform.rotation, ControlBox.transform) as GameObject;
                Coinobj.name += num;
                Coinobj.transform.localPosition = new Vector3((96f * (num - 1)) - 218f, 93f, 0);
                Coinlist.Add("VineCoin(Clone)" + num);//將按鈕名稱加入list

            }
            else
            {
                ///print("showcoin disable");
            }
        }
    }

    public static string comboformonster;
    public void AllCancelCoin()//取消按鈕
    {
        comboformonster = combo;
        while (count > 0)
        {
            count = count - 1;
            Coinlist.RemoveAt(count);
            Destroy(Coinobj);//移除最後一個按鈕
            if (Coinlist.Count != 0)
            {
                Coinobj = GameObject.Find(Coinlist[Coinlist.Count - 1]);//Coinobj為當前最後一筆紀錄的按鈕。
            }
            combo = combo.Substring(0, combo.Length - 1);
        }
        AllSkillCar_Transparent(); //透明場面上飛透明的技能卡與技能資訊卡
        GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 0);
        checkrange = false;
        GameObject.Find("BFS").GetComponent<BreathFirst>().Clear();
        GameObject.Find("System").GetComponent<MapBehaviour>().Clear();
    }
    public void AllSkillCar_Transparent()//透明場面上飛透明的技能卡與技能資訊卡
    {
        GameObject.Find("Card_NormalAttack").GetComponent<Image>().color = transparent;
        GameObject.Find("Card_Song").GetComponent<Image>().color = transparent;
        GameObject.Find("Card_Punch").GetComponent<Image>().color = transparent;
        GameObject.Find("Card_AddHP").GetComponent<Image>().color = transparent;
        GameObject.Find("Card_AddEnergy").GetComponent<Image>().color = transparent;
        GameObject.Find("Card_Rotation").GetComponent<Image>().color = transparent;
        GameObject.Find("Card_Boom").GetComponent<Image>().color = transparent;
        GameObject.Find("Card_Cow").GetComponent<Image>().color = transparent;
        GameObject.Find("Card_Build").GetComponent<Image>().color = transparent;
        GameObject.Find("Card_Pull").GetComponent<Image>().color = transparent;
        GameObject.Find("Card_Push").GetComponent<Image>().color = transparent;


        GameObject.Find("NormalAttackIF").GetComponent<Image>().color = transparent;
        GameObject.Find("SongIF").GetComponent<Image>().color = transparent;
        GameObject.Find("PunchIF").GetComponent<Image>().color = transparent;
        GameObject.Find("AddHpIF").GetComponent<Image>().color = transparent;
        GameObject.Find("AddEnergyIF").GetComponent<Image>().color = transparent;
        GameObject.Find("RotationIF").GetComponent<Image>().color = transparent;
        GameObject.Find("BoomIF").GetComponent<Image>().color = transparent;
        GameObject.Find("CowIF").GetComponent<Image>().color = transparent;
        GameObject.Find("BuildIF").GetComponent<Image>().color = transparent;
        GameObject.Find("PullIF").GetComponent<Image>().color = transparent;
        GameObject.Find("PushIF").GetComponent<Image>().color = transparent;
    }

    public void Create(Vector3Int _from)
    {
        //BA_C = GameObject.FindGameObjectsWithTag("Bullet_A");
        //BB_C = GameObject.FindGameObjectsWithTag("Bullet_B");
        if (combo == "H" || combo == "HH" || combo == "HHH"
            || combo == "PPP" || combo == "HHP" || combo == "PPH"
            ||combo=="VH" || combo == "PH")
        {
            //road.RemoveAt(0);
            b = grid_.CellToWorld(_from);
            b.y = b.y + 0.35f;
            Instantiate(BA, b, new UnityEngine.Quaternion(0, 0, 0, 0));//create
            if (combo == "H") Instantiate(Bomb[0], b, new UnityEngine.Quaternion(0, 0, 0, 0));//攻擊特效
            else if (combo == "HH") Instantiate(Bomb[1], b, new UnityEngine.Quaternion(0, 0, 0, 0));//攻擊特效
            else if (combo == "HHH") Instantiate(Bomb[2], b, new UnityEngine.Quaternion(0, 0, 0, 0));//攻擊特效
            else if (combo == "PPP") Instantiate(Bomb[3], b, new UnityEngine.Quaternion(0, 0, 0, 0));//攻擊特效
            else if (combo == "HHP") Instantiate(Bomb[4], b, new UnityEngine.Quaternion(0, 0, 0, 0));//攻擊特效
            else if (combo == "PPH") Instantiate(Bomb[5], b, new UnityEngine.Quaternion(0, 0, 0, 0));//攻擊特效
            //else if (combo == "VH") Instantiate(Bomb[6], b, new UnityEngine.Quaternion(0, 0, 0, 0));//攻擊特效
            //else if (combo == "PH") Instantiate(Bomb[6], b, new UnityEngine.Quaternion(0, 0, 0, 0));//攻擊特效



        }

        else if(combo == "V")
        {
            tilemap3.SetTile(_from, null);
            tilemap4.SetTile(_from, vine);
            GameObject.Find("碰撞層").GetComponent<TileTest>().obstacle_tiletest(Scenemapdata.obstacle);
            
        }
        //print("combo??" + combo);
    }

    public void Compute_Energy()//H
    {

        if ( combo == "H" || combo == "HH"||combo == "HHH"
            || combo == "P"|| combo == "PP"|| combo == "PPP"  
            || combo == "HHP"|| combo == "PPH"
            || combo == "V" || combo == "VH" || combo == "PH")
        {
            PlayerController.EnergyCosume = 0;
            ATKroad.Clear();
            GameObject.Find("BFS").GetComponent<BreathFirst>().Clear();
            if (combo == "H")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().H(MapBehaviour.checkPos);//H
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 1);

            }

            else if (combo == "HH")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().HH(MapBehaviour.checkPos);
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 2);
            }
            else if (combo == "HHH")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().HHH(MapBehaviour.checkPos);
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 3);
            }
            else if (combo == "P")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().P(MapBehaviour.checkPos);
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 2);
            }
            else if (combo == "PP")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().PP(MapBehaviour.checkPos);
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 1);
            }
            else if (combo == "PPP")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().PPP(MapBehaviour.checkPos);
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 3);
            }
            else if (combo == "HHP")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().HHP(MapBehaviour.checkPos);
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 3);
            }
            
            else if (combo == "PPH")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().PPH(MapBehaviour.checkPos);
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 4);
            }                                 
            else if (combo == "V")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().V(MapBehaviour.checkPos);
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", GameObject.Find("System").GetComponent<MapBehaviour>().V_energy+1);
            }
            else if (combo == "VH")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().VH(MapBehaviour.checkPos);
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 3);
            }
            
            else if (combo == "PH")
            {
                GameObject.Find("BFS").GetComponent<BreathFirst>().PH(MapBehaviour.checkPos);
                GameObject.Find("Player").GetComponent<PlayerController>().ShowUsedEnergy("attack", 3);
            }
            

        }
    }


    public int Calculate_relative_position(Vector3 _from)//計算相對位置  讓外部傳入vector3座標進來並算出所在象限
    {
        Vector3Int player;
        Vector3Int monster;
        player = MapBehaviour.checkPos;
        //print(player);
        monster = tilemap.WorldToCell(_from);
        //print(_from);
        int quadrant = 0;//象限英文單字QQ
        if (player.x < monster.x && player.y == monster.y)
        {
            quadrant = 1; //實際上是玩家在怪物的左方
        }
        if (player.x > monster.x && player.y == monster.y)
        {
            quadrant = 3; //實際上是玩家在怪物的右方
        }
        if (player.y < monster.y && player.x == monster.x)
        {
            quadrant = 2; //實際上是玩家在怪物的下方
        }
        if (player.y > monster.y && player.x == monster.x)
        {
            quadrant = 4; //實際上是玩家在怪物的上方
        }
        
        return quadrant;
    }
   


    public void Destroy_bullet()//Destroy Bullet
    {
        if (combo == "H" ||  combo == "HH"||combo == "HHH"  
            || combo == "PPP" || combo == "HHP"|| combo == "PPH"
            || combo == "V" || combo=="PH"||combo=="VH")
        {
            BA_C = GameObject.FindGameObjectsWithTag("Bullet_A");
            Bomb_C = GameObject.FindGameObjectsWithTag("Bomb"); 
            //print("length" + BA_C.Length);

            for (int i = 0; i < BA_C.Length; i++)
            {
                if (BA_C[i].gameObject.tag == "Bullet_A")
                {
                    Destroy(BA_C[i].gameObject, 0.1f);

                    //print("BA_C" + BA_C[i]);
                }
                

            }
            for (int i = 0; i < Bomb_C.Length; i++)
            {
                
                if (Bomb_C[i].gameObject.tag == "Bomb")
                {
                    Destroy(Bomb_C[i].gameObject, 0.1f);
                }

            }
            //Destroy(BA_C);
        }
        

    }
    public void Clear()
    {

        foreach (var i in ATKroad)
        {
            print("atk" + i);

            tilemap.SetColor(i, Color.white);
            tilemap2.SetColor(i, Color.white);
        }
    }

}
