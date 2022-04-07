using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    //Rigidbody2D rb2D;//剛體 以便可以被施力

    /*
    [Header("錢幣數量顯示文字UI")]
    public Text MoneyNumText;//這不是string字串喔

    [Header("貧富顯示文字UI")]
    public Text MoneyLevelText;//這不是string字串喔
    */

    [Header("最大能量顯示文字UI")]
    public Text MaxEnergyText;//這不是string字串喔

    [Header("現有能量顯示文字UI")]
    public Text EnergyText;//這不是string字串喔

    [Header("目前消耗能量顯示文字UI")]
    public Text CurrentEnergyText;//這不是string字串喔


    public int Hp = 5;//血量
    public int MaxHp = 5;//血量
    // public GameObject HpPic; //血條增減控制
    public int MaxEnergy = 99; //能量最大雞腿數量
    static public int Energy = 60; //現有能量雞腿數量
    static public int EnergyCosume; //玩家消耗多少的能量雞腿

    //private int Money;//錢幣量


    //回合控制脚本
    private TurnControl turnScript;

    // Start is called before the first frame update
    void Start()
    {
        turnScript = GameObject.FindGameObjectWithTag("System").GetComponent<TurnControl>();

        //rb2D = GetComponent<Rigidbody2D>();
        //rb2D = 取得組件<剛體2D> (); 
        //MoneyLevelText.text = "";//清空過關文字的內容
        //setMoneyText();//顯示　目前錢數:0
        setEnergyText();//顯示　目前能量數 能量/最大能量

    }

    void Update()
    {
        setHPImage();
        if (Hp >= MaxHp)
        {
            Hp = MaxHp;
        }
        if (Hp < 0)
        {
            Hp = 0;
        }
        if (Energy >= MaxEnergy)
        {
            Energy = MaxEnergy;
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(name + "觸發了" + other.name);

        /*
        if (other.CompareTag("PickUp"))
        //其他.比較標籤("PickUp")
        {
            other.gameObject.SetActive(false);
            //其他.遊戲物件.設定活躍狀態(否);

            Money = Money + 1;
            setMoneyText();
        }
        */

        if (other.CompareTag("Energy"))
        //其他.比較標籤("PickUp")
        {
            other.gameObject.SetActive(false);
            //其他.遊戲物件.設定活躍狀態(否);

            Energy = Energy + 10;
            setEnergyText();
        }

        if (other.CompareTag("Monster_Col1")) //玩家跟怪物1的攻擊碰撞框相碰時血量會減少
        {
            if (Hp > 0)
            {
                Hp -= 1;
            }
        }

        if (other.CompareTag("Monster_Col2")) //玩家跟怪物2的攻擊碰撞框相碰時血量會減少
        {
            if (Hp > 0)
            {
                Hp -= 1;
            }
        }
        if (other.CompareTag("Monster_Col3")) //玩家跟怪物2的攻擊碰撞框相碰時血量會減少
        {
            if (Hp > 0)
            {
                Hp -= 3;
            }
        }
        if (other.CompareTag("Monster_Col4")) //玩家跟怪物2的攻擊碰撞框相碰時血量會減少
        {
            if (Hp > 0)
            {
                Hp -= 3;
            }
        }
        if (other.CompareTag("Monster_Col5")) //玩家跟怪物2的攻擊碰撞框相碰時血量會減少
        {
            if (Hp > 0)
            {
                Hp -= 5;
            }
        }

    }

    void setHPImage()//顯示血量圖片
    {
        if (Hp == 5)
        {
            for (int i = 1; i <= 5; i++)
            { GameObject.Find("HP_" + i).GetComponent<Image>().color = new Color(1, 1, 1, 1); }
        }
        if (Hp == 4)
        {
            for (int i = 1; i <= 4; i++)
            {
                GameObject.Find("HP_" + i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            GameObject.Find("HP_5").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        if (Hp == 3)
        {
            for (int i = 1; i <= 3; i++)
            { GameObject.Find("HP_" + i).GetComponent<Image>().color = new Color(1, 1, 1, 1); }
            GameObject.Find("HP_5").GetComponent<Image>().color = new Color(1, 1, 1, 0);
            GameObject.Find("HP_4").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        if (Hp == 2)
        {
            GameObject.Find("HP_1").GetComponent<Image>().color = new Color(1, 1, 1, 1);
            GameObject.Find("HP_2").GetComponent<Image>().color = new Color(1, 1, 1, 1);
            for (int i = 3; i <= 5; i++)
            { GameObject.Find("HP_" + i).GetComponent<Image>().color = new Color(1, 1, 1, 0); }
        }
        if (Hp == 1)
        {
            GameObject.Find("HP_1").GetComponent<Image>().color = new Color(1, 1, 1, 1);
            for (int i = 2; i <= 5; i++)
            { GameObject.Find("HP_" + i).GetComponent<Image>().color = new Color(1, 1, 1, 0); }
        }
        if (Hp == 0)
        {
            for (int i = 1; i <= 5; i++)
            { GameObject.Find("HP_" + i).GetComponent<Image>().color = new Color(1, 1, 1, 0); }
        }
    }

    /*
    void setMoneyText()//顯示金幣
    {
        MoneyNumText.text = "你的摳摳:" + Money.ToString();
        //錢數量UI.文字 = "目前分數:" + 分數.轉字串 ( );

        if (Money >= 0 && Money < 5)
        {
            MoneyLevelText.text = "有夠窮";
        }
        else if (Money >= 5 && Money < 9)
        {
            MoneyLevelText.text = "夠窮";
        }
        else if (Money >= 9 && Money < 13)
        {
            MoneyLevelText.text = "窮";
        }
    }
    */

    public void ShowUsedEnergy(string type,int num)
    {
        if (type == "move")//移動
        {
            EnergyCosume = 0;
            EnergyCosume = EnergyCosume + num;
            setCurrentEnergyText();
        }
        if(type == "attack")//攻擊
        {
            EnergyCosume = 0;
            EnergyCosume += num;
            setCurrentEnergyText();
        }
    }

    public void CalculatePlayerEnergy() //計算你消耗的能量
    {
        Energy = Energy - EnergyCosume;
        EnergyCosume = 0;
        setEnergyText();
        setCurrentEnergyText();
    }

    public void setEnergyText()//顯示最大值能量與現有能量文字
    {
        MaxEnergyText.text = "/" + MaxEnergy.ToString();
        EnergyText.text = Energy.ToString();
    }


    public void setCurrentEnergyText()//顯示目前消耗的能量文字
    {
        CurrentEnergyText.text = "*" + EnergyCosume.ToString();
    }
}