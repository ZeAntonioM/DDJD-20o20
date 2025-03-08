using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public bool isInvisible = false;
    public bool hasCard = false;
    private int points = 0;

    public void SetInvisibility(bool value)
    {
        isInvisible = value;
    }

    public void SetCard(bool value)
    {
        hasCard = value;
    }

    public void AddPoints(int value)
    {
        points += value;
    }

    public int GetPoints()
    {
        return points;
    }

}
