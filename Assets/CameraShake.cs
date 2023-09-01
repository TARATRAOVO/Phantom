using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineCameraOffset offset;

    public float magnitudeX = 1f;
    public float magnitudeY = 1f;
    public float duration = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake()
    {
        StartCoroutine(TheShake());
    }

    public IEnumerator TheShake()
    {     
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitudeX;
            float y = Random.Range(-1, 1) * magnitudeY;
            offset.m_Offset.x = x;
            offset.m_Offset.y = y;
            elapsed += Time.deltaTime;
            yield return null;
        }
        offset.m_Offset = Vector3.zero;
    }
}
