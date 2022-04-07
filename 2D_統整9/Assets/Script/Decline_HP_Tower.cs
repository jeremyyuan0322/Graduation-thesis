using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decline_HP_Tower : MonoBehaviour
{
    public int HP = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //print("Trigger_monster:" + other.gameObject.name); //Collider2D Coll
        HP -= 20; //生命值-10
        //print("youdie");
        if (HP <= 0)
        {
            //Destroy(gameObject); //摧毀物件

            GameObject.Find("TOWER").GetComponent<TowerbreakController>().Break();
            //print("monster die");
        }
    }
   
}
