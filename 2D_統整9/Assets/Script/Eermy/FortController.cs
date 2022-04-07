using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FortController : MonoBehaviour
{
    public GameObject col;
    public GameObject player;
    public Tilemap tilemap;
    public Grid grid_;
    public Animator animator;

    private Vector3Int PlayerPosition;
    public bool Attacklock = true;
    private TurnControl turnScript;

    // Start is called before the first frame update
    void Start()
    {
        turnScript = GameObject.FindGameObjectWithTag("System").GetComponent<TurnControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnControl.currentState == TurnControl.GameState.Game)
        {
            if (turnScript.isEnemyAction)
            {
                animator.SetBool("Attack", false);
                if (!Attacklock)
                {
                    animator.SetBool("Attack",true);
                    PlayerPosition = tilemap.WorldToCell(player.transform.position);
                    FortAttack(PlayerPosition);
                }
            }
        }
    }

    private bool FortAttack(Vector3Int Pos)
    {
        Vector3 obj = grid_.CellToWorld(Pos);
        GameObject Obj = Instantiate(col, obj, new Quaternion(0, 0, 0, 0));
        Obj.name += turnScript.ColNum_Monster3;
        turnScript.ColNum_Monster3++;
        Attacklock = true;
        return Attacklock;
    }
}
