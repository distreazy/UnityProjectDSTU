using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [Header("Шаги")]
    public AudioClip[] stepClips;
    public float stepInterval = 0.5f;
    private float stepTimer;

    [Header("Урон")]
    public AudioClip hurtClip;

    private AudioSource audioSource;
    private CharacterController controller;
    private float basePitch = 1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        basePitch = audioSource.pitch;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        bool isMoving = new Vector2(inputX, inputZ).sqrMagnitude > 0.01f;

        // Если мы на земле и жмем WASD
        if (controller.isGrounded && isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayStepSound();

                // проверяем зажат ли Shift
                bool isRunning = Input.GetKey(KeyCode.LeftShift);

                // Если бежим
                stepTimer = isRunning ? 0.3f : 0.5f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayStepSound()
    {
        if (stepClips.Length == 0) return;

        AudioClip clip = stepClips[Random.Range(0, stepClips.Length)];

        // Рандомим питч
        audioSource.pitch = basePitch + Random.Range(-0.1f, 0.1f);
        audioSource.PlayOneShot(clip, 0.2f);
    }

    public void PlayHurtSound()
    {
        if (hurtClip != null)
        {
            audioSource.pitch = basePitch + Random.Range(-0.1f, 0.1f);
            audioSource.PlayOneShot(hurtClip, 0.8f);
        }
    }
}