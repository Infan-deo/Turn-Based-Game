using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber;
    public static TurnSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void NextTurn()
    {
        turnNumber++;
    }
}
