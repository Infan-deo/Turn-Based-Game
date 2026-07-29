using System;
using System.Linq;
using UnityEngine;
using Unity.Cinemachine;

public struct SetCameraPoint : IEvent
{
    public int Index;
    public Transform Target;
}
public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private GameObject actionCameraGameObject;
    public Transform SequencerCameraParent;
    private Transform[] SequencerCameraChild;
    public Transform mainCamera;

    EventBinding<SetCameraPoint> setCameraPointEvent;
    protected override void Awake()
    {
        base.Awake();
        SequencerCameraChild = SequencerCameraParent
            .GetComponentsInChildren<Transform>()
            .Where(t => t != SequencerCameraParent)
            .ToArray();
    }
    private void OnEnable()
    {
        setCameraPointEvent = new EventBinding<SetCameraPoint>(OnSetCameraPoint);
        EventBus<SetCameraPoint>.Register(setCameraPointEvent);
        ShootAction.OnAnyParryTimingStarted += SetCameraForParry;
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }
    private void Start()
    {
        HideActionCamera();
    }
    private void OnDisable()
    {
        EventBus<SetCameraPoint>.Deregister(setCameraPointEvent);
        ShootAction.OnAnyParryTimingStarted -= SetCameraForParry;
        BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
    }
    private void OnSetCameraPoint(SetCameraPoint point)
    {
        SetSequenceChildCameraPoint(point.Index, point.Target);
    }    
    private void SetCameraForParry(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                if (shootAction is IParryable parryable &&
                parryable.isThisActionParryableNow())
                {
                    Unit targetUnit = shootAction.GetTargetUnit();
                    SetCameraAtUnitCameraPoint(targetUnit, 2, actionCameraGameObject.transform);
                    ShowActionCamera();
                }
                break;
            default:
                break;

        }

    }

    public void SetActionCameraAtUnitCameraPoint(Unit targetunit, int cameraPointIndex)
    {
         SetCameraAtUnitCameraPoint(targetunit, cameraPointIndex, actionCameraGameObject.transform);
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;

        }
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                if (shootAction is IParryable parryable &&
                parryable.isThisActionParryableNow())
                {
                   
                }
                else
                {
                    Unit shooterUnit = shootAction.GetSelectedUnit();
                    Unit targetUnit = shootAction.GetTargetUnit();
                    Vector3 cameraCharacterHeight = Vector2.up * 1.7f;
                    Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                    float shoulderOffsetAmount = 0.5f;
                    Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;
                    Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);
                    actionCameraGameObject.transform.position = actionCameraPosition;
                    actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                    ShowActionCamera();
                }
                break;

        }
    }

    public void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }
    public void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    public void SetSequenceChildCameraPoint(int index, Transform targetTransform, float HoldDuration = 1.0f)
    {
        Transform childCameraPoint = SequencerCameraChild[index];
        childCameraPoint.transform.position = targetTransform.position;
        childCameraPoint.transform.rotation = targetTransform.rotation;
    }

    public void SetSequenceCameraState(bool state)
    {
        SequencerCameraParent.gameObject.SetActive(state);
    }

    public void SetCameraAtUnitCameraPoint(Unit targetunit, int cameraPointIndex, Transform CameraTransform)
    {
        Transform cameraPoint = targetunit.GetUnitCameraTransform()[cameraPointIndex];
        CameraTransform.transform.position = cameraPoint.position;
        CameraTransform.transform.rotation = cameraPoint.rotation;
    }


}
