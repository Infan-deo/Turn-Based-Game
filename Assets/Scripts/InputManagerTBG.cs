#define USE_NEW_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;


public class InputManagerTBG : Singleton<InputManagerTBG>
{
    private PlayerInputAction inputActions;
    private void Start()
    {
        inputActions = new PlayerInputAction();
        inputActions.Player.Enable();
    }
    public Vector3 GetMousePosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
    return Input.mousePosition;
#endif
    }

    public bool GetMouseButtonDownThisFrame()
    {
        #if USE_NEW_INPUT_SYSTEM
        return inputActions.Player.MouseClick.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
        #endif
    }

    public Vector2 GetInputMoveDir()
    {

#if USE_NEW_INPUT_SYSTEM
        return inputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
   

        Vector2 inputMoveDir = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y = +1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1;
        }
        return inputMoveDir;

#endif
    }

    public float GetCameraRotationAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return inputActions.Player.CameraRotation.ReadValue<float>();
#else
        float rotateAmt = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotateAmt = -1;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotateAmt = +1;
        }

        return rotateAmt;
#endif
    }
    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return inputActions.Player.CameraZoom.ReadValue<float>();
#else
        float zoomAmount = 0f;
        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = -1;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = 1;
        }

        return zoomAmount;
#endif
    }
}
