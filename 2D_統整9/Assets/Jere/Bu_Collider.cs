using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bu_Collider : MonoBehaviour
{
    private PolygonCollider2D Bu;
    Vector2 scale, offset;
    GameObject BA;
    private List<Vector3Int> hadSearch = new List<Vector3Int>();//已经查找过的坐标

    // Start is called before the first frame update
    void Start()
    {
        
        scale = new Vector2(1,1);
        offset = new Vector2(0, 0.34f);

        Bu.CreatePrimitive(4, scale, offset);
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Collider_Controller(int point, float X, float Y)
    {
        GameObject.Find("bullet_A").GetComponent<PolygonCollider2D>().points[point].Set(X, Y);
        

    }
}
