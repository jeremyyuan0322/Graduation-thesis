using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScenceController : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void GameStart()//開始第一關遊戲
    {
        Scenemapdata.obstacle = -4;
        SceneManager.LoadScene("Scene1");
    }
}