using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Text pln;
    public GameObject PauseMenu; //主選單
    public GameObject SkillList; //技能表

    public static string SceneName;
    public static string SceneInstruction;
    // Start is called before the first frame update
    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;

        if (SceneName == "Scene1")
        {
            UIManager2.Instance.ShowPanel("Page3");
            //UIManager2.Instance.ShowPanel("Page2");
            //UIManager2.Instance.ShowPanel("Page1");
        }
        if (SceneName == "Scene2")
        {
            UIManager2.Instance.ShowPanel("Page5");
            //UIManager2.Instance.ShowPanel("Page4");
        }
        if (SceneName == "Scene3")
        {
            UIManager2.Instance.ShowPanel("Temp_Cartoon");
        }
        if (SceneName == "Scene4")
        {
            UIManager2.Instance.ShowPanel("Page8");
            //UIManager2.Instance.ShowPanel("Page7");
            //UIManager2.Instance.ShowPanel("Page6");
        }

        //UIManager.Instance.ShowPanel("Notice_move");   //移動提示
        //UIManager.Instance.ShowPanel("Notice_AttackSingle");  //單體攻擊提示
        //UIManager.Instance.ShowPanel("Notice_AttackMul");  //十字攻擊提示


    }

    // Update is called once per frame
    void Update()
    {
        pln.text = SaveData.PName.ToString();

        if (PauseMenu.activeSelf == false)
        {
            if (Input.GetKeyUp(KeyCode.Escape)) //暫停遊戲，呼叫主選單
            {
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
            }

            if (SkillList.activeSelf == false)
            {
                if (Input.GetKeyDown(KeyCode.Tab)) //不暫停遊戲，呼叫技能表
                {
                    SkillList.SetActive(true);
                }
            }
            if (SkillList.activeSelf == true)
            {
                if (Input.GetKeyUp(KeyCode.Tab)) //不暫停遊戲，呼叫技能表
                {
                    SkillList.SetActive(false);
                }
            }
        }

    }

    public void ContinueGame() //點擊繼續按鈕
    {
        if (PauseMenu.activeSelf == true)
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void RestartGame()//點擊重新開始按鈕
    {
        Time.timeScale = 1;
        SceneInstruction = "RestartGame";
        SceneManager.LoadScene("Initial");
    }

    public void QuitGame()//點擊離開遊戲按鈕
    {
        Application.Quit();//打包编译后退出
    }

    public void BackToMainMenu()//回到主選單，選擇關卡
    {
        Time.timeScale = 1;
        SceneInstruction = "BackToMainMenu";
        SceneManager.LoadScene("Initial");
    }

    public void NextLevel()//前往下一關
    {
        Time.timeScale = 1;
        SceneInstruction = "NextLevel";
        SceneManager.LoadScene("Initial");
    }
}
