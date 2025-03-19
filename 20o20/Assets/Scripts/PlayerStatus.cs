using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private GameObject Points;
    public bool isInvisible = false;
    public bool doorAnimation = false;
    public bool hasCard = false;
    private int p = 0;
    private string stringp = "000000";

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetInvisibility(bool value, bool animation = true)
    {
        isInvisible = value;
        if(animation) animator.SetBool("isInvisible", isInvisible);
    }

    public void SetDoorAnimation(bool value)
    {
        doorAnimation = value;
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

    public float GetTimeTaken()
    {
        TimeLeft timeComponent = GameObject.FindFirstObjectByType<TimeLeft>();
        if (timeComponent != null)
        {
            return timeComponent.GetTimeTakenInSeconds();
        }
        else
        {
            Debug.LogError("TimeLeft component not found!");
            return 0f;
        }
    }
}