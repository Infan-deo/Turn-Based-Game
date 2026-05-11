using System;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{

    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private MeshRenderer meshRenderer;
    private GridPosition gridPosition;
    private bool isGreen;
    public Action OnInteractComplete;
    private bool IsActive;
    private float timer;
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        if (isGreen)
        {
            SetMaterialToGreen();
        }
        else
        {
            SetMaterialToRed();
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
        if (!isGreen)
        {
            SetMaterialToGreen();
        }
        else
        {
            SetMaterialToRed();
        }
    }

    public void SetMaterialToGreen()
    {
        isGreen = true;

        meshRenderer.material = greenMaterial;
    }

    public void SetMaterialToRed()
    {
        isGreen = false;

        meshRenderer.material = redMaterial;
    }


}
