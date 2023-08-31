using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 隐藏鼠标并限制鼠标在屏幕内
public class HideCursor : MonoBehaviour
{
    public bool hideCursor = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; // 鼠标锁定模式
            Cursor.visible = true; // 鼠标可见
        }

        
    }
}
