using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    
    public GameObject Image;//外面這個腳本會多出個選項 要去把要用的腳本塞進去
    Dialogues g ;

    // Start is called before the first frame update
    void Start()
    {
        g = Image.GetComponent<Dialogues>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetMouseButtonDown(0))
        {
            //print("why");
            //print("other?"+other);
            //UIManager.Instance.ShowPanel("SHIH/Panel");           
        }
    }
}
