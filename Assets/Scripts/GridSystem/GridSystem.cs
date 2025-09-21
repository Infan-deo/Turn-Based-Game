using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Diagnostics;

public class GridSystem
{
    int _width;
    int _height;
    int _cellsize;

    GridObject[,] gridObjectArray;


    public GridSystem(int width, int height, int cellsize)
    {
        _width = width;
        _height = height;
        _cellsize = cellsize;
        gridObjectArray = new GridObject[width, height];
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = new GridObject(this, gridPosition);

            }
        }

    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellsize;
    }


    public GridPosition GetGridPosition(Vector3 worldposition)
    {
        return new GridPosition(
           Mathf.RoundToInt(worldposition.x / _cellsize),
            Mathf.RoundToInt(worldposition.z / _cellsize)
        );
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }

    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
                gridPosition.z >= 0 &&
                gridPosition.x < _width &&
                gridPosition.z < _height;
    }

    public int Getwidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

}
