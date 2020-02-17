using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 0.1f;
    public int max_hp = 0;
    public int hp = 0;
    public Image hp_bar_up;
    void Start()
    {
        max_hp = 10;
        hp = max_hp;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            this.gameObject.transform.position += new Vector3(speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.gameObject.transform.position -= new Vector3(speed, 0, 0);
        }
        hp_bar_up.transform.localScale = new Vector3((float)hp / (float)max_hp, hp_bar_up.transform.localScale.y, hp_bar_up.transform.localScale.z);

    }
    
}
