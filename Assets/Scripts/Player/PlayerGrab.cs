using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Настройки захвата")]
    public float grabDistance = 4f;
    public float holdDistance = 2.5f;
    public float grabForce = 15f;
    public LayerMask boxLayer;

    [Header("Ссылки")]
    public Camera playerCamera;

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        // Захват или сброс на E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null) TryGrab();
            else Release();
        }

        // Бросок на ЛКМ
        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            Throw();
        }
    }

    void FixedUpdate()
    {
        if (heldObject != null && heldRb != null)
        {
            Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * holdDistance;
            Vector3 direction = targetPosition - heldObject.transform.position;

            heldRb.linearVelocity = direction * grabForce;

            heldRb.angularVelocity = Vector3.zero;
        }
    }

    void TryGrab()
    {
        RaycastHit hit;
        // Пускаем луч из центра камеры
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grabDistance, boxLayer))
        {
            if (hit.rigidbody != null)
            {
                heldObject = hit.rigidbody.gameObject;
                heldRb = hit.rigidbody;

                // временно отключаем коллизию с игроком
                Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), GetComponent<Collider>(), true);

                // Включаем интерполяцию для плавности
                heldRb.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }
    }

    public void Release()
    {
        if (heldObject != null)
        {
            // Возвращаем коллизию обратно
            Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), GetComponent<Collider>(), false);

            heldRb.interpolation = RigidbodyInterpolation.None;
            heldObject = null;
            heldRb = null;
        }
    }

    void Throw()
    {
        if (heldObject != null)
        {
            // Запоминаем объект и его Rigidbody до того, как отпустим
            GameObject objToThrow = heldObject;
            Rigidbody rbToThrow = heldRb;

            // Отпускаем
            Release();

            // Применяем силу к сохраненному Rigidbody
            Vector3 throwDirection = playerCamera.transform.forward;

            // Сила броска
            float throwForce = 50f;

            BoxDamage boxDamage = objToThrow.GetComponent<BoxDamage>();
            if (boxDamage != null) boxDamage.PlayThrowSound();

            rbToThrow.AddForce(throwDirection * throwForce, ForceMode.Impulse);

            if (CameraShake.Instance != null) CameraShake.Instance.Shake(0.2f, 0.15f);
        }
    }
}