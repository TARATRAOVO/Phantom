using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidOverlap : MonoBehaviour
{
    public GameObject toAvoid;
    public float avoidDistance = 0.5f;
    private CharacterController characterController;
    // Start is called before the first frame update
    public void Start()
    {
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    public void Update()
    {
        float distance = (toAvoid.transform.position - transform.position).magnitude;
        // 如果距离过近
        if (distance < avoidDistance)
        {
            // 计算分离方向
            Vector3 pushDirection = (transform.position - toAvoid.transform.position).normalized;
            // 移动角色
            characterController.Move(pushDirection * 1 * Time.deltaTime);
        }

    }
}
