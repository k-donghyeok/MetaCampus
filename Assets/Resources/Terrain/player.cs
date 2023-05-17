using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{
    //가져와야하는 클래스들
    private Camera playerCamera = null;
    private CharacterController characterController = null;

    //플레이어 움직임에 필요한 변수
    [SerializeField, Range(1f, 100f)] private float moveSpeed = 1f;

    #region 카메라회전에 필요한 변수
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

        //if(horizontalPower>270f)// 유니티에서 카메라의 회전값이 계속커지지않도록 작업해놓은거같음
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
        Vector3 cameraDir = playerCamera.transform.forward; //메인카메라 방향
        cameraDir.y = 0f;
        cameraDir.Normalize();

        float horizontalval = Input.GetAxis("Horizontal");
        float verticalval = Input.GetAxis("Vertical");

        float horizontalmovePower = horizontalval * moveSpeed * Time.deltaTime; //힘의크기
        float verticalmovePower = verticalval * moveSpeed * Time.deltaTime;

        Vector3 camerapos = cameraDir * verticalmovePower + Quaternion.Euler(0f, 90f, 0f) * cameraDir * horizontalmovePower + Physics.gravity; //쿼터니언 과 벡터의 곱에서 쿼터니언이 벡터뒤에 있으면 연산이 안됨 주의
        characterController.Move(camerapos);

        //transform.position += new Vector3(cameraDir.x * verticalmovePower + cameraDir.z * horizontalmovePower, 0f, cameraDir.z * verticalmovePower + -cameraDir.x * horizontalmovePower); //다른방법 벡터의 합을 모두 풀어서 쓰면이렇게됨
    }
}


