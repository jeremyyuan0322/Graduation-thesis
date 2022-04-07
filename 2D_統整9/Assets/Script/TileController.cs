using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TileController : MonoBehaviour
{
    private Tilemap tilemap;

    public Tile normalTile;
    public Tile mouseOnTile;
    public GameObject player;
    public Vector3 targetPoint;

    private Ray ray;
    public static Vector3Int posMouseOnGrid;
    private Vector3Int savePosMouseOnGrid;

    private TurnControl turnScript ;
    private void OnMouseOver()
    {

        if (savePosMouseOnGrid != posMouseOnGrid)
        {
            if (tilemap.HasTile(savePosMouseOnGrid))
            {
                tilemap.SetTileFlags(savePosMouseOnGrid, TileFlags.None);
                tilemap.SetColor(savePosMouseOnGrid, Color.white);
            }
            savePosMouseOnGrid = posMouseOnGrid;


        }
        if(turnScript.isWaitForPlayerMove)
        {
            if (tilemap.HasTile(posMouseOnGrid))
            {
                tilemap.SetTileFlags(savePosMouseOnGrid, TileFlags.None);
                tilemap.SetColor(posMouseOnGrid, Color.gray);
               
            }
        }
        /*if (turnScript.isWaitForPlayerAttack)
        {
            if (BreathFirst.HadWaySave.Contains(posMouseOnGrid))
            {
                tilemap.SetTileFlags(savePosMouseOnGrid, TileFlags.None);
                tilemap.SetColor(posMouseOnGrid, Color.gray);
            }
        }*/
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
        OnMouseOver();

        /*if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            targetPoint = tilemap.WorldToCell(new Vector3(ray.origin.x, ray.origin.y, 0));
            print(targetPoint);
        }

        player.transform.position = Vector3.MoveTowards(player.transform.position, targetPoint, 8 * Time.deltaTime);
        */
    }
}
