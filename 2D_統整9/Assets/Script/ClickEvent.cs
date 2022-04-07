using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// 全局Click检测
/// </summary>
public class ClickEvent : MonoBehaviour
{

    private static RaycastHit ObjHit;
    private static Ray CustomRay;
    private static bool TF;
    public  bool check_UI_click()
    {
        
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("当前点击在UI上" + ": |" + EventSystem.current.currentSelectedGameObject + "|");
                TF = true;
            }
            else
            {
                //从摄像机发出一条射线,到点击的坐标
                CustomRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                //显示一条射线，只有在scene视图中才能看到
                Debug.DrawLine(CustomRay.origin, ObjHit.point, Color.red, 2);
                if (Physics.Raycast(CustomRay, out ObjHit, 100))
                {
                    if (ObjHit.collider.gameObject != null)
                    {
                        Debug.Log("Click Object:" + ObjHit.collider.gameObject);
                    }
                }
                else
                {
                    Debug.Log("Click Null");
                }
                TF = false;
            }
        
        return TF;
    }
    /*
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
#if UNITY_ANDROID || UNITY_IPHONE
            //移动端判断如下
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
            //PC端判断如下
            if (EventSystem.current.IsPointerOverGameObject())
#endif
            {
                Debug.Log("当前点击在UI上" + ": |"+EventSystem.current.currentSelectedGameObject+"|");
            }
            else
            {
                //从摄像机发出一条射线,到点击的坐标
                CustomRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                //显示一条射线，只有在scene视图中才能看到
                Debug.DrawLine(CustomRay.origin, ObjHit.point, Color.red, 2);
                if (Physics.Raycast(CustomRay, out ObjHit, 100))
                {
                    if (ObjHit.collider.gameObject != null)
                    {
                        Debug.Log("Click Object:" + ObjHit.collider.gameObject);
                    }
                }
                else
                {
                    Debug.Log("Click Null");
                }
            }
        }
    }
    */

}