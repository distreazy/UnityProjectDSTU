using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;
    public AudioClip clickClip;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayClick()
    {
        if (audioSource != null && clickClip != null)
        {
            audioSource.pitch = 1f + Random.Range(-0.05f, 0.05f);
            audioSource.PlayOneShot(clickClip, 0.7f);
        }
    }
}