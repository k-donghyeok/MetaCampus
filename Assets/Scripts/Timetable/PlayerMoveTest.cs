using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ� ����
    public float mouseSensitivity = 2f; // ���콺 ���� ����

    private float verticalRotation = 0f;
    private float upDownRange = 90f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ŀ���� ȭ�� ����� ������Ŵ
        Cursor.lockState = CursorLockMode.Locked;

        // �߷� ��Ȱ��ȭ
        rb.useGravity = false;
    }

    private void Update()
    {
        // �÷��̾� �̵� ó��
        float forwardSpeed = Input.GetAxis("Vertical") * moveSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * moveSpeed;

        Vector3 movement = new Vector3(sideSpeed, 0, forwardSpeed);
        movement = transform.rotation * movement;

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // �÷��̾� ȸ�� ó��
        float rotX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotX, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Ŀ�� ��� �� ����
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
