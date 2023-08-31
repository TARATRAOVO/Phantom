using System.Collections;
using UnityEngine;
using Cinemachine;
using MxM;

public class FrameFreeze : MonoBehaviour
{
    public float freezeTime = 0.2f;  // Freeze time duration
    public float freezeScale = 0.2f; // Freeze time scale

    public void FreezeFrame()
    {
        print("FreezeFrame");
        StartCoroutine(FreezeFrameIE());
    }
     IEnumerator FreezeFrameIE()
    {
        Time.timeScale = freezeScale; // Pause the game
        yield return new WaitForSecondsRealtime(freezeTime); // Wait for freezeTime seconds in real time
        Time.timeScale = 1; // Resume the game
    }

}