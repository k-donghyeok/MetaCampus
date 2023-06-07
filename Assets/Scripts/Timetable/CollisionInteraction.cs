using UnityEngine;

public class CollisionInteraction : MonoBehaviour
{
    public GameObject uiObject;
    public GameObject uiObject2;
    private bool isColliding = false;
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("시간표 확인");
            isColliding = true;
            uiObject.SetActive(true);
            uiObject2.SetActive(true);
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = false;
            uiObject.SetActive(false);
            uiObject2.SetActive(false);
        }

        if (!isColliding)
        {
            uiObject.SetActive(false);
            uiObject2.SetActive(false);
        }
    }
}
