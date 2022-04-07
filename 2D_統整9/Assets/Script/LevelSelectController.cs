using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    public void EnterLevels(int LevelNum)//進入第幾關
    {
        SceneManager.LoadScene("Scene"+ LevelNum);
        if (LevelNum == 1)
        {
            Scenemapdata.obstacle = -4;
        }
        if (LevelNum == 2)
        {
            Scenemapdata.obstacle = -13;
            Scenemapdata.ground = -12;
        }
        if (LevelNum == 3)
        {
            Scenemapdata.obstacle = -12;
            Scenemapdata.ground = -10;
        }
        if (LevelNum == 4)
        {
            Scenemapdata.obstacle = -17;
            Scenemapdata.ground = -8;
        }
        print(Scenemapdata.obstacle);
        //GameObject.Find("碰撞層").GetComponent<TileTest>().ground_tiletest(-12);
    }
}
