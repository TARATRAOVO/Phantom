using UnityEngine;

public class HitAudio : MonoBehaviour
{
    private AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // 获取AudioSource组件
        if (audioSource == null)                   // 如果物体没有AudioSource组件
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // 添加AudioSource组件
        }


    }

    public void PlayAudio()
    {
        print("PlayAudio");
        audioSource.Play(); // 播放音频
    }
}