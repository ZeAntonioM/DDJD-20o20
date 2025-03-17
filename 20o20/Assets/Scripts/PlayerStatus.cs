using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{

    [SerializeField] private GameObject Points;
    [SerializeField] private GameObject Time;
    public bool isInvisible = false;
    public bool hasCard = false;
    private int p = 0;
    private string stringp = "000000";

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void SetInvisibility(bool value)
    {
        isInvisible = value;
        animator.SetBool("isInvisible", isInvisible);
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
        TimeLeft timeComponent = Time.GetComponent<TimeLeft>();
        return timeComponent.GetTimeTakenInSeconds();
    }

    public float GetTotalTime()
    {
        TimeLeft timeComponent = Time.GetComponent<TimeLeft>();
        return timeComponent.timeToComplete;
    }


}
