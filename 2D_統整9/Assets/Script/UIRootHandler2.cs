using UnityEngine;

public class UIRootHandler2 : MonoBehaviour
{
    void Awake()
    {
        UIManager2.Instance.m_CanvasRoot = gameObject;
    }
}