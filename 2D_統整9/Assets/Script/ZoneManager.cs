using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class ZoneManager : MonoBehaviour
{
    private static readonly List<Vector3Int> tileOffset = new List<Vector3Int>()
    {
        Vector3Int.down,Vector3Int.right,Vector3Int.up,Vector3Int.left
    };

    private static readonly Dictionary<string, int> tileMoveCostDictionary = new Dictionary<string, int>()
    {
        { "Base_Green",1},{"Base_Brown",2 }
    };
    private List<Vector3Int> movePointRangeList;
    private List<Vector3Int> blockingPointList;
    private List<int> blockingRemainList;

    public Tilemap tilemap;
    public GridLayout gridLayout;

    public GameObject movePointPrefab;
    public Transform movePointObjParent;

    public int movementPoints;

    private Transform currentSelect;
    private Camera mainCamera;
    private RaycastHit2D raycastHit2D;
    public Physics2D Physics2D;

    void Start()
    {
        mainCamera = Camera.main;
        movePointRangeList = new List<Vector3Int>();
        blockingPointList = new List<Vector3Int>();
        blockingRemainList = new List<int>();
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            raycastHit2D = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (raycastHit2D.collider != null)
            {
                switch (raycastHit2D.transform.tag)
                {
                    case "Infantry":
                        currentSelect = raycastHit2D.transform;

                        if (movePointObjParent.childCount == 0)
                        {
                            DisplayMovementRange(gridLayout.WorldToCell(raycastHit2D.point));
                        }
                        else
                        {
                            currentSelect = null;

                            CleanMovementRangeObj();
                        }
                        break;
                    case "TileMap":
                        if (currentSelect != null)
                        {
                            currentSelect.localPosition = gridLayout.CellToLocal(gridLayout.WorldToCell(raycastHit2D.point));

                            currentSelect = null;

                            CleanMovementRangeObj();
                        }
                        break;
                }
            }
        }
    }

    private void DisplayMovementRange(Vector3Int startPos)
    {
        Queue<Vector3Int> currentQueue = new Queue<Vector3Int>();
        Queue<Vector3Int> nextQueue = new Queue<Vector3Int>();

        Vector3Int currentPoint;
        Vector3Int nextPoint;
        int value;

        nextQueue.Enqueue(startPos);

        for (int i = 0; i < movementPoints; i++)
        {
            currentQueue = new Queue<Vector3Int>(nextQueue);
            nextQueue.Clear();

            while (currentQueue.Count > 0)
            {
                currentPoint = currentQueue.Dequeue();

                if (blockingPointList.Contains(currentPoint))
                {
                    int index = blockingPointList.IndexOf(currentPoint);
                    value = GetTileCost(currentPoint);

                    blockingRemainList[index]++;
                    if (blockingRemainList[index] < value)
                    {
                        nextQueue.Enqueue(currentPoint);
                        continue;
                    }
                }

                //4 Direction
                for (int j = 0; j < 4; j++)
                {
                    nextPoint = currentPoint + tileOffset[j];

                    if (IsNextPointInRange(nextPoint))
                    {
                        if (!movePointRangeList.Contains(nextPoint))
                        {
                            value = GetTileCost(nextPoint);

                            movePointRangeList.Add(nextPoint);
                            nextQueue.Enqueue(nextPoint);

                            if (value > 1 && !blockingPointList.Contains(nextPoint))
                            {
                                blockingPointList.Add(nextPoint);
                                blockingRemainList.Add(0);
                            }
                        }
                    }
                }
            }
        }

        CreateMovementRangeObj();
    }

    private int GetTileCost(Vector3Int tilePos)
    {
        int value;
        if (tileMoveCostDictionary.TryGetValue(tilemap.GetTile(tilePos).name, out value))
        {
            return value;
        }
        else
        {
            print("Cannot Find Tile Cost");
            return -1;
        }
    }

    private bool IsNextPointInRange(Vector3Int nextPoint)
    {
        return nextPoint.x >= -2 && nextPoint.x < 2 && nextPoint.y >= 0 && nextPoint.y < 2;
    }

    private void CreateMovementRangeObj()
    {
        foreach (Vector3Int item in movePointRangeList)
        {
            GameObject obj = Instantiate(movePointPrefab, movePointObjParent);
            obj.transform.localPosition = gridLayout.CellToLocal(item);
        }

        movePointRangeList.Clear();
    }

    private void CleanMovementRangeObj()
    {
        if (movePointObjParent.childCount == 0)
            return;

        for (int i = 0; i < movePointObjParent.childCount; i++)
        {
            Destroy(movePointObjParent.GetChild(i).gameObject);
        }

        blockingPointList.Clear();
        blockingRemainList.Clear();
    }

}
