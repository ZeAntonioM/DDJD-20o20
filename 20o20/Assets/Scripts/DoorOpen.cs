using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private Animator animator;
    private int charactersInTrigger = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Guard"))
        {
            charactersInTrigger++;
            if (charactersInTrigger == 1)
            {
                animator.SetBool("Opening", true);
                animator.SetBool("isOpen", true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Guard"))
        {
            charactersInTrigger--;
            if (charactersInTrigger == 0)
            {
                animator.SetBool("Opening", false);
                animator.SetBool("isOpen", false);
            }
        }
    }
}
