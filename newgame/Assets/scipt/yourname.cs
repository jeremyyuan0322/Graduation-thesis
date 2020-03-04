using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class yourname : MonoBehaviour
{
    Nameinput N;

    //public Text playername;
    public GameObject yn;

 


    void Start()
    {
       //N = yn.GetComponent<Nameinput>();

    }


    void Update()
    {
        
    }
    public void Final()
    {
        GameObject tagObject;
        string x;
        tagObject = GameObject.FindWithTag("tag");

        print("123");
        
        

        //N.Name_print();
    }
    void Awake()
    {
        //abc
    }
}
