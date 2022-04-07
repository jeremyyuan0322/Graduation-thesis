using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    TurnControl turnScript;
    private int count = 0;

    void Start()
    {
        turnScript = GameObject.Find("System").GetComponent<TurnControl>();
    }

    public void BtContinueClick(string DestroyPanalName)
    {
        if (DestroyPanalName != null)
        {
            DestroyThisPanel(DestroyPanalName);
        }
        turnScript.GameStartConfirm();
    }

    public void Cartoon(string str)
    {
        switch (str)
        {
            case "Page1":
                BtNextCartoonPage(1, 4);
                break;
            case "Page2":
                BtNextCartoonPage(2, 11);
                break;
            case "Page3": //第一關漫畫結尾
                BtNextCartoonPage(3, 8);
                break;
            case "Page4":
                BtNextCartoonPage(4, 4);
                break;
            case "Page5"://第二關漫畫結尾
                BtNextCartoonPage(5, 3);
                break;
            case "Page6":
                BtNextCartoonPage(6, 4);
                break;
            case "Page7":
                BtNextCartoonPage(7, 8);
                break;
            case "Page8"://第二關漫畫結尾
                BtNextCartoonPage(8, 6);
                break;
        }
    }
    public void BtNextCartoonPage(int page, int num)
    {
        if (count < num)
        {
            count++;
            GameObject.Find("Page" + page + "_" + count).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        /*
        else if (count == num&&page==3)
        {
            UIManager2.Instance.ClosePanel("Page" + page);
            UIManager2.Instance.ShowPanel("SHIH/Panel");
        }
        */
        else if (count == num)
        {
            UIManager2.Instance.ClosePanel("Page" + page);
            if ((page == 3) || (page == 5) || (page == 8))
            {
                turnScript.GameStartConfirm();
            }
        }
        
    }

    public void DestroyThisPanel(string name)
    {
        UIManager2.Instance.ClosePanel(name);
            
    }

}
