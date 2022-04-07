using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class TowerbreakController : MonoBehaviour
{
    public Animator tower;
    public Tilemap tilemap;
    public Tilemap tilemap2;

    public Tile normalTile;//tile(陸地)
    public GameObject g;
    public static Vector3Int Pos;//塔座標
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Break()
    {
        tower.SetBool("break",true);
        print("塔的點"+tilemap.WorldToCell(g.transform.position));
        Pos = tilemap.WorldToCell(g.transform.position);
        Pos.x = Pos.x - 3;
        Pos.y = Pos.y - 3;
        tilemap2.SetTile(Pos, normalTile);
        Pos.x = Pos.x + 1;
        tilemap2.SetTile(Pos, normalTile);
        Pos.x = Pos.x + 1;
        tilemap2.SetTile(Pos, normalTile);
        Pos.y = Pos.y + 1;
        tilemap2.SetTile(Pos, normalTile);
        Pos.y = Pos.y + 1;
        tilemap2.SetTile(Pos, normalTile);
        Pos.x = Pos.x - 1;
        tilemap2.SetTile(Pos, normalTile);
        Pos.x = Pos.x - 1;
        tilemap2.SetTile(Pos, normalTile);
        Pos.y = Pos.y - 1;
        tilemap2.SetTile(Pos, normalTile);

        Pos.x = Pos.x + 1;
        tilemap2.SetTile(Pos, normalTile);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
