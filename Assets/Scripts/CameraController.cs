using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CinemachineFollow cinemachineFollow;
    Vector3 targetFollowOffset;
    private void Start()
    {
        targetFollowOffset = cinemachineFollow.FollowOffset;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1;
        }

        float moveSpeed = 10f;
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
    private void HandleRotation()
    {
        Vector3 rotateDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            rotateDir.y = -1;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotateDir.y = +1;
        }

        float rotateSpeed = 100f;
        transform.eulerAngles += rotateDir * rotateSpeed * Time.deltaTime;
    }
    private void HandleZoom()
    {

        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, 2, 12);
        cinemachineFollow.FollowOffset = Vector3.Lerp(cinemachineFollow.FollowOffset, targetFollowOffset, Time.deltaTime * 5f);
    }
}
