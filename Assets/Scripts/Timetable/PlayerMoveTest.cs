using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도 조절
    public float mouseSensitivity = 2f; // 마우스 감도 조절

    private float verticalRotation = 0f;
    private float upDownRange = 90f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 커서를 화면 가운데에 고정시킴
        Cursor.lockState = CursorLockMode.Locked;

        // 중력 비활성화
        rb.useGravity = false;
    }

    private void Update()
    {
        // 플레이어 이동 처리
        float forwardSpeed = Input.GetAxis("Vertical") * moveSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * moveSpeed;

        Vector3 movement = new Vector3(sideSpeed, 0, forwardSpeed);
        movement = transform.rotation * movement;

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // 플레이어 회전 처리
        float rotX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotX, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // 커서 잠금 및 해제
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Cursor.lockState == CursorLockMode.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
