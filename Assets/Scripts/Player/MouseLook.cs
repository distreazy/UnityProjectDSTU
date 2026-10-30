using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform playerBody;   // Тело игрока (капсула)
    public Transform cameraPivot;  // Пустой объект, внутри которого камера

    [Header("Настройки чувствительности")]
    public float mouseSensitivity = 200f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private float xRotation = 0f;

    void Start()
    {
        // Прячем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraPivot != null)
            xRotation = cameraPivot.localEulerAngles.x;
    }

    void Update()
    {
        // Если игра на паузе (или меню) - не крутим камеру
        if (Time.timeScale == 0f) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Вычитаем движение мыши по Y
        xRotation -= mouseY;

        // ограничиваем угол
        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch);

        // Применяем наклон ТОЛЬКО к CameraPivot
        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Вращаем ТЕЛО игрока по оси Y
        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX);
    }
}