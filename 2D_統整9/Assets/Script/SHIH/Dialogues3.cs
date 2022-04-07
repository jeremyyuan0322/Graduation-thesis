using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.Collections.Generic;//用到了容器类  

public class Dialogues3 : MonoBehaviour
{
    //这是场景中的各个物体
    public GameObject roleA;
    public GameObject roleB;
    public GameObject roleC;
    public GameObject roleName;
    public GameObject detail;
    public GameObject Image;
    private List<string> dialogues_list;//存放dialogues的list
    private int dialogue_index = 0;//对话索引
    private int dialogue_count = 0;//对话数量
    private string role;//当前在说话的角色
    private string role_detail;//当前在说话的内容。

    public void Start()
    {
        //变量初始化
        /*roleA = GameObject.Find("Canvas/飛機頭");
        roleB = GameObject.Find("Canvas/馬鈴薯");
        roleName = GameObject.Find("Canvas/SHIH/Panel/Image/roleName");
        detail = GameObject.Find("Canvas/SHIH/Panel/Image/detail");
        Image = GameObject.Find("Canvas/SHIH/Panel/Image");*/
        roleA.SetActive(false);
        roleB.SetActive(false);
        Image.SetActive(false);
        XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器”  
        dialogues_list = new List<string>();//初始化存放dialogues的list
        //载入资源文件
        string data = Resources.Load("dialogues3").ToString();//注意这里没有后缀名xml。你可以看看编辑器中，也是不带后缀的。因此不要有个同名的其它格式文件注意！
        //如果Resources下又有目录那就：Resources.Load("xx\\xx\\dialogues").ToString()
        xmlDocument.LoadXml(data);//载入这个xml  
        XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("dialogues").ChildNodes;//选择<dialogues>为根结点并得到旗下所有子节点  
        foreach (XmlNode xmlNode in xmlNodeList)//遍历<dialogues>下的所有节点<dialogue>压入List
        {
            XmlElement xmlElement = (XmlElement)xmlNode;//对于任何一个元素，其实就是每一个<dialogue>  
            dialogues_list.Add(xmlElement.ChildNodes.Item(0).InnerText + "," + xmlElement.ChildNodes.Item(1).InnerText);
            //将角色名和对话内容存入这个list，中间存个逗号一会儿容易分割
        }
        dialogue_count = dialogues_list.Count;//获取到底有多少条对话
        dialogues_handle(0);//载入第一条对话的场景
        Image.SetActive(true);
    }
    public void asd()
    {
        Image.SetActive(true);
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))//如果点击了鼠标左键
        {
            dialogue_index++;//对话跳到一下个
            if (dialogue_index < dialogue_count)//如果对话还没有完
            {
                dialogues_handle(dialogue_index);//那就载入下一条对话
            }
            else
            { //对话完了
                //进入下一游戏场景之类的
                roleA.SetActive(false);
                roleB.SetActive(false);
                roleC.SetActive(false);
                //roleName.SetActive(false);  //關閉角色名
                //detail.SetActive(false);  //關閉對話內容
                Image.SetActive(false);     //關閉對話視窗(會連同內容上的東西一起關閉)
                UIManager2.Instance.ClosePanel("SHIH/Panel3");
                GameObject.Find("System").GetComponent<TurnControl>().GameStartConfirm();
            }
        }
    }

    /*处理每一条对话的函数，就是将dialogues_list每一条对话弄到场景*/
    private void dialogues_handle(int dialogue_index)
    {
        //切割数组
        string[] role_detail_array = dialogues_list[dialogue_index].Split(',');//list中每一个对话格式就是“角色名,对话”
        role = role_detail_array[0];
        role_detail = role_detail_array[1];

        switch (role)//根据角色名
        {   //显示当前说话的角色
            case "A":
                roleA.SetActive(true);
                roleB.SetActive(true);
                //roleC.SetActive(false);
                GameObject.Find("玉米A").GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 1);
                GameObject.Find("飛機頭").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                GameObject.Find("馬鈴薯").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

                roleName.GetComponent<Text>().text = "飛機頭與馬鈴薯:";
                break;
            case "B":
                //roleB.SetActive(false);
                //roleA.SetActive(false);
                roleC.SetActive(true);
                GameObject.Find("飛機頭").GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 1);
                GameObject.Find("馬鈴薯").GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 1);
                GameObject.Find("玉米A").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                roleName.GetComponent<Text>().text = "玉米惡霸:";
                break;
        }
        detail.GetComponent<Text>().text = role_detail;//并加载当前的对话
    }

}