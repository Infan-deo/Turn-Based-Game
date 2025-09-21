using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    private GridObject _gridObject;
    

    [SerializeField] private TextMeshPro _textMeshPro;

    public void SetGridObject(GridObject gridObject)
    {
        _gridObject = gridObject;


    }

    private void Update()
    {
        UpdatePositionText();
    }

    public void UpdatePositionText()
    {
        _textMeshPro.text = _gridObject.ToString();
    }
}
