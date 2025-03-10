using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private  float floatAmplitude = 0.075f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
