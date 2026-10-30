using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Настройки захвата")]
    public float grabDistance = 4f;      // Максимальное расстояние до ящика
    public float holdDistance = 2.5f;    // На каком расстоянии держать перед камерой
    public float grabForce = 15f;        // Сила притягивания к точке удержания
    public LayerMask boxLayer;           // Слой, на котором лежат ящики

    [Header("Ссылки")]
    public Camera playerCamera;          // Ссылка на камеру

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        // Захват или сброс на E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null) TryGrab();
            else Release(); // Просто отпускаем (без импульса)
        }

        // Бросок на ЛКМ (только если предмет в руках)
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

            // ВАЖНО: используем linearVelocity для Unity 6
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

                // Чтобы ящик не сбивал с ног игрока (CharacterController), временно отключаем коллизию с игроком
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
            // 1. Запоминаем объект и его Rigidbody ДО того, как отпустим
            GameObject objToThrow = heldObject;
            Rigidbody rbToThrow = heldRb;

            // 2. Отпускаем (возвращаем коллизии и очищаем переменные)
            Release();

            // 3. Применяем силу к сохраненному Rigidbody
            Vector3 throwDirection = playerCamera.transform.forward;

            // Сила броска (настрой в инспекторе под себя, сейчас 15)
            float throwForce = 50f;

            // Чтобы тяжелые ящики кидались хуже, делим силу на массу (опционально)
            // Если хочешь, чтобы все кидались одинаково, убери деление на rbToThrow.mass
            rbToThrow.AddForce(throwDirection * throwForce, ForceMode.Impulse);
        }
    }
}