// Remy Pijuan

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPan : MonoBehaviour
{
    // The input action asset containing all actions related to camera movement
    [SerializeField] InputActionAsset cameraActions;

    // Camera pan actions
    InputAction possessAction;
    InputAction cameraPanAction;
    InputAction unpossessAction;
    
    // Camera zoom actions
    InputAction mouseWheelAction;
    InputAction touchContactAction;
    InputAction primaryFingerPosAction;
    InputAction secondaryFingerPosAction;

    bool isCursorPosInitialised = false;    // Initialises when the player clicks on the screen
    Camera orthoCam;
    float initialOrthoSize;
    Vector2 prevPos;                // Used to determine the direction of the camera's movement
    Vector2 panDistance;

    Vector2 primaryFingerPosition;
    Vector2 secondaryFingerPosition;
    float prevPinchDistance;
    float currentPinchDistance;

    [Header("Invert Camera Controls")]

    [SerializeField]
    [Tooltip("Invert Camera Controls when panning on the x-axis.")]
    bool InvertX = true;
    [SerializeField]
    [Tooltip("InvertCameraControls when panning on the y-axis.")]
    bool InvertY = true;


    [Header("Camera Movement Speed")]

    [SerializeField]
    [Tooltip("Controls the camera's pan speed.")]
    float panSpeed = 0.25f;
    float panSpeedTouch;
    float panSpeedMouse;

    [SerializeField]
    [Tooltip("Controls the camera's zoom speed.")]
    float zoomSpeed = 1f;
    float zoomSpeedTouch;
    float zoomSpeedMouse;


    [Header("Camera Limits")]

    [SerializeField]
    [Tooltip("The distance the camera is allowed to pan.")]
    Vector2 panLimit = new Vector2(10, 10);

    
    [Tooltip("The distance the camera is allowed to zoom in.")]
    public float minZoomDistance = 3.5f;

    [Tooltip("The distance the camera is allowed to zoom out.")]
    public float maxZoomDistance = 10f;

    // These function references are necessary for callback registering/deregistering to work properly
    Action<InputAction.CallbackContext> possessCamera;
    Action<InputAction.CallbackContext> unpossessCamera;
    Action<InputAction.CallbackContext> startPinchZoom;
    Action<InputAction.CallbackContext> stopPinchZoom;

    /** Set all initial variables and required callbacks */
    void Awake()
    {
        if (cameraActions)
        {
            possessAction = cameraActions.FindAction("PossessCamera");
            cameraPanAction = cameraActions.FindAction("PanCamera");
            unpossessAction = cameraActions.FindAction("UnpossessCamera");
            mouseWheelAction = cameraActions.FindAction("MouseWheelZoom");
            touchContactAction = cameraActions.FindAction("SecondaryTouchContact");
            primaryFingerPosAction = cameraActions.FindAction("PrimaryFingerPosition");
            secondaryFingerPosAction = cameraActions.FindAction("SecondaryFingerPosition");
        }

        if (gameObject != null)
        {
            possessCamera = ctx => PossessCamera();
            unpossessCamera = ctx => UnpossessCamera();
            startPinchZoom = ctx => StartPinchZoom();
            stopPinchZoom = ctx => StopPinchZoom();

            possessAction.performed += possessCamera;
            unpossessAction.performed += unpossessCamera;
            mouseWheelAction.performed += MouseWheelZoom;
            touchContactAction.performed += startPinchZoom;
            touchContactAction.canceled += stopPinchZoom;
        }

        // Set device-dependent Zoom and Pan speeds
        zoomSpeedTouch = zoomSpeed * 0.2f;
        zoomSpeedMouse = zoomSpeed * 0.5f;

        panSpeedTouch = panSpeed * .5f;
        panSpeedMouse = panSpeed * .25f;
    }

    void Start()
    {
        if (orthoCam = FindAnyObjectByType<Camera>())
        {
            initialOrthoSize = orthoCam.orthographicSize;
        }
    }

    /** While the player is touching the screen, they can rotate the camera */
    void PossessCamera()
    {
        if (gameObject != null)
        {
            cameraPanAction.performed += PanCamera;
        }
    }

    /** Pan the camera when the player taps and drags the screen */
    void PanCamera(InputAction.CallbackContext context)
    {
        Vector2 currentPos = context.ReadValue<Vector2>();

        if (isCursorPosInitialised)
        {
            Vector3 deltaPos = currentPos - prevPos;

            // Invert controls as needed
            if (InvertX)
            {
                deltaPos.x = prevPos.x - currentPos.x;
            }
            
            if (InvertY)
            {
                deltaPos.y = prevPos.y - currentPos.y;
            }

            // Change Panning Speed based on device
            float deviceBasedPanSpeed = panSpeed;
            if (context.control.device.name == "Mouse")
                deviceBasedPanSpeed = panSpeedMouse;
            else if (context.control.device.name == "Touchscreen")
                deviceBasedPanSpeed = panSpeedTouch;

            // panning movement is scaled by a device-based speed as well as a ratio of the camera size
            deltaPos *= deviceBasedPanSpeed * (orthoCam.orthographicSize / initialOrthoSize);
            deltaPos = ClampedPan(deltaPos);

            // Transform.Translate applies the tranformation to local space by default
            // Movement along the X direction in screen space translates directly to movement along the X axis in local space
            transform.Translate(new Vector2(deltaPos.x, 0));
            
            // Apply screen space Y translate
            transform.Translate(GetMovementAlongPlaneXZ(deltaPos.y), Space.World);
        }
        else
        {
            isCursorPosInitialised = true;
        }

        prevPos = currentPos;
    }

    /** Clamps the camera's panning movement and returns an adjusted deltaPos */
    Vector2 ClampedPan(Vector2 deltaPos)
    {
        panDistance += deltaPos;

        if (panDistance.x > panLimit.x)
        {
            deltaPos.x -= panDistance.x - panLimit.x;
            panDistance.x = panLimit.x;
        }
        else if (panDistance.x < -panLimit.x)
        {
            deltaPos.x += -panDistance.x - panLimit.x;
            panDistance.x = -panLimit.x;
        }
        
        if (panDistance.y > panLimit.y)
        {
            deltaPos.y -= panDistance.y - panLimit.y;
            panDistance.y = panLimit.y;
        }
        else if (panDistance.y < -panLimit.y)
        {
            deltaPos.y += -panDistance.y - panLimit.y;
            panDistance.y = -panLimit.y;
        }

        return deltaPos;
    }

    /** Translates movement along the Y direction in screen space to movement along the XZ plane in world space */
    Vector3 GetMovementAlongPlaneXZ(float deltaScreenSpaceY)
    {
        // Rotate the vector along the X axis
        Vector3 PlaneVectorXZ = new Vector3(0, 0, deltaScreenSpaceY);

        // Then rotate along the Y axis
        PlaneVectorXZ = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * PlaneVectorXZ;

        return PlaneVectorXZ;
    }

    /** When the player lifts their finger from the screen, the camera stops moving */
    void UnpossessCamera()
    {
        if (gameObject != null)
        {
            cameraPanAction.performed -= PanCamera;
            isCursorPosInitialised = false;
        }
    }

    /** Applies camera zoom based on input from the mouse wheel */
    void MouseWheelZoom(InputAction.CallbackContext context)
    {
        float mouseWheelDirection = context.ReadValue<float>();

        if (mouseWheelDirection > 0)
        {
            Zoom(true, zoomSpeedMouse);
        }
        else if (mouseWheelDirection < 0)
        {
            Zoom(false, zoomSpeedMouse);
        }
    }

    void StartPinchZoom()
    {
        // Disable Camera Pan
        UnpossessCamera();

        // Enable Pinch Zoom
        if (primaryFingerPosAction != null && secondaryFingerPosAction != null)
        {
            primaryFingerPosAction.performed += ReadPrimaryFingerPosition;
            secondaryFingerPosAction.performed += ReadSecondaryFingerPosition;
        }
    }

    void StopPinchZoom()
    {
        // Disable Pinch Zoom
        if (primaryFingerPosAction !=null && secondaryFingerPosAction != null)
        {
            primaryFingerPosAction.performed -= ReadPrimaryFingerPosition;
            secondaryFingerPosAction.performed -= ReadSecondaryFingerPosition;
        }
    }

    /** Get the distance between the two fingers on the screen
    *   Compare to the last time distance was taken
    */
    void ReadPrimaryFingerPosition(InputAction.CallbackContext context)
    {
        prevPinchDistance = currentPinchDistance;
        primaryFingerPosition = context.ReadValue<Vector2>();
        currentPinchDistance = Vector2.Distance(primaryFingerPosition, secondaryFingerPosition);
        Zoom(IsPinchZoomIn(prevPinchDistance, currentPinchDistance), zoomSpeedTouch);
    }

    void ReadSecondaryFingerPosition(InputAction.CallbackContext context)
    {
        prevPinchDistance = currentPinchDistance;
        secondaryFingerPosition = context.ReadValue<Vector2>();
        currentPinchDistance = Vector2.Distance(primaryFingerPosition, secondaryFingerPosition);
        Zoom(IsPinchZoomIn(prevPinchDistance, currentPinchDistance), zoomSpeedTouch);
    }

    bool IsPinchZoomIn(float prevDistance, float currentDistance)
    {
        if (currentDistance - prevDistance < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /** Zooms the camera in and out
     *  This is for an orthographic camera
     *  Changes the size of the orthographic viewport
     */
    void Zoom(bool isZoomIn, float zoomSpeed)
    {
        if (orthoCam)
        {
            if (isZoomIn)
            {
                orthoCam.orthographicSize -= zoomSpeed;
                
                // Clamped min zoom distance
                if (orthoCam.orthographicSize < minZoomDistance)
                {
                    orthoCam.orthographicSize += zoomSpeed;
                }
            }
            else
            {
                orthoCam.orthographicSize += zoomSpeed;

                // Clamped max zoom distance
                if (orthoCam.orthographicSize > maxZoomDistance)
                {
                    orthoCam.orthographicSize = maxZoomDistance;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (possessAction != null)
        {
            possessAction.performed -= possessCamera;
        }

        if (unpossessAction != null)
        {
            unpossessAction.performed -= unpossessCamera;
        }

        if (mouseWheelAction != null)
        {
            mouseWheelAction.performed -= MouseWheelZoom;
        }

        if (touchContactAction != null)
        {
            touchContactAction.performed -= startPinchZoom;
            touchContactAction.canceled -= stopPinchZoom;
        }

        UnpossessCamera();
        StopPinchZoom();
    }
}