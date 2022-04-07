using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class Decline_HP : MonoBehaviour
{
    public int HP = 0;//現有血量
    public int Max_hp ;//血量最大值
    public Tilemap tilemap;//需要轉換怪物世界座標成瓦片座標
    public Tilemap tilemap2;//放碰撞層
    public Tilemap tilemapwater;//給第三關放水層
    public int quadrant;//讓人回傳遞幾象限
    Vector3Int temp;    //怪物目前位置
    Vector3Int temp2;   //怪物被移動後位置
    public TileBase obs;
    public GameObject HP_bar;
    
    void Start()
    {
        
        HP = Max_hp;

    }

    
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject); //摧毀物件
            //print("monster die");
        }
        float _percent = ((float)HP / (float)Max_hp);
        if (HP_bar)
        {
            HP_bar.transform.localScale = new Vector3(_percent, HP_bar.transform.localScale.y, HP_bar.transform.localScale.z);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (AttackController.comboformonster == "PH")//推
        {

            quadrant = GameObject.Find("Player").GetComponent<AttackController>().Calculate_relative_position(gameObject.transform.position);

            switch (quadrant)
            {
                case 1:
                    temp = tilemap.WorldToCell(gameObject.transform.position);
                    temp2 = temp;
                    temp2.x = temp2.x + 1;
                    break;
                case 2:
                    temp = tilemap.WorldToCell(gameObject.transform.position);
                    temp2 = temp;
                    temp2.y = temp2.y + 1;
                    break;
                case 3:
                    temp = tilemap.WorldToCell(gameObject.transform.position);
                    temp2 = temp;
                    temp2.x = temp2.x - 1;
                    break;
                case 4:
                    temp = tilemap.WorldToCell(gameObject.transform.position);
                    temp2 = temp;
                    temp2.y = temp2.y - 1;
                    break;
            }
            if (tilemap.HasTile(temp2))
            {
                gameObject.transform.position = tilemap.CellToWorld(temp2);

                tilemap2.SetTile(temp, null);
                tilemap2.SetTile(temp2, obs);
                GameObject.Find("碰撞層").GetComponent<TileTest>().obstacle_tiletest(Scenemapdata.obstacle);
            }
            print(temp + "/" + temp2);
            //print("兩次?");
        }
        else if (AttackController.comboformonster == "VH")//拉
        {

            quadrant = GameObject.Find("Player").GetComponent<AttackController>().Calculate_relative_position(gameObject.transform.position);
            //print(tilemap.WorldToCell(gameObject.transform.position));
            switch (quadrant)
            {
                case 1:
                    temp = tilemap.WorldToCell(gameObject.transform.position);
                    temp2 = temp;
                    temp2.x = temp2.x - 1;
                    break;
                case 2:
                    temp = tilemap.WorldToCell(gameObject.transform.position);
                    temp2 = temp;
                    temp2.y = temp2.y - 1;
                    break;
                case 3:
                    temp = tilemap.WorldToCell(gameObject.transform.position);
                    temp2 = temp;
                    temp2.x = temp2.x + 1;
                    break;
                case 4:
                    temp = tilemap.WorldToCell(gameObject.transform.position);
                    temp2 = temp;
                    temp2.y = temp2.y + 1;
                    break;
            }
            if (!tilemap2.HasTile(temp2))
            {
                gameObject.transform.position = tilemap.CellToWorld(temp2);

                //tilemap2.SetTile(temp, null);
                //tilemap2.SetTile(temp2, obs);
                //GameObject.Find("碰撞層").GetComponent<TileTest>().obstacle_tiletest(Scenemapdata.obstacle);
            }
            print(temp + "/" + temp2);
            if (other.gameObject.tag == "Bullet_A")
            {

                if (AttackController.comboformonster == "VH"&&tilemapwater.HasTile(temp2))
                { HP -= 200;                }
                if (AttackController.comboformonster == "VH" && tilemap2.HasTile(temp))
                {
                    tilemap2.SetTile(temp, null);
                    tilemap2.SetTile(temp2, obs);

                    GameObject.Find("碰撞層").GetComponent<TileTest>().obstacle_tiletest(Scenemapdata.obstacle);
                }
            }
        }
        else
        {
            if (other.gameObject.tag == "Bullet_A")
            {

                if (AttackController.comboformonster == "H")
                { HP -= 1; }
                else if (AttackController.comboformonster == "HHH")
                { HP -= 3; }
                else 
                { HP -= 2; }
            }

        }
        


        //print("youdie");
        
    }
    

}
