using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;     // 플레이어 이동 속도
    public float mouseSensitivity = 2f;   // 마우스 감도

    private float mouseX;     // 마우스 X축 회전 값
    private float mouseY;     // 마우스 Y축 회전 값

    void Update()
    {
        // WASD 입력 처리
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(moveX, 0f, moveZ);

        // 마우스 입력 처리
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);   // 마우스 Y축 회전 값 제한

        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0f);   // 플레이어 회전 값 적용
    }
}
