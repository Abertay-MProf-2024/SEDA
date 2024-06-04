using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;
    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }
    private void Update()
    {
        CameraMovement();
        CameraRotation();
        CameraZoom();
    }

    private void CameraMovement()
    {
        Vector3 inputMoveDir = new Vector3(0,0,0);

        if(Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }        
        if(Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }   
        if(Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }    
        if(Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }          

        float moveSpeed = 5f;

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;

        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
    private void CameraRotation()
    {
         Vector3 rotateVector = new Vector3(0,0,0);
        if(Input.GetKey(KeyCode.Q))
        {
            rotateVector.y = +1f;
        }    
        if(Input.GetKey(KeyCode.E))
        {
            rotateVector.y = -1f;
        }      

        float rotateSpeed = 100f;
        transform.eulerAngles += rotateVector * rotateSpeed * Time.deltaTime;
    }
    private void CameraZoom()
    {
         float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y >0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y <0)
        {
            targetFollowOffset.y += zoomAmount;
        }

        float zoomSpeed = 5f;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y,MIN_FOLLOW_Y_OFFSET,MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset,Time.deltaTime * zoomSpeed);
    }
}