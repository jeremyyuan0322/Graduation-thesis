using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Nameinput : MonoBehaviour
{
    public string playername;
    public string entertext;

    
 
    void Start()
    {
     

    }

  
    void Update()
    {
        
    }
    public void Name_Enter()
    {
        playername = entertext;
        print((string)playername);
    }
    
    public void Gamestart()
    {
        SceneManager.LoadScene("game");
    }
    public void Name_print()
    {
        
        print(playername);
    }
}
