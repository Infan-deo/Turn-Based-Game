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

        Vector2 inputMoveDir = InputManagerTBG.Instance.GetInputMoveDir();


        float moveSpeed = 10f;
        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
    private void HandleRotation()
    {
        Vector3 rotateDir = new Vector3(0, 0, 0);

        rotateDir.y = InputManagerTBG.Instance.GetCameraRotationAmount();

        float rotateSpeed = 100f;
        transform.eulerAngles += rotateDir * rotateSpeed * Time.deltaTime;
    }
    private void HandleZoom()
    {

        float zoomIncreaseAmount = 1f;

        targetFollowOffset.y += InputManagerTBG.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, 2, 12);
        cinemachineFollow.FollowOffset = Vector3.Lerp(cinemachineFollow.FollowOffset, targetFollowOffset, Time.deltaTime * 5f);
    }
}
