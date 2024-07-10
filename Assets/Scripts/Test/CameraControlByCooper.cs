using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

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

        if(Keyboard.current.wKey.isPressed )
        {
            inputMoveDir.z = +1f;
        }        
        if(Keyboard.current.sKey.isPressed  /*((Mouse.current.position.ReadValue().y<0 && Mouse.current.position.ReadValue().y>-1)&&
                                                (Mouse.current.position.ReadValue().x<0 && Mouse.current.position.ReadValue().y<0))*/)
        {
            inputMoveDir.z = -1f;
        }   
        if(Keyboard.current.aKey.isPressed || Mouse.current.position.ReadValue().x<0 && Mouse.current.position.ReadValue().x>-1)
        {
            inputMoveDir.x = -1f;
        }    
        if(Keyboard.current.dKey.isPressed || (Mouse.current.position.ReadValue().x>0 && Mouse.current.position.ReadValue().x<1))
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

        if(Keyboard.current.qKey.isPressed)
        {
            rotateVector.y = +1f;
        }    
        if(Keyboard.current.eKey.isPressed)
        {
            rotateVector.y = -1f;
        }      

        float rotateSpeed = 100f;
        transform.eulerAngles += rotateVector * rotateSpeed * Time.deltaTime;
    }
    private void CameraZoom()
    {
         float zoomAmount = 1f;
        if (Mouse.current.scroll.y.ReadValue()>0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        if (Mouse.current.scroll.y.ReadValue()<0)
        {
            targetFollowOffset.y += zoomAmount;
        }

        float zoomSpeed = 5f;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y,MIN_FOLLOW_Y_OFFSET,MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset,Time.deltaTime * zoomSpeed);
    }
}
