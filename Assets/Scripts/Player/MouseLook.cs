using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    public Transform cameraPivot;

    public float mouseSensitivity = 200f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private float xRotation = 0f;

    void Start()
    {
        // Прячем и блокируем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // На паузе остановка камеры
        if (Time.timeScale == 0f) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Наклон камеры вверх-вниз
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Поворот тела влево-вправо
        playerBody.Rotate(Vector3.up * mouseX);
    }
}