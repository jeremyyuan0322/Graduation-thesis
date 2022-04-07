using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.CloseAllPanel();
        UIManager2.Instance.CloseAllPanel();
        if (SceneController.SceneName == "Scene1")//如果是Sence1要重置這些
        {
            MonsterController.mapSize = new Vector2Int(0, 0);
            MonsterController.startPos = new Vector3Int(0, 0, 0);
            MonsterController.CurrentPos = new Vector3Int(0, 0, 0);
            MonsterController.endPos = new Vector3Int(0, 0, 0);
            MonsterController.pathSave.Clear();
            MonsterController.road.Clear();
            MonsterController2.mapSize = new Vector2Int(0, 0);
            MonsterController2.startPos = new Vector3Int(0, 0, 0);
            MonsterController.CurrentPos = new Vector3Int(0, 0, 0);
            MonsterController2.endPos = new Vector3Int(0, 0, 0);
            MonsterController2.pathSave.Clear();
            MonsterController2.road.Clear();
        }
        if (SceneController.SceneName == "Scene2")//如果是Sence2要重置這些
        {
            MonsterController.mapSize = new Vector2Int(0, 0);
            MonsterController.startPos = new Vector3Int(0, 0, 0);
            MonsterController.CurrentPos = new Vector3Int(0, 0, 0);
            MonsterController.endPos = new Vector3Int(0, 0, 0);
            MonsterController.pathSave.Clear();
            MonsterController.road.Clear();
            MonsterController2.mapSize = new Vector2Int(0, 0);
            MonsterController2.startPos = new Vector3Int(0, 0, 0);
            MonsterController.CurrentPos = new Vector3Int(0, 0, 0);
            MonsterController2.endPos = new Vector3Int(0, 0, 0);
            MonsterController2.pathSave.Clear();
            MonsterController2.road.Clear();
            BossController.mapSize = new Vector2Int(0, 0);
            BossController.startPos = new Vector3Int(0, 0, 0);
            BossController.CurrentPos = new Vector3Int(0, 0, 0);
            BossController.endPos = new Vector3Int(0, 0, 0);
            BossController.pathSave.Clear();
            BossController.road.Clear();
        }
        if (SceneController.SceneName == "Scene3")//如果是Sence3要重置這些
        {

        }
        if (SceneController.SceneName == "Scene4")//如果是Sence4要重置這些
        {

        }
        TileTest.obstacle.Clear();
        BreathFirst.isRunning = true;
        BreathFirst.ExplorArounds = new Vector3Int(0, 0, 0);
        BreathFirst.HadWaySave.Clear();
        MapBehaviour.hasStartPosSet = false;
        MapBehaviour.hasEndPosSet=false;
        MapBehaviour.mapSize = new Vector2Int(0, 0);
        MapBehaviour.startPos = new Vector3Int(0, 0, 0);
        MapBehaviour.endPos = new Vector3Int(0, 0, 0);
        MapBehaviour.pathSave.Clear();
        MapBehaviour.road.Clear();
        TileController.posMouseOnGrid = new Vector3Int(0, 0, 0);
        TileController2.posMouseOnGrid = new Vector3Int(0, 0, 0);
        AttackController.combo = "";
        AttackController.count = 0;
        Scenemapdata.obstacle = 0;
        Scenemapdata.ground = 0;
        PlayerController.Energy = 60;
        PlayerController.EnergyCosume = 0;

        if (SceneController.SceneInstruction == "RestartGame") //如果場景標示是重新開始遊戲
        {
            if (SceneController.SceneName == "Scene1") //重新載入scene1要重新補正地圖
            {
                Scenemapdata.obstacle = -4;
            }
            if (SceneController.SceneName == "Scene2")//重新載入scene2要重新補正地圖
            {
                Scenemapdata.obstacle = -13;
                Scenemapdata.ground = -12;
            }
            if (SceneController.SceneName == "Scene3")//重新載入scene3要重新補正地圖
            {
                Scenemapdata.obstacle = -12;
                Scenemapdata.ground = -10;
            }
            if (SceneController.SceneName == "Scene4")//重新載入scene4要重新補正地圖
            {
                Scenemapdata.obstacle = -17;
                Scenemapdata.ground = -8;
            }

            SceneManager.LoadScene(SceneController.SceneName);
            SceneController.SceneInstruction = "";
            SceneController.SceneName = "";
        }

        if (SceneController.SceneInstruction == "BackToMainMenu") //如果場景標示是回到主選單
        {
            SceneManager.LoadScene("MainMenu");
            SceneController.SceneInstruction = "";
            SceneController.SceneName = "";
        }

        if (SceneController.SceneInstruction == "NextLevel") //如果場景標示是前往下一關
        {
            if (TurnControl.NextLevelNum == 1)//前往第一關，補正第一關
            {
                Scenemapdata.obstacle = -4;
            }
            if (TurnControl.NextLevelNum == 2)//前往第二關，補正第二關
            {
                Scenemapdata.obstacle = -13;
                Scenemapdata.ground = -12;
            }
            if (TurnControl.NextLevelNum == 3)//前往第四關，補正第四關
            {
                Scenemapdata.obstacle = -12;
                Scenemapdata.ground = -10;
            }
            if (TurnControl.NextLevelNum == 4)//前往第四關，補正第四關
            {
                Scenemapdata.obstacle = -17;
                Scenemapdata.ground = -8;
            }


            SceneManager.LoadScene("Scene" + TurnControl.NextLevelNum);
            SceneController.SceneInstruction = "";
            SceneController.SceneName = "";
            TurnControl.NextLevelNum = 0;

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
