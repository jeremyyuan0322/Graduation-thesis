using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TurnControl : MonoBehaviour
{
    //玩家移動階段
    public bool isWaitForPlayerMove = true;
    //玩家攻擊階段
    public bool isWaitForPlayerAttack = false;
    //怪物行動階段
    public bool isEnemyAction = false;

    public GameObject MoveGUI;//拖入圖片
    private Image MovePic;
    public GameObject AttackGUI;//拖入圖片
    private Image AttackPic;
    public GameObject EnemyGUI;//拖入圖片
    private Image EnemyPic;
    public AudioSource Audio;
    public AudioClip[] AudioCl; //bgm


    public int ColNum_Monster1 = 0; //怪物1攻擊碰撞框的數量;
    public int ColNum_Monster2 = 0; //怪物2攻擊碰撞框的數量;
    public int ColNum_Monster3 = 0; //怪物3攻擊碰撞框的數量;
    public int ColNum_Monster4 = 0; //怪物4攻擊碰撞框的數量;
    public int ColNum_Monster5 = 0; //怪物5攻擊碰撞框的數量;

    public static int NextLevelNum = 0;//下一關
    public int TurnCount = 0; //現在是第幾回合

    public GameObject PauseMenu; //暫停選單

    public CanvasGroup PanelBCanvasGroup;
    private Button CheckBT;
    private Button TurnBT;
    //获取玩家及敌人脚本的引用 
    private PlayerController playerScript;
    private AttackController attackScript;

    public int xx = 0;//給新手教學使用的前幾次特殊次數
    public float m_Time = 2f;

    public GameObject[] Monster;
    //定义游戏状态枚举  
    public enum GameState
    {
        Wait,//玩家點選技能操作ui階段  
        Game,//戰鬥游戏中
        Over//游戏结束
    }
    //游戏初始状态
    public static GameState currentState;


    void Start()
    {
        currentState = GameState.Wait;
        //获取玩家及敌人脚本引用 
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        attackScript = GameObject.FindGameObjectWithTag("Player").GetComponent<AttackController>();
        PanelBCanvasGroup = GameObject.Find("PanelB").GetComponent<CanvasGroup>();
        CheckBT = GameObject.Find("CheckBT").GetComponent<Button>();
        TurnBT  = GameObject.Find("SwithBT").GetComponent<Button>();
        Audio = GameObject.Find("AudioEffect").GetComponent<AudioSource>();

        //獲取Three states的圖片引用
        MovePic = MoveGUI.GetComponent<Image>();

        AttackPic = AttackGUI.GetComponent<Image>();

        EnemyPic = EnemyGUI.GetComponent<Image>();
        //currentState = GameState.Wait;
    }

    public void GameStartConfirm()
    {
        if (currentState == GameState.Wait)
        {
            currentState = GameState.Game;
            GameObject.FindGameObjectWithTag("System").GetComponent<AnimationController>().Controller_box_open();
            PlayerMoveState();
            if (SceneController.SceneName == "Scene1")
            {
                UIManager2.Instance.ShowPanel("SHIH/Panel");
            }
            if (SceneController.SceneName == "Scene2")
            {
                UIManager2.Instance.ShowPanel("Notice_Sence2");
            }
            if (SceneController.SceneName == "Scene3")
            {
                UIManager2.Instance.ShowPanel("Notice_Sence3");
            }
            if (SceneController.SceneName == "Scene4")
            {
                UIManager2.Instance.ShowPanel("Notice_Sence4");
            }
        }
        else
        {
            Debug.Log("現在不是Gamestate.wait");
        }
    }

    public void TurnChange()//轉換階段
    {
        if (currentState == GameState.Game)
        {
            if (isWaitForPlayerMove == true)
            {
                
                GameObject.Find("BFS").GetComponent<BreathFirst>().Clear();
                GameObject.Find("System").GetComponent<MapBehaviour>().Clear();     
                GameObject.Find("System").GetComponent<MapBehaviour>().turnfalse();
                if (SceneController.SceneName == "Scene1")
                {
                    if (TurnCount == 0)
                    {
                        UIManager2.Instance.ShowPanel("Notice_Attack_First");
                    }
                    if (TurnCount == 1)
                    {
                        UIManager2.Instance.ShowPanel("Notice_Attack_Second");
                    }
                    if (TurnCount == 2)
                    {
                        UIManager2.Instance.ShowPanel("Notice_HP");
                    }
                }



                    PlayerAttackState();
            }
            else if (isWaitForPlayerAttack == true)
            {
                EnemyState();
            }
        }
        else
        {
            Debug.Log("現在不是Gamestate.wait");
        }
    }


    public void PlayerMoveState() //玩家移動階段
    {
        isEnemyAction = false;
        isWaitForPlayerAttack = false;
        PanelBCanvasGroup.interactable = false;
        CheckBT.interactable = true;
        //playerScript.CalculatePlayerEnergy();
        isWaitForPlayerMove = true;
    }

    public void PlayerAttackState() //玩家攻擊階段
    {
        isWaitForPlayerMove = false;
        isEnemyAction = false;
        PanelBCanvasGroup.interactable = true;
        CheckBT.interactable = true;
        //playerScript.CalculatePlayerEnergy();
        isWaitForPlayerAttack = true;
    }

    public void EnemyState() //敵人階段
    {
        attackScript.AllCancelCoin();
        isWaitForPlayerMove = false;
        isWaitForPlayerAttack = false;
        PanelBCanvasGroup.interactable = false;
        CheckBT.interactable = false;
        PlayerStateClear(); //在敵人回合時，清除前面玩家移動、攻擊所產生的UI
        isEnemyAction = true;
        StartCoroutine(EnemyTime());
    }
    public void WhenGO1() //玩家移動時
    {
        
        CheckBT.interactable = false;
        TurnBT.interactable = false;
       
    }
    public void WhenGO2() //玩家移動時
    {

        CheckBT.interactable = true;
        TurnBT.interactable = true;

    }
    public void PlayerStateClear()//在敵人回合時，清除前面玩家移動、攻擊所產生的UI，並把怪物的攻擊移動限制解除。
    {
        GameObject.Find("PunchIF").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameObject.Find("BoomIF").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        if (SceneController.SceneName == "Scene1") //Scene1有兩隻怪物
        {
            if (Monster[0]) GameObject.Find("corn1").GetComponent<MonsterController>().Movelock = false;
            if (Monster[1]) GameObject.Find("corn2").GetComponent<MonsterController2>().Movelock = false;
        }

        if (SceneController.SceneName == "Scene2")
        {
            if (Monster[0]) GameObject.Find("lb1").GetComponent<MonsterController>().Movelock = false;
            if (Monster[1]) GameObject.Find("lb2").GetComponent<MonsterController2>().Movelock = false;
            if (Monster[2]) GameObject.Find("fort1").GetComponent<FortController>().Attacklock = false;
            if (Monster[3]) GameObject.Find("fort2").GetComponent<FortController>().Attacklock = false;
            if (Monster[4]) GameObject.Find("LbBoss").GetComponent<BossController>().Movelock = false;
        }

        if (SceneController.SceneName == "Scene4")
        {
            if (Monster[0])
            {
                if ((TurnCount % 2) == 0)
                {
                    GameObject.Find("FishBoss").GetComponent<FishBossController>().PredictAttacklock = false;
                }
                if ((TurnCount % 2) == 1)
                {
                    GameObject.Find("FishBoss").GetComponent<FishBossController>().Attacklock = false;
                }
            }
        }
    }

    public void EnemyStateClear()//在敵人回合時，清除怪物產生的碰撞框及其他動作
    {
        for (int i = 0; i < ColNum_Monster1; i++)
        {
            Destroy(GameObject.Find("Monster_Col1(Clone)" + i));
            if (i == ColNum_Monster1 - 1)
            {
                ColNum_Monster1 = 0;
                break;
            }
        }

        for (int i = 0; i < ColNum_Monster2; i++)
        {
            Destroy(GameObject.Find("Monster_Col2(Clone)" + i));
            if (i == ColNum_Monster2 - 1)
            {
                ColNum_Monster2 = 0;
                break;
            }
        }
        for (int i = 0; i < ColNum_Monster3; i++)
        {
            Destroy(GameObject.Find("Monster_Col3(Clone)" + i));
            if (i == ColNum_Monster3 - 1)
            {
                ColNum_Monster3 = 0;
                break;
            }
        }
        for (int i = 0; i < ColNum_Monster4; i++)
        {
            Destroy(GameObject.Find("Monster_Col4(Clone)" + i));
            if (i == ColNum_Monster4 - 1)
            {
                ColNum_Monster4 = 0;
                break;
            }
        }
        for (int i = 0; i < ColNum_Monster5; i++)
        {
            Destroy(GameObject.Find("Monster_Col5(Clone)" + i));
            if (i == ColNum_Monster5 - 1)
            {
                ColNum_Monster5 = 0;
                break;
            }
        }
    }

    /*
    public bool EnemyTime()
    {
        m_Time -= Time.deltaTime;

        if (m_Time <= 0)
        {
            EnemyStateClear();
            isEnemyAction = false;
            PlayerMoveState();
            m_Time = 2f;
        }
        return isEnemyAction;

    }
    */

    IEnumerator EnemyTime()
    {
        yield return new WaitForSeconds(2);
        EnemyStateClear();
        isEnemyAction = false;
        TurnCount++;
        PlayerMoveState();
    }

    IEnumerator VictoryShown()
    {
        Audio.clip = AudioCl[1];
        Audio.Play();
        Audio.loop = false;
        UIManager.Instance.ShowPanel("Victory");
        yield return new WaitForSeconds(6);
        GameObject.FindGameObjectWithTag("System").GetComponent<SceneController>().NextLevel();
    }

    IEnumerator LoseShown()
    {
        Audio.clip = AudioCl[2];
        Audio.Play();
        Audio.loop = false;
        UIManager.Instance.ShowPanel("Lose");
        yield return new WaitForSeconds(6);
        Time.timeScale = 0;
    }

    public void EnergyConfirm()
    {
        if (isWaitForPlayerMove)
        {

        }

        else if (isWaitForPlayerAttack)
        {

        }
    }

    void Update()
    {
        if (currentState == GameState.Game)
        {
            if (MapBehaviour.go == true)
            {
                WhenGO1();
            }
            else
            {
                WhenGO2();
            }
            if (SceneController.SceneName == "Scene3") //第三關失敗條件
            {
                if (TurnCount <= 7)
                {
                    GameObject.Find("TurnDeadCount").GetComponent<Text>().text = Convert.ToString(7 - TurnCount);
                    if (Convert.ToInt32(GameObject.Find("TurnDeadCount").GetComponent<Text>().text) <= 0)
                    {
                        Debug.Log("你已經輸了");
                        currentState = GameState.Over;
                        StartCoroutine(LoseShown());
                    }
                }
            }

            //如果玩家生命值为0，则游戏结束  
            if (playerScript.Hp <= 0)
            {
                Debug.Log("你已經輸了");
                currentState = GameState.Over;
                StartCoroutine(LoseShown());
            }

            if (SceneController.SceneName == "Scene1") //第一關通關條件
            {
                if ((Monster[0] || Monster[1]) == false)
                {
                    Debug.Log("你已經贏了");
                    currentState = GameState.Wait;
                    NextLevelNum = 2;
                    StartCoroutine(VictoryShown());
                }
            }

            if (SceneController.SceneName == "Scene2") //第二關通關條件
            {
                if ((Monster[0] || Monster[1] || Monster[2] || Monster[3] || Monster[4]) == false)
                {
                    Debug.Log("你已經贏了");
                    currentState = GameState.Wait;
                    NextLevelNum = 3;
                    StartCoroutine(VictoryShown());
                }
            }

            if (SceneController.SceneName == "Scene3") //第三關通關條件
            {
                if (GameObject.Find("Player").GetComponent<Transform>().position == new Vector3(-14,5.5f,0))
                {
                    Debug.Log("你已經贏了");
                    currentState = GameState.Wait;
                    NextLevelNum = 4;
                    StartCoroutine(VictoryShown());
                }
            }
            if (SceneController.SceneName == "Scene4") //第三關通關條件
            {
                if (GameObject.Find("Player").GetComponent<Transform>().position == new Vector3(-5.5f, 5.25f, 0))
                {
                    Debug.Log("你已經贏了");
                    currentState = GameState.Wait;
                }
            }
        }

        if (isWaitForPlayerMove == true)
        {
            MovePic.sprite = Resources.Load("UI/State/move", typeof(Sprite)) as Sprite;
            AttackPic.sprite = Resources.Load("UI/State/attack_BW", typeof(Sprite)) as Sprite;
            EnemyPic.sprite = Resources.Load("UI/State/enemy_BW", typeof(Sprite)) as Sprite;
        }

        else if (isWaitForPlayerAttack == true)
        {
            MovePic.sprite = Resources.Load("UI/State/move_BW", typeof(Sprite)) as Sprite;
            AttackPic.sprite = Resources.Load("UI/State/attack", typeof(Sprite)) as Sprite;
        }

        else if (isEnemyAction == true)
        {
            AttackPic.sprite = Resources.Load("UI/State/attack_BW", typeof(Sprite)) as Sprite;
            EnemyPic.sprite = Resources.Load("UI/State/enemy", typeof(Sprite)) as Sprite;
        }

    }

}