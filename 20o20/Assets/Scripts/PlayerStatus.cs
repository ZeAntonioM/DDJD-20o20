using System;
using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private GameObject Points;
    [SerializeField] private int Lives = 3;
    [SerializeField] private float detectionCooldown = 3f; // Cooldown duration in seconds
    private bool isOnCooldown = false;
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
        if (animation) animator.SetBool("isInvisible", isInvisible);
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

    public void DecreaseLife()
    {
        Debug.Log("Player detected! Decreasing life...");
        Debug.Log(GetLives());
        Lives--;
        Debug.Log("Lives remaining: " + Lives);

        if (Lives <= 0)
        {
            GameOver();
        } else{
            StartCooldown();
        }
    }
    public bool IsOnCooldown()
    {
        return isOnCooldown;
    }

    public void StartCooldown()
    {
        if (!isOnCooldown)
        {
            SetInvisibility(true, false);
            Debug.Log("Starting cooldown...");
            isOnCooldown = true;
            StartCoroutine(CooldownRoutine());
            StartCoroutine(FlashSpriteDuringCooldown());
        }
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(detectionCooldown);
        isOnCooldown = false;
        SetInvisibility(false, false);
        Debug.Log("Cooldown finished!");
    }

    private IEnumerator FlashSpriteDuringCooldown()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the player!");
            yield break;
        }

        while (isOnCooldown)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        spriteRenderer.enabled = true;
    }

    public void ResetLives(int value)
    {
        Lives = value;
    }

    public int GetLives()
    {
        return Lives;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        GameController gameController = GameObject.FindFirstObjectByType<GameController>();
        if (gameController != null)
        {
            gameController.GameOver();
        }
    }
}