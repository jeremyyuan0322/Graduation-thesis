using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AT : MonoBehaviour
{

    private string root = "UI/Coin/";
    private string combo;
    public int count;
    public GameObject punch;
    public GameObject drow;
    public GameObject boom;
    public Color transparent = new Color(1, 1, 1, 0);
    public Color Non_transparent = new Color(1, 1, 1, 1);

    public GameObject Coinobj;
    List<string> Coinlist = new List<string>(); //記錄玩家每個所案的按鈕

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PotatoBtClick()//當點擊馬鈴薯的按鈕
    {
        if (count < 7)
        {
            count++;
            ShowCoin("PotatoCoin", count);
            AttackCombine("P");
        }
    }

    public void HumanBtClick()//當點擊人類的按鈕
    {
        if (count < 7)
        {
            count++;
            ShowCoin("HumanCoin", count);
            AttackCombine("H");
        }
    }

    void AttackCombine(string coin)//攻擊組合
    {
        combo += coin;
        switch (combo)
        {
            case "PPH":
                Debug.Log("PPH");
                //combo = "";
                boom.GetComponent<Image>().color = Non_transparent;
                break;
            case "HPH":
                Debug.Log("HPH");
                //combo = "";
                punch.GetComponent<Image>().color = Non_transparent;
                break;
        }


    }

    void ShowCoin(string coin, int num)//顯示按鈕
    {
        if (num < 8)
        {
            if (coin == "PotatoCoin")
            {
                Object obj = Resources.Load(root + coin, typeof(GameObject));
                Coinobj = Instantiate(obj) as GameObject;
                Coinobj.name += num;
                Coinobj.transform.position = new Vector3((1.5f * num) - 1.5f, -2.5f, 0);
                Coinlist.Add("PotatoCoin(Clone)" + num);//將按鈕名稱加入list

            }
            if (coin == "HumanCoin")
            {
                Object obj = Resources.Load(root + coin, typeof(GameObject));
                Coinobj = Instantiate(obj) as GameObject;
                Coinobj.name += num;
                Coinobj.transform.position = new Vector3((1.5f * num) - 1.5f, -2.5f, 0);
                Coinlist.Add("HumanCoin(Clone)" + num);//將按鈕名稱加入list

            }
            else
            { print("showcoin disable"); }
        }
    }

    public void CancelCoin()//取消按鈕
    {
        if (count != 0)
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
    }
}