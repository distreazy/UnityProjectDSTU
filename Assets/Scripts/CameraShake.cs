using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private float shakeTimer = 0f;
    private float shakeIntensity = 0f;
    private Vector3 originalPos;

    void Awake() { Instance = this; }

    void Start() { originalPos = transform.localPosition; }

    void Update()
    {
        if (shakeTimer > 0f)
        {
            // Случайное смещение камеры
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.localPosition = originalPos + randomOffset;

            shakeTimer -= Time.unscaledDeltaTime; // unscaled, чтобы трясло даже на паузе
            if (shakeTimer <= 0f) transform.localPosition = originalPos;
        }
    }

    public void Shake(float intensity, float duration)
    {
        shakeIntensity = intensity;
        shakeTimer = duration;
    }
}