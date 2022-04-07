using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.UI;
public class TileController2 : MonoBehaviour
{
    private Tilemap tilemap;
    public Tilemap tilemap2;
    //private List<Tilemap> tilemap2 = new List<Tilemap>();

    public Tile normalTile;
    public Tile mouseOnTile;
    public GameObject player;
    public Vector3 targetPoint;

    private Ray ray;
    public static Vector3Int posMouseOnGrid;
    private Vector3Int savePosMouseOnGrid;

    private TurnControl turnScript;
    private List<Vector3Int> save_road = new List<Vector3Int>();
    private int count = 0;
    //public Button Attack_button;


    void strat()
    {
        savePosMouseOnGrid = Vector3Int.zero;
    }
    private void OnMouseOver()
    {
        if (turnScript.isWaitForPlayerAttack)
        {
            if (savePosMouseOnGrid != posMouseOnGrid)
            {
                tilemap2.SetColor(savePosMouseOnGrid, Color.white);
                if (save_road.Contains(savePosMouseOnGrid) || save_road.Contains(posMouseOnGrid))
                {
                    //print("++");
                    tilemap2.SetColor(savePosMouseOnGrid, Color.gray);
                }
                if (tilemap2.HasTile(posMouseOnGrid))
                {
                    tilemap2.SetTileFlags(posMouseOnGrid, TileFlags.None);
                    tilemap2.SetColor(posMouseOnGrid, Color.gray);
                }
                savePosMouseOnGrid = posMouseOnGrid;
            }
        }

    }
    private void OnMouseUp()
    {

        if (tilemap2.HasTile(posMouseOnGrid))
        {
            if (turnScript.isWaitForPlayerAttack)
            {

                GameObject.Find("System").GetComponent<MapBehaviour>().AStarSearchPath(posMouseOnGrid);
                GameObject.Find("Player").GetComponent<AttackController>().Compute_Energy(); //若不是固定消耗的技能，點選時會判定消耗量
                
                foreach (var i in save_road)
                {
                    tilemap2.SetTileFlags(i, TileFlags.None);
                    tilemap2.SetColor(i, Color.white);
                }

                save_road.Clear();

                foreach (var i in MapBehaviour.road)
                {

                    //print("ROAD" + i);
                    save_road.Add(i);
                    tilemap2.SetTileFlags(i, TileFlags.None);
                    tilemap2.SetColor(i, Color.gray);
                }

            }

        }

    }
    void Awake()
    {
        tilemap = gameObject.GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.Log(gameObject.name + " does not have a Tilemap component");
        }
    }

    void Start()
    {

        savePosMouseOnGrid = new Vector3Int(0, 0, 0);
        targetPoint = player.transform.position;

        turnScript = GameObject.FindGameObjectWithTag("System").GetComponent<TurnControl>();
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        posMouseOnGrid = tilemap.WorldToCell(new Vector3(ray.origin.x, ray.origin.y, 0));
        //print("這啥" + posMouseOnGrid);
        //OnMouseOver();


        /*if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            targetPoint = tilemap.WorldToCell(new Vector3(ray.origin.x, ray.origin.y, 0));
            print(targetPoint);
        }

        player.transform.position = Vector3.MoveTowards(player.transform.position, targetPoint, 8 * Time.deltaTime);
        */
    }
    public void Clear()
    {

    }
}
