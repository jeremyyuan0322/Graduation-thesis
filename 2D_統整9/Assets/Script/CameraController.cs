using UnityEngine;
/// <summary>
/// 摄像机视角
/// 鼠标滚轮实现缩放，按住鼠标滚落拖动平移
/// </summary>
public class CameraController : MonoBehaviour
{
    //MoveCamera
    private bool isMouseDown = false;
    private Vector3 lastMousePosition = Vector3.zero;

    //玩家座標
    public Transform PlayerTrans;

    // 距离限制：最大值越大 看到的物体越小，反之同理
    public Vector2 maxPosition;
    public Vector2 minPosition;

    //ZoomCamera
    // 距离限制：最大值越大 看到的物体越小，反之同理
    public float maxDistance = 10f;
    public float minDistance = 4f;

    //缩放速度:数值越大缩放速度越快
    int zoomRate = 40;
    //缩放阻尼：数值越大停的越快
    float zoomDampening = 5.0f;

    //当前距离
    private float currentDistance;
    //目标距离
    private float desiredDistance;


    void Start()
    {
        Camera.main.orthographicSize = 7;
        //currentDistance = 5;
    }
    void LateUpdate()
    {
        Move();
        Zoom();
    }

    //摄像机平移
    void Move()
    {
        Vector3 CamPos = transform.position;
        CamPos.x = Mathf.Clamp(CamPos.x, minPosition.x, maxPosition.x);
        CamPos.y = Mathf.Clamp(CamPos.y, minPosition.y, maxPosition.y);
        if (Input.GetMouseButtonDown(1))
        {
            isMouseDown = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isMouseDown = false;
            lastMousePosition = Vector3.zero;
        }
        if (isMouseDown)
        {
            if (lastMousePosition != Vector3.zero)
            {
                Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                CamPos -= offset;
                transform.position = CamPos;
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        }



    }

    //摄像机视角缩放
    void Zoom()
    {
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
        Camera.main.orthographicSize = currentDistance;
    }

    public void SetCamPosLimit(Vector2 MaxPos, Vector2 MinPos) //設置攝影機範圍限制
    {
        maxPosition = MaxPos;
        minPosition = MinPos;
    }

    public void SetCamToPlayer() //將攝影機移回玩家身上
    {
        transform.position = PlayerTrans.position + new Vector3(0, 0, -10);
    }
}