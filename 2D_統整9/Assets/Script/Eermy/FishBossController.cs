using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class FishBossController : MonoBehaviour
{
    public GameObject col;
    public GameObject player;

    public Tilemap tilemap;
    public TileBase AttackTile;//紅色tile
    public TileBase noneTile; //回復原狀
    public Grid grid_;
    public Animator animator;

    public int NumCount; //現在是哪一回合
    private Dictionary<int, Vector3> Dic_AttackPoint = new Dictionary<int, Vector3>();//紀錄哪幾個點是可以攻擊的
    private int[] randomArray; //亂數存取
    private Vector3[] AttackRange; //怪物攻擊的點
    private List<Vector3Int> AttackSize = new List<Vector3Int>();//攻擊的範圍
    private List<Vector3Int> AttackStorage = new List<Vector3Int>();//攻擊暫存

    private Vector3Int PlayerPosition;
    public bool PredictAttacklock = true; //預測攻擊
    public bool Attacklock = true;
    private TurnControl turnScript;

    // Start is called before the first frame update
    void Start()
    {
        Dic_AttackPoint = new Dictionary <int, Vector3>
        {
            { 0, new Vector3(-0.5f, -2.75f, 0) }, { 1, new Vector3(2, -1.5f, 0) }, { 2, new Vector3(4, -0.5f, 0) }, { 3, new Vector3(5.5f, 0.25f, 0) }, //第一排石頭位置
            { 4, new Vector3(-0.5f, -0.75f, 0) }, { 5, new Vector3(1.5f, 0.25f, 0) }, { 6, new Vector3(3.5f, 1.25f, 0) }, { 7, new Vector3(5.5f, 2.25f, 0) }, { 8, new Vector3(3.5f, 3.25f, 0) }, //第二排石頭位置
            { 9, new Vector3(-3.5f, -1.25f, 0) }, { 10, new Vector3(-2, 0.5f, 0) }, { 11, new Vector3(-1, 2, 0) }, { 12, new Vector3(-0.5f, 3.25f, 0) }, { 13, new Vector3(1.5f, 4.25f, 0) } //第三排石頭位置
        };
        turnScript = GameObject.FindGameObjectWithTag("System").GetComponent<TurnControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnControl.currentState == TurnControl.GameState.Game)
        {
            if (turnScript.isEnemyAction)
            {
                
                if (!PredictAttacklock)
                {
                    if (NumCount > 6)
                    {
                        NumCount = 6;
                    }
                    FishBoss_PredictAttack(NumCount); //隨機預測“第幾回合”顆石頭座標）
                    NumCount++; //每過一回合攻擊數+1;
                }
                
                animator.SetBool("Attack", false);
                if (!Attacklock)
                {
                    animator.SetBool("Attack", true);
                    FishBoss_Attack(PlayerPosition);
                }
            }
        }
    }

    private bool FishBoss_Attack(Vector3Int Pos)
    {
        while (AttackStorage.Count > 0)
        {
            tilemap.SetTile(AttackStorage[0], noneTile);
            Vector3 obj = grid_.CellToWorld(AttackStorage[0]);
            GameObject Obj = Instantiate(col, obj, new Quaternion(0, 0, 0, 0));
            Obj.name += turnScript.ColNum_Monster5;
            turnScript.ColNum_Monster5++;
            AttackStorage.RemoveAt(0);
        }
        Attacklock = true;
        return Attacklock;
    }


    private bool FishBoss_PredictAttack(int num)
    {
        randomArray = new int[num];
        AttackRange = new Vector3[num + 1];
        for (int i = 0; i < num; i++)
        {
            randomArray[i] = Random.Range(0, 14);   //亂數產生，亂數產生的範圍是0~13
            AttackRange[i] = Dic_AttackPoint[randomArray[i]];

            for (int j = 0; j < i; j++)
            {
                while (randomArray[j] == randomArray[i])    //檢查是否與前面產生的數值發生重複，如果有就重新產生
                {
                    j = 0;  //如有重複，將變數j設為0，再次檢查 (因為還是有重複的可能)
                    randomArray[i] = Random.Range(0, 14);   //重新產生，存回陣列，亂數產生的範圍是1~13
                    AttackRange[i] = Dic_AttackPoint[randomArray[i]];
                }
            }
        }
        AttackRange[num] = player.transform.position;

        for (int i = 0; i <= num; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    Vector3Int AttackPosition = tilemap.WorldToCell(AttackRange[i]);
                    AttackSize.Add(AttackPosition + new Vector3Int(x, y, 0));
                    //print(Pos + new Vector3Int(x, y, 0));
                }

            }
        }
        while (AttackSize.Count > 0)
        {
            tilemap.SetTile(AttackSize[0], AttackTile);
            AttackStorage.Add(AttackSize[0]);
            AttackSize.RemoveAt(0);
        }

        PredictAttacklock = true;
        return PredictAttacklock;
    }

}
