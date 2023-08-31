using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer; // 地面物体的layer
    public bool isGrounded; // 是否接触到地面
    public float groundOffset = 0.1f; // 地面偏移量

    private void Update()
    {
        // 从当前位置向下发射射线
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundOffset, groundLayer);

    }
}
