using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 20f;
    public GameObject uiObject;
    private bool isColliding = false;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("시간표 확인");
            isColliding = true;
            uiObject.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isColliding = false;
            uiObject.SetActive(false);
        }

        if (!isColliding)
        {
            uiObject.SetActive(false);
        }
    }
}
