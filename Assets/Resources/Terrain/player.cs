using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{
    //�����;��ϴ� Ŭ������
    private Camera playerCamera = null;
    private CharacterController characterController = null;

    //�÷��̾� �����ӿ� �ʿ��� ����
    [SerializeField, Range(1f, 100f)] private float moveSpeed = 1f;

    #region ī�޶�ȸ���� �ʿ��� ����
    [SerializeField, Range(50f, 200f)] private float rotSpeed = 50f;
    float horizontalPowerMouse = 0f;
    float verticalPowerMouse = 0f;
    #endregion

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
    }

    
    private void Update()
    {
        CalculateVector();
        ChangeCameraAngleWithMouse();
        
      

    }

    private void ChangeCameraAngleWithMouse()
    {
        float horizontalVal = Input.GetAxis("Mouse X"); //yaw ,y
        float verticalVal = Input.GetAxis("Mouse Y");//pitch,x


        horizontalPowerMouse += horizontalVal * rotSpeed * Time.deltaTime;

        //if(horizontalPower>270f)// ����Ƽ���� ī�޶��� ȸ������ ���Ŀ�����ʵ��� �۾��س����Ű���
        //{
        //    horizontalPower = -90f;
        //}
        //if(horizontalPower<-450f)
        //{
        //    horizontalPower = -90f;
        //}    


        verticalPowerMouse += verticalVal * rotSpeed * Time.deltaTime;
        verticalPowerMouse = Mathf.Clamp(verticalPowerMouse, -20f, 40f);

        playerCamera.transform.rotation = Quaternion.Euler(-verticalPowerMouse, horizontalPowerMouse, 0f);
        transform.rotation = Quaternion.Euler(-verticalPowerMouse, horizontalPowerMouse, 0f);
    }

    private void CalculateVector()
    {
        Vector3 cameraDir = playerCamera.transform.forward; //����ī�޶� ����
        cameraDir.y = 0f;
        cameraDir.Normalize();

        float horizontalval = Input.GetAxis("Horizontal");
        float verticalval = Input.GetAxis("Vertical");

        float horizontalmovePower = horizontalval * moveSpeed * Time.deltaTime; //����ũ��
        float verticalmovePower = verticalval * moveSpeed * Time.deltaTime;

        Vector3 camerapos = cameraDir * verticalmovePower + Quaternion.Euler(0f, 90f, 0f) * cameraDir * horizontalmovePower + Physics.gravity; //���ʹϾ� �� ������ ������ ���ʹϾ��� ���͵ڿ� ������ ������ �ȵ� ����
        characterController.Move(camerapos);

        //transform.position += new Vector3(cameraDir.x * verticalmovePower + cameraDir.z * horizontalmovePower, 0f, cameraDir.z * verticalmovePower + -cameraDir.x * horizontalmovePower); //�ٸ���� ������ ���� ��� Ǯ� �����̷��Ե�
    }
}


