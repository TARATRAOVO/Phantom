using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioClip audioClips;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayAudio()
    {
        audioSource.clip = audioClips;
        audioSource.Play();
    }
}
