using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;     // �÷��̾� �̵� �ӵ�
    public float mouseSensitivity = 2f;   // ���콺 ����

    private float mouseX;     // ���콺 X�� ȸ�� ��
    private float mouseY;     // ���콺 Y�� ȸ�� ��

    void Update()
    {
        // WASD �Է� ó��
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(moveX, 0f, moveZ);

        // ���콺 �Է� ó��
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);   // ���콺 Y�� ȸ�� �� ����

        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0f);   // �÷��̾� ȸ�� �� ����
    }
}
