using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    [SerializeField] private GameObject Points;
    public bool isInvisible = false;
    public bool hasCard = false;
    private int p = 0;
    private string stringp = "000000";


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
        p += value;
        stringp = p.ToString("D6");
        Points.GetComponent<TMPro.TextMeshProUGUI>().text = "Points: " + stringp;
    }

    public int GetPoints()
    {
        return p;
    }

}
