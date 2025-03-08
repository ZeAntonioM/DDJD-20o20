using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public bool isInvisible = false;
    public bool hasCard = false;

    public void SetInvisibility(bool value)
    {
        isInvisible = value;
    }

    public void SetCard(bool value)
    {
        hasCard = value;
    }
}
