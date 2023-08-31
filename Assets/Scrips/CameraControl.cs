using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // 通过鼠标控制摄像机旋转
    public float Pitch;
    public float Yaw;
    [HideInInspector]
    public GameObject player;
    // 玩家高度
    public float playerHeight = 1.0f;
    // 鼠标灵敏度
    public float mouseSensitivity = 0.1f;
    // 仰俯角极限
    public float pitchLimitUp = 90.0f;
    public float pitchLimitDown = -90.0f;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Camera Control
        // 通过鼠标控制摄像机旋转
        UpdateRotation();
        Vector3 targetPos = player.transform.position;
        targetPos.y += playerHeight;
        // 物体跟随player移动，但是延迟
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f); // 执行Lerp操作
    }

    private void UpdateRotation()
    {
        // Camera Control
        // 通过鼠标控制摄像机旋转
        Yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        Pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        Pitch = Mathf.Clamp(Pitch, pitchLimitDown, pitchLimitUp);
        transform.eulerAngles = new Vector3(Pitch, Yaw, 0.0f);
    }

}
