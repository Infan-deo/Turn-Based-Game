using System;
using Ami.BroAudio;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    private GridPosition gridPosition;
    private Animator animator;
    private bool isOpen;
    public Action OnInteractComplete;
    private bool IsActive;

    private float timer;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void Update()
    {
        if (!IsActive)
        {
            return;
        }
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            OnInteractComplete?.Invoke();
            IsActive = false;
        }
    }

    public void Interact(Action OnInteractComplete)
    {
        this.OnInteractComplete = OnInteractComplete;
        IsActive = true;
        timer = .7f;
        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);
        BroAudio.Play(SFXGameManager.Instance.DoorOpenSound);
        Pathfinding.Instance.SetisWalkableGridPostion(gridPosition, isOpen);
    }

    public void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);
        BroAudio.Play(SFXGameManager.Instance.DoorCloseSound);
        Pathfinding.Instance.SetisWalkableGridPostion(gridPosition, isOpen);
    }
}
